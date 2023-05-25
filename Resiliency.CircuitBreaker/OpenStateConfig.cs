namespace Resiliency.CircuitBreaker
{
    public class OpenStateConfig
    {
        private const int DefaultUpperLimitInMinutes = 30;

        /// <summary>
        /// The time frame circuit stays open until next trial.
        /// </summary>
        public TimeSpan Timeout { get; }

        public OpenStateConfig(TimeSpan timeout)
        {
            TimeSpan upperLimit = TimeSpan.FromMinutes(DefaultUpperLimitInMinutes);
            if (timeout <= TimeSpan.Zero || timeout > upperLimit)
            {
                throw new ArgumentOutOfRangeException(nameof(timeout), timeout, $"Timeout must be positive and less than {upperLimit}.");
            }
            Timeout = timeout;
        }
    }
}