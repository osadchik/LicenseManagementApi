using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resiliency.CircuitBreaker;
using UserIntegrationLambda.Interfaces.CircuitBreaker;
using UserIntegrationLambda.Models;
using UserIntegrationLambda.Options;

namespace UserIntegrationLambda.Repository
{
    /// <summary>
    /// Circuit breaker state datastore.
    /// </summary>
    public class CircuitStateRepository : ICircuitStateRepository
    {
        private static readonly string StateId = "7b5523ce-4b60-441d-98d5-a23bde47f7ae";

        private readonly ICircuitStateToDatabaseDtoMapper _dbMapper;
        private readonly IDynamoDBContext _dynamoDBContext;
        private readonly CircuitBreakerOptions _options;
        private readonly ILogger<CircuitStateRepository> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="CircuitStateRepository"/> class.
        /// </summary>
        /// <param name="dbMapper"><see cref="ICircuitStateToDatabaseDtoMapper"/></param>
        /// <param name="dynamoDBContext"><see cref="IDynamoDBContext"/></param>
        /// <param name="circuitBreakerOptions"><see cref="CircuitBreakerOptions"/></param>
        /// <param name="logger">Logger instance.</param>
        public CircuitStateRepository(ICircuitStateToDatabaseDtoMapper dbMapper, IDynamoDBContext dynamoDBContext, IOptions<CircuitBreakerOptions> circuitBreakerOptions, ILogger<CircuitStateRepository> logger)
        {
            _dbMapper = dbMapper;
            _dynamoDBContext = dynamoDBContext;
            _options = circuitBreakerOptions.Value;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<CircuitState> GetAsync()
        {
            var openStateConfig = new OpenStateConfig(_options.Timeout);
            var closedStateConfig = new ClosedStateConfig(_options.ErrorThreshold, _options.SamplingDuration);

            try
            {
                CircuitStateDatabaseDto databaseDto = await _dynamoDBContext.LoadAsync<CircuitStateDatabaseDto>(StateId) ?? GetClosedStateDto();

                CircuitState circuitState = databaseDto.StateId switch
                {
                    "Open" => new OpenState(databaseDto.OpenedAt.GetValueOrDefault(DateTime.UtcNow), openStateConfig, closedStateConfig),
                    "HalfOpen" => new HalfOpenState(openStateConfig, closedStateConfig),
                    "Closed" => GetClosedState(databaseDto, openStateConfig, closedStateConfig),
                    "PermanentlyClosed" => new PermanentlyClosedState(),
                    _ => throw new NotSupportedException($"{databaseDto.StateId} is not supported.")
                };

                return circuitState;
            }
            catch (AmazonDynamoDBException ex)
            {
                _logger.LogCritical(ex, "Failed to load circuit state from database. Falling back to default (Closed) state.");
                CircuitStateDatabaseDto closedStateDto = GetClosedStateDto();
                return GetClosedState(closedStateDto, openStateConfig, closedStateConfig);
            }
        }

        /// <inheritdoc/>
        public Task SaveAsync(CircuitState circuitState)
        {
            throw new NotImplementedException();
        }

        private static CircuitStateDatabaseDto GetClosedStateDto()
        {
            return new CircuitStateDatabaseDto
            {
                Id = StateId,
                StateId = "Closed"
            };
        }

        private static ClosedState GetClosedState(CircuitStateDatabaseDto databaseDto, OpenStateConfig openStateConfig, ClosedStateConfig closedStateConfig)
        {
            return new ClosedState(databaseDto.ErrorCounter.GetValueOrDefault(0), databaseDto.StartedAt.GetValueOrDefault(DateTime.UtcNow), openStateConfig, closedStateConfig);
        }
    }
}
