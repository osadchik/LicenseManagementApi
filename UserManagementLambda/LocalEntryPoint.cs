namespace UserManagementLambda;

/// <summary>
/// The Main function can be used to run the ASP.NET Core application locally using the Kestrel webserver.
/// </summary>
public class LocalEntryPoint
{
    /// <summary>
    /// Local entry point main function.
    /// </summary>
    /// <param name="args">Main arguments.</param>
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Initializes a host which encapsulates an app's resources.
    /// </summary>
    /// <param name="args">Builder arguments.</param>
    /// <returns></returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureAppConfiguration(c =>
                {
                    c.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    c.AddJsonFile("appsettings.json");
                    c.AddEnvironmentVariables();
                });
            });
}