using System.Diagnostics.CodeAnalysis;

namespace UserIntegrationLambda.Options
{
    /// <summary>
    /// Contains configuration parameters for Event Bridge Rule.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class EventBridgeOptions
    {
        /// <summary>
        /// Gets or sets the Event Bridge Rule name.
        /// </summary>
        public string RuleName { get; set; }
    }
}