namespace Resiliency.CircuitBreaker
{
    public class HalfOpenState : CircuitState
    {
        private readonly OpenStateConfig _openStateConfig;
        private readonly ClosedStateConfig _closedStateConfig;

        public HalfOpenState(OpenStateConfig openStateConfig, ClosedStateConfig closedStateConfig)
        {
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
            return new OpenState(_openStateConfig, _closedStateConfig);
        }

        internal override async Task<CircuitState> ExecuteAsync(Func<Task> action)
        {
            await action();

            return new ClosedState(_openStateConfig, _closedStateConfig);
        }

        internal override async Task<ExecutionResult<TResult>> ExecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            TResult result = await action();

            return new ExecutionResult<TResult>(new ClosedState(_openStateConfig, _closedStateConfig), result);
        }
    }
}