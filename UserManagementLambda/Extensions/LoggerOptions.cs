using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace UserManagementLambda.Logging
{
    [ExcludeFromCodeCoverage]
    public static class LoggerOptions
    {
        public static IServiceCollection AddSerilogLogger(this IServiceCollection services, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .CreateLogger();

            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSerilog();
            });

            return services;
        }
    }
}
