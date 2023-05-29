using Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Middleware
{
    /// <summary>
    /// Logs unhandled exceptions during request lifetime.
    /// </summary>
    public class UnhandledExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UnhandledExceptionLoggingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="UnhandledExceptionLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next"><see cref="RequestDelegate"/></param>
        /// <param name="logger">Logger instance.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public UnhandledExceptionLoggingMiddleware(RequestDelegate next, ILogger<UnhandledExceptionLoggingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
        }

        /// <summary>
        /// Invokes http context and handles exceptions.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/></param>
        /// <returns>Task.</returns>
        /// <exception cref="ArgumentNullException">Context is null.</exception>
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
            catch (ValidationException ex)
            {
                const string message = "Entity validation failed.";
                _logger.LogWarning(ex, message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            catch (Exception ex) when (ex is UserNotFoundException || 
                                       ex is ProductNotFoundException || 
                                       ex is LicenseNotFoundException)
            {
                const string message = "Entity does not exist";
                _logger.LogWarning(ex, message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
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
