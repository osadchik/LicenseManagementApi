namespace UserManagementLambda.Options
{
    /// <summary>
    /// Contains lambda parameters.
    /// </summary>
    public class LambdaParameters
    {
        /// <summary>
        /// Amazon Resource Number for Simple Notification Service.
        /// </summary>
        public string SnsTopicArn { get; set; } = null!;
    }
}
