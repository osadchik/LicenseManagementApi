using Common.Extensions;
using Microsoft.OpenApi.Models;

namespace ProductManagementLambda;

public class Startup
{
    private IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureLogging();
        services.ConfigureDynamoDB(_configuration);
        services.AddControllers();
        services.ConfigureSwaggerServices(new OpenApiInfo
        {
            Version = "v1",
            Title = "Product Management Lambda",
            Description = "Products API Lambda implementation for License Management Service."
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "product-api/{controller}/{id?}");
        });

        app.UseSwagger("product-api", _configuration);
    }
}