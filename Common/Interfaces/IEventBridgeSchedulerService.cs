namespace Common.Interfaces
{
    /// <summary>
    /// Interface for Event Bridge Scheduler service.
    /// </summary>
    public interface IEventBridgeSchedulerService
    {
        /// <summary>
        /// Sets Event Brdige rule state to DISABLED.
        /// </summary>
        /// <returns></returns>
        Task CancelCircuitClosureTrialAsync();

        /// <summary>
        /// Sets Event Bridge rule state to ENABLED.
        /// </summary>
        /// <returns></returns>
        Task ScheduleCircuitClosureTrialAsync(DateTimeOffset trialDate);
    }
}