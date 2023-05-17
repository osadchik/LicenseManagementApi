namespace Resiliency.CircuitBreaker
{
    public class PermanentlyClosedState : CircuitState
    {
        public DateTimeOffset StartedAt { get; private set; }

        public PermanentlyClosedState() : this(DateTimeOffset.UtcNow) { }

        public PermanentlyClosedState(DateTimeOffset startedAt)
        {
            if (startedAt > DateTimeOffset.UtcNow)
            {
                throw new ArgumentOutOfRangeException(nameof(startedAt), startedAt, "Starting date must be in the past.");
            }

            StartedAt = startedAt;
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