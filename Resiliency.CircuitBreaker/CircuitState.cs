namespace Resiliency.CircuitBreaker
{
    public abstract class CircuitState
    {
        public abstract void Accept(ICircuitStateVisitor visitor);

        internal abstract CircuitState RecordError();
        internal abstract Task<CircuitState> ExecuteAsync(Func<Task> action);
        internal abstract Task<ExecutionResult<TResult>> ExecuteAsync<TResult>(Func<Task<TResult>> action);
    }
}
