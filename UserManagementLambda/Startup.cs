using Common.Extensions;
using Common.Interfaces;
using Common.Middleware;
using Common.Services;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using UserManagementLambda.Interfaces;
using UserManagementLambda.Options;
using UserManagementLambda.Repositories;
using UserManagementLambda.Services;

namespace UserManagementLambda;

/// <summary>
/// Startup class.
/// </summary>
[ExcludeFromCodeCoverage]
public class Startup
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Intializes a new instance of <see cref="Startup"/> class.
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <exception cref="ArgumentNullException"></exception>
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Adds services to the DI container. This method gets called by the runtime.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureLambdaVariables<LambdaParameters>(_configuration);

        services.ConfigureLogging();

        services.ConfigureDynamoDB(_configuration);
        services.ConfigureSns();

        services.AddControllers();

        services.ConfigureSwaggerServices(new OpenApiInfo
        {
            Version = "v1",
            Title = "User Management Lambda",
            Description = "Users API Lambda implementation for License Management Service."
        });

        services.AddScoped<ISnsClient, SnsClient>();
        services.AddScoped<IUsersReadRepository, UsersReadRepository>();
        services.AddScoped<IUserManagementService, UserManagementService>();
    }

    /// <summary>
    /// Adds services to the DI container. This method gets called by the runtime.
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/></param>
    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseMiddleware<UnhandledExceptionLoggingMiddleware>();

        app.UseEndpoints(endpoints => 
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "users-api/{controller}/{id?}");
        });

        app.UseSwagger("users-api", _configuration);
    }
}