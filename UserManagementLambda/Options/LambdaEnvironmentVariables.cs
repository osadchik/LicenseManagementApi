namespace UserManagementLambda.Options
{
    /// <summary>
    /// Encapsulates Lambda environment variables.
    /// </summary>
    public class LambdaEnvironmentVariables
    {
        /// <summary>
        /// Amazon Resource Number for Simple Notification Service.
        /// </summary>
        public string SnsTopicArn { get; set; } = null!;
    }
}
