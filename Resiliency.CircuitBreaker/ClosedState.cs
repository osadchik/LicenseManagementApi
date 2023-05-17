using System.ComponentModel.DataAnnotations;

namespace Resiliency.CircuitBreaker
{
    public class ClosedState : CircuitState
    {
        private readonly OpenStateConfig _openStateConfig;
        private readonly ClosedStateConfig _closedStateConfig;

        public int ErrorCounter { get; private set; }
        public DateTimeOffset StartedAt { get; private set; }

        public ClosedState(OpenStateConfig openStateConfig, ClosedStateConfig closedStateConfig) :
            this(0, DateTimeOffset.UtcNow, openStateConfig, closedStateConfig)
        { }

        public ClosedState([Range(0, int.MaxValue)] int errorCounter, DateTimeOffset startedAt, OpenStateConfig openStateConfig, ClosedStateConfig closedStateConfig)
        {
            if (errorCounter < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(errorCounter), errorCounter, "Error counter must be a non-negative integer.");
            }
            ErrorCounter = errorCounter;

            if (startedAt > DateTimeOffset.UtcNow)
            {
                throw new ArgumentOutOfRangeException(nameof(startedAt), startedAt, "Sampling start date must be in the past.");
            }
            StartedAt = startedAt;

            _openStateConfig = openStateConfig ?? throw new ArgumentNullException(nameof(openStateConfig));
            _closedStateConfig = closedStateConfig ?? throw new ArgumentNullException(nameof(closedStateConfig));
        }

        public override void Accept(ICircuitStateVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }

        internal override CircuitState RecordError()
        {
            if (DateTimeOffset.UtcNow - StartedAt < _closedStateConfig.SamplingDuration)
            {
                if(++ErrorCounter > _closedStateConfig.ErrorThreshold)
                {
                    return new OpenState(_openStateConfig, _closedStateConfig);
                }
            }
            else
            {
                ErrorCounter = 1;
                StartedAt = DateTimeOffset.UtcNow;
            }

            return this;
        }

        internal override async Task<CircuitState> ExecuteAsync(Func<Task> action)
        {
            await action();

            return this;
        }

        internal override async Task<ExecutionResult<TResult>> ExecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            TResult result = await action();

            return new ExecutionResult<TResult>(this, result);
        }
    }
}