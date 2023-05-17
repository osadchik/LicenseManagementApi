using System.ComponentModel.DataAnnotations;

namespace Resiliency.CircuitBreaker
{
    public class ClosedStateConfig
    {
        public int ErrorThreshold { get; }
        public TimeSpan SamplingDuration { get; }

        public ClosedStateConfig([Range(0, int.MaxValue)] int errorThreshold, TimeSpan samplingDuration)
        {
            if (errorThreshold < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(errorThreshold), errorThreshold, "Error threshold must be a non-negative integer.");
            }
            ErrorThreshold = errorThreshold;

            if (samplingDuration <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(samplingDuration), samplingDuration, "Sampling duration must be greater than zero.");
            }
            SamplingDuration = samplingDuration;
        }
    }
}