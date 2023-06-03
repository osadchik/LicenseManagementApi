namespace ProductManagementLambda.Options
{
    /// <summary>
    /// Contains lambda parameters.
    /// </summary>
    public class LambdaParameters
    {
        /// <summary>
        /// Name of users Dynamo DB table.
        /// </summary>
        public string SnsTopicArn { get; set; } = null!;
    }
}
