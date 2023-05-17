namespace Resiliency.CircuitBreaker
{
    public class OpenState : CircuitState
    {
        private readonly OpenStateConfig _openStateConfig;
        private readonly ClosedStateConfig _closedStateConfig;

        public DateTimeOffset OpenedAt { get; }

        public OpenState(OpenStateConfig openStateConfig, ClosedStateConfig closedStateConfig) : this(DateTimeOffset.UtcNow, openStateConfig, closedStateConfig)
        { }

        public OpenState(DateTimeOffset openedAt, OpenStateConfig openStateConfig, ClosedStateConfig closedStateConfig)
        {
            if (openedAt > DateTimeOffset.UtcNow)
            {
                throw new ArgumentOutOfRangeException(nameof(openedAt), openedAt, "Opening date must be in the past.");
            }

            OpenedAt = openedAt;
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

        internal override async Task<CircuitState> ExecuteAsync(Func<Task> action)
        {
            CircuitState state = EvaluateNextState();
            await action();
            return state;
        }

        internal override async Task<ExecutionResult<TResult>> ExecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            CircuitState state = EvaluateNextState();
            TResult result = await action();
            return new ExecutionResult<TResult>(state, result);
        }

        internal override CircuitState RecordError()
        {
            return this;
        }

        private CircuitState EvaluateNextState()
        {
            DateTimeOffset openUntil = OpenedAt.Add(_openStateConfig.Timeout);
            if (openUntil <= DateTimeOffset.UtcNow)
            {
                return new HalfOpenState(_openStateConfig, _closedStateConfig);
            }

            throw new OpenCircuitException(openUntil);
        }
    }
}