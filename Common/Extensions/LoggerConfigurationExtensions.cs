using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Common.Extensions
{
    /// <summary>
    /// Logger configuration extension class.
    /// </summary>
    public static class LoggerConfigurationExtensions
    {
        /// <summary>
        /// Configure Serilog logger.
        /// </summary>
        /// <param name="serviceCollection"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="ServiceCollection"/></returns>
        public static IServiceCollection ConfigureLogging(this IServiceCollection serviceCollection, LogEventLevel logEventLevel = LogEventLevel.Information)
        {
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            serviceCollection.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(logger);
            });

            return serviceCollection;
        }
    }
}
