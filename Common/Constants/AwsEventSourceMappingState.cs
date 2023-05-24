namespace UserIntegrationLambda.Services.CircuitBreaker
{
    public static class AwsEventSourceMappingState
    {
        public const string Enabled = "Enabled";
        public const string Disabled = "Disabled";

        public const string Enabling = "Enabling";
        public const string Disabling = "Disabling";
    }
}