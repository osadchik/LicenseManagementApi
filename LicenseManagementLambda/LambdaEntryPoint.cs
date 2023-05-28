namespace LicenseManagementLambda;

/// <summary>
/// This class extends from APIGatewayProxyFunction which contains the method FunctionHandlerAsync which is the actual Lambda function entry point. 
/// </summary>
public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
{
    /// <summary>
    /// Initializes web host builder configuration.
    /// </summary>
    /// <param name="builder"><see cref="IWebHostBuilder"/></param>
    protected override void Init(IWebHostBuilder builder)
    {
        builder.UseStartup<Startup>();
    }
}