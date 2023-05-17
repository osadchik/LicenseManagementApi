namespace Resiliency.CircuitBreaker
{
    public class ExecutionResult<TResult>
    {
        public CircuitState State { get; }
        public TResult Result { get; }

        public ExecutionResult(CircuitState state, TResult result)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
            Result = result;
        }
    }
}