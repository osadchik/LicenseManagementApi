namespace Common.Interfaces
{
    /// <summary>
    /// Interface of lambda event source mapping service.
    /// </summary>
    public interface IEventSourceMappingClient
    {
        /// <summary>
        /// Disables event source mapping for lambda.
        /// </summary>
        /// <returns></returns>
        public Task DisableEventSourceMappingAsync();

        /// <summary>
        /// Enables event source mapping for lambda.
        /// </summary>
        /// <returns></returns>
        public Task EnableEventSourceMappingAsync();
    }
}