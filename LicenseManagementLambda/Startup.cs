using Common.Extensions;
using Common.Middleware;
using LicenseManagementLambda.Interfaces;
using LicenseManagementLambda.Options;
using LicenseManagementLambda.Repositories;
using LicenseManagementLambda.Services;
using Microsoft.OpenApi.Models;

namespace LicenseManagementLambda;

/// <summary>
/// Startup class.
/// </summary>
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
        services.AddControllers();
        services.ConfigureSwaggerServices(new OpenApiInfo
        {
            Version = "v1",
            Title = "License Management Lambda",
            Description = "License API Lambda implementation for License Management Service."
        });

        var usersUrl = _configuration?.GetSection("Parameters:UsersApiUrl").Value
            ?? throw new ArgumentException(nameof(_configuration));

        var productsUrl = _configuration?.GetSection("Parameters:ProductsApiUrl").Value
            ?? throw new ArgumentException(nameof(_configuration));

        services.AddHttpClient("ProductsAPI", httpClient =>
        {
            httpClient.BaseAddress = new Uri(productsUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        services.AddHttpClient("UsersAPI", httpClient =>
        {
            httpClient.BaseAddress = new Uri(usersUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddScoped<ILicenseManagementService, LicenseManagementService>();
        services.AddScoped<ILicenseRepository, LicenseRepository>();
        services.AddScoped<IProductEntitlementManagementService, ProductEntitlementManagementService>();
        services.AddScoped<IProductEntitlementRepository, ProductEntitlementRepository>();
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
                pattern: "license-api/{controller}/{id?}");
        });

        app.UseSwagger("license-api", _configuration);
    }
}