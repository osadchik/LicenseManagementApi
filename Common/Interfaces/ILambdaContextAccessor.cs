using Amazon.Lambda.Core;

namespace Common.Interfaces
{
    /// <summary>
    /// Interface for lambda function context wrapper.
    /// </summary>
    public interface ILambdaContextAccessor
    {
        /// <summary>
        /// <see cref="ILambdaContext"/>
        /// </summary>
        ILambdaContext Context { get; } 
    }
}
