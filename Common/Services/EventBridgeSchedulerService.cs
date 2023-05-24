using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using Common.Interfaces;
using Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace Common.Services
{
    /// <summary>
    /// Event Bridge Scheduler service.
    /// </summary>
    public class EventBridgeSchedulerService : IEventBridgeSchedulerService
    {
        private readonly IAmazonEventBridge _amazonEventBridge;
        private readonly EventBridgeOptions _eventBridgeOptions;
        private readonly ILogger<EventBridgeSchedulerService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="EventBridgeSchedulerService"/> class.
        /// </summary>
        /// <param name="amazonEventBridge"><see cref="IAmazonEventBridge"/></param>
        /// <param name="eventBridgeOptions"><see cref="EventBridgeOptions"/></param>
        /// <param name="logger">Logger instance.</param>
        public EventBridgeSchedulerService(IAmazonEventBridge amazonEventBridge, IOptions<EventBridgeOptions> eventBridgeOptions, ILogger<EventBridgeSchedulerService> logger)
        {
            _amazonEventBridge = amazonEventBridge;
            _eventBridgeOptions = eventBridgeOptions.Value;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Task CancelCircuitClosureTrialAsync()
        {
            return ChangeEventBridgeRuleStateAsync(RuleState.DISABLED, "rate(365 days)");
        }


        /// <inheritdoc/>
        public Task ScheduleCircuitClosureTrialAsync(DateTimeOffset trialDate)
        {
            return ChangeEventBridgeRuleStateAsync(RuleState.ENABLED, GetCronExpression(trialDate));
        }

        private async Task ChangeEventBridgeRuleStateAsync(RuleState state, string scheduleExpression)
        {
            _logger.LogDebug("Changing EventBridge {rule} rule to state {state}", _eventBridgeOptions.RuleName, state);
            var request = new PutRuleRequest
            {
                Name = _eventBridgeOptions.RuleName,
                ScheduleExpression = scheduleExpression,
                State = state
            };

            PutRuleResponse? response = await _amazonEventBridge.PutRuleAsync(request);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"Failed to change EventBridge {_eventBridgeOptions.RuleName} rule to {state} state", null, response.HttpStatusCode);
            }
            _logger.LogInformation("Changed EventBridge {rule} rule to {state} state", _eventBridgeOptions.RuleName, state);
        }

        private static string GetCronExpression(DateTimeOffset trialDate)
        {
            return trialDate - DateTimeOffset.UtcNow <= TimeSpan.FromMinutes(1)
                ? "cron(0/1 * * * ? *)"
                : $"cron({trialDate.Minute} {trialDate.Hour} {trialDate.Day} * ? *)";
        }
    }
}
