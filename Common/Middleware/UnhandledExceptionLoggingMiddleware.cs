using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Middleware
{
    public class UnhandledExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UnhandledExceptionLoggingMiddleware> _logger;

        public UnhandledExceptionLoggingMiddleware(RequestDelegate next, ILogger<UnhandledExceptionLoggingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                const string message = "An unhandled exception occured during request processing.";
                _logger.LogError(ex, message);
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
