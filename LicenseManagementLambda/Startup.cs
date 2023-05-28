using Common.Extensions;
using Common.Middleware;
using LicenseManagementLambda.Interfaces;
using LicenseManagementLambda.Options;
using LicenseManagementLambda.Repositories;
using LicenseManagementLambda.Services;
using Microsoft.OpenApi.Models;
using UserManagementLambda.Extensions;

namespace LicenseManagementLambda;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container
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

        services.AddHttpClient();
        services.AddScoped<ILicenseManagementService, LicenseManagementService>();
        services.AddScoped<ILicenseRepository, LicenseRepository>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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