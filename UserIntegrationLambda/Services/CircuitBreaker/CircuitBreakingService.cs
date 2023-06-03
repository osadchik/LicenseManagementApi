using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.SQS.Model;
using Common.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resiliency.CircuitBreaker;
using System.Net;
using UserIntegrationLambda.Interfaces.CircuitBreaker;
using UserIntegrationLambda.Options;
using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace UserIntegrationLambda.Services.CircuitBreaker
{
    /// <summary>
    /// Circuit Breaker service adapter.
    /// </summary>
    public class CircuitBreakingService : ICircuitBreakingService
    {
        private readonly ICircuitStateRepository _circuitStateRepository;
        private readonly IEventBridgeSchedulerService _circuitBreakerClosureScheduler;
        private readonly IEventSourceMappingClient _eventSourceMappingClient;
        private readonly CircuitBreakerOptions _circuitBreakerOptions;
        private readonly ISqsClient _sqsClient;
        private readonly ILogger<CircuitBreakingService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="CircuitBreakingService"/> class.
        /// </summary>
        /// <param name="circuitStateRepository">Circuit state datastore.</param>
        /// <param name="circuitBreakerClosureScheduler">Service to work with Event Bridge Schedule.</param>
        /// <param name="eventSourceMappingClient">Service to manage lambda event source.</param>
        /// <param name="circuitBreakerOptions"><see cref="CircuitBreakerOptions"/></param>
        /// <param name="sqsClient"></param>
        /// <param name="logger">Logger instance.</param>
        public CircuitBreakingService(ICircuitStateRepository circuitStateRepository,
            IEventBridgeSchedulerService circuitBreakerClosureScheduler, 
            IEventSourceMappingClient eventSourceMappingClient,
            IOptions<CircuitBreakerOptions> circuitBreakerOptions,
            ISqsClient sqsClient,
            ILogger<CircuitBreakingService> logger)
        {
            _circuitStateRepository = circuitStateRepository;
            _circuitBreakerClosureScheduler = circuitBreakerClosureScheduler;
            _eventSourceMappingClient = eventSourceMappingClient;
            _circuitBreakerOptions = circuitBreakerOptions.Value;
            _sqsClient = sqsClient;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(SQSMessage sqsMessage, Func<SQSMessage, Task> action)
        {
            CircuitState currentState = await _circuitStateRepository.GetAsync();
            CircuitBreakerPolicy circuitBreakerPolicy = new(currentState);

            try
            {
                await circuitBreakerPolicy
                    .Handle(IsTransientError)
                    .ExecuteAsync(() => action(sqsMessage));
            }
            catch (OpenCircuitException openCircuitException)
            {
                await _eventSourceMappingClient.DisableEventSourceMappingAsync();
                await _circuitBreakerClosureScheduler.ScheduleCircuitClosureTrialAsync(openCircuitException.OpenUntil);
                await _sqsClient.EnqueueAsync(sqsMessage, _circuitBreakerOptions.SourceQueueUrl);
            }
            catch (Exception ex) when (IsPermanentError(ex))
            {
                _logger.LogError(ex, "Failed to handle SQS message due to permanent error");
                await _sqsClient.EnqueueAsync(sqsMessage, _circuitBreakerOptions.DeadLetterQueueUrl);
            }
            finally
            {
                await _circuitStateRepository.SaveAsync(circuitBreakerPolicy.State);
            }
        }

        /// <inheritdoc/>
        public async Task Close()
        {
            _logger.LogDebug("Switching the CB state to closed.");
            await _eventSourceMappingClient.EnableEventSourceMappingAsync();
            await _circuitBreakerClosureScheduler.CancelCircuitClosureTrialAsync();
            OpenStateConfig openStateConfig = new(_circuitBreakerOptions.Timeout);
            ClosedStateConfig closedStateConfig = new(_circuitBreakerOptions.ErrorThreshold, _circuitBreakerOptions.SamplingDuration);
            var circuitState = new ClosedState(openStateConfig, closedStateConfig);
            await _circuitStateRepository.SaveAsync(circuitState);
        }

        /// <inheritdoc/>
        public async Task HalfOpen()
        {
            _logger.LogDebug("Switching the CB state to half-open.");
            await _eventSourceMappingClient.EnableEventSourceMappingAsync();
            await _circuitBreakerClosureScheduler.CancelCircuitClosureTrialAsync();
            OpenStateConfig openStateConfig = new(_circuitBreakerOptions.Timeout);
            ClosedStateConfig closedStateConfig = new(_circuitBreakerOptions.ErrorThreshold, _circuitBreakerOptions.SamplingDuration);
            var circuitState = new HalfOpenState(openStateConfig, closedStateConfig);
            await _circuitStateRepository.SaveAsync(circuitState);
        }

        /// <inheritdoc/>
        public async Task Open(TimeSpan? customTimeout)
        {
            _logger.LogDebug("Switching the CB state to open.");
            await _eventSourceMappingClient.EnableEventSourceMappingAsync();
            TimeSpan timeout = customTimeout ?? _circuitBreakerOptions.Timeout;
            await _circuitBreakerClosureScheduler.ScheduleCircuitClosureTrialAsync(DateTimeOffset.UtcNow + timeout);
            OpenStateConfig openStateConfig = new(timeout);
            ClosedStateConfig closedStateConfig = new(_circuitBreakerOptions.ErrorThreshold, _circuitBreakerOptions.SamplingDuration);
            OpenState circuitState = new(openStateConfig, closedStateConfig);
            await _circuitStateRepository.SaveAsync(circuitState);
        }

        /// <inheritdoc/>
        public async Task PermanentlyClose()
        {
            _logger.LogDebug("Switching the CB state to permanently closed.");
            await _eventSourceMappingClient.EnableEventSourceMappingAsync();
            await _circuitBreakerClosureScheduler.CancelCircuitClosureTrialAsync();
            var circuitState = new PermanentlyClosedState();
            await _circuitStateRepository.SaveAsync(circuitState);
        }

        /// <inheritdoc/>
        public async Task Trial(Func<SQSMessage, Task> action)
        {
            _logger.LogDebug("Starting Circuit Breaker trial.");

            Message? message = await _sqsClient.DequeueAsync(_circuitBreakerOptions.SourceQueueUrl);
            _logger.LogDebug("Retrieved the message: {@message} from the source queue", message);
            
            if (message is not null)
            {
                var sqsMessage = new SQSMessage
                {
                    Attributes = message.Attributes,
                    Body = message.Body,
                    MessageId = message.MessageId,
                    ReceiptHandle = message.ReceiptHandle
                };

                try
                {
                    await ExecuteAsync(sqsMessage, action);

                    await _eventSourceMappingClient.EnableEventSourceMappingAsync();
                    await _circuitBreakerClosureScheduler.CancelCircuitClosureTrialAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Circuit Breaker Trial has failed.");
                    await _sqsClient.EnqueueAsync(sqsMessage, _circuitBreakerOptions.SourceQueueUrl);
                    await _circuitBreakerClosureScheduler.ScheduleCircuitClosureTrialAsync(DateTimeOffset.UtcNow + _circuitBreakerOptions.Timeout);
                }
            }
            else
            {
                _logger.LogInformation("No messages found in the source queue. Switching the state to half-open.");
                await HalfOpen();
            }
        }

        private static bool IsTransientError(Exception exception)
        {
            var transientErrorCodes = new HashSet<HttpStatusCode>
            {
                HttpStatusCode.NotFound,
                HttpStatusCode.RequestTimeout,
                HttpStatusCode.TooManyRequests,
                HttpStatusCode.BadGateway,
                HttpStatusCode.ServiceUnavailable,
                HttpStatusCode.GatewayTimeout
            };

            return exception is ResourceNotFoundException or AmazonDynamoDBException;
        }

        private static bool IsPermanentError(Exception exception)
        {
            var permanentErrorCodes = new HashSet<HttpStatusCode>
            {
                HttpStatusCode.BadRequest,
                HttpStatusCode.Forbidden,
                HttpStatusCode.Unauthorized,
                HttpStatusCode.MethodNotAllowed,
                HttpStatusCode.Conflict,
                HttpStatusCode.RequestEntityTooLarge
            };

            return exception is ValidationException;
        }
    }
}
