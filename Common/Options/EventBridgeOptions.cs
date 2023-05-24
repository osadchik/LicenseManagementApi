using System.Diagnostics.CodeAnalysis;

namespace Common.Options
{
    /// <summary>
    /// Contains configuration parameters for Event Bridge Rule.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EventBridgeOptions
    {
        /// <summary>
        /// Gets or sets the Event Bridge Rule name.
        /// </summary>
        public string RuleName { get; set; } = null!;
    }
}