using NSubstitute;
using Resiliency.CircuitBreaker;
using UnitTests.Utils;

namespace UnitTests
{
    public class CircuitBreakerPolicyTests
    {
        [Theory, AutoMockData]
        public async Task CircuitBreaker_Should_AllowExecutionOfVoidFunction_When_Closed(ClosedState state, Func<Task> action)
        {
            // Act
            await new CircuitBreakerPolicy(state).ExecuteAsync(action);

            // Assert
            await action.Received()();
        }
    }
}