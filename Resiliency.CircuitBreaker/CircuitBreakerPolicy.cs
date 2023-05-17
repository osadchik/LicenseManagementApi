namespace Resiliency.CircuitBreaker
{
    public class CircuitBreakerPolicy
    {
        private Func<Exception, bool> _exceptionFilter = _ => true;
        public CircuitState State { get; private set; }

        public CircuitBreakerPolicy(CircuitState currentState)
        {
            State = currentState ?? throw new ArgumentNullException(nameof(currentState));
        }

        public CircuitBreakerPolicy Handle(Func<Exception, bool> exceptionFilter)
        {
            _exceptionFilter = exceptionFilter ?? throw new ArgumentNullException(nameof(exceptionFilter));
            return this;
        }

        public Task ExecuteAsync(Func<Task> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return InnerExecuteAsync(action);
        }

        public Task ExecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return InnerExecuteAsync(action);
        }

        public void ClosePermanently()
        {
            State = new PermanentlyClosedState();
        }

        private async Task InnerExecuteAsync(Func<Task> action)
        {
            try
            {
                State = await State.ExecuteAsync(action);
            }
            catch (Exception exception) when (_exceptionFilter(exception) && exception is not OpenCircuitException)
            {
                State = State.RecordError();
                throw;
            }
        }

        private async Task<TResult> InnerExecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            try
            {
                ExecutionResult<TResult> executionResult = await State.ExecuteAsync(action);
                State = executionResult.State;
                return executionResult.Result;
            }
            catch (Exception exception) when (_exceptionFilter(exception) && exception is not OpenCircuitException)
            {
                State = State.RecordError();
                throw;
            }
        }
    }
}