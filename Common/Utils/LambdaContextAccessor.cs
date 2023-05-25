using Amazon.Lambda.Core;
using Common.Interfaces;

namespace Common.Utils
{
    /// <summary>
    /// Lambda context wrapper.
    /// </summary>
    public class LambdaContextAccessor : ILambdaContextAccessor
    {
        private static readonly AsyncLocal<ILambdaContext> AsyncStore = new ();

        /// <inheritdoc/>
        public ILambdaContext Context
        {
            get => AsyncStore.Value;
            set => AsyncStore.Value = value;
        }
    }
}
