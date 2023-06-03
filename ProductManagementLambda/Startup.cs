using Common.Extensions;
using Common.Interfaces;
using Common.Services;
using Microsoft.OpenApi.Models;
using ProductManagementLambda.Interfaces;
using ProductManagementLambda.Options;
using ProductManagementLambda.Repositories;
using ProductManagementLambda.Services;
using System.Diagnostics.CodeAnalysis;

namespace ProductManagementLambda;

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
        services.AddControllers();
        services.ConfigureSwaggerServices(new OpenApiInfo
        {
            Version = "v1",
            Title = "Product Management Lambda",
            Description = "Products API Lambda implementation for License Management Service."
        });

        services.AddScoped<IProductManagementService, ProductManagementService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISnsClient, SnsClient>();
    }

    /// <summary>
    /// Adds services to the DI container. This method gets called by the runtime.
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/></param>
    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "products-api/{controller}/{id?}");
        });

        app.UseSwagger("products-api", _configuration);
    }
}