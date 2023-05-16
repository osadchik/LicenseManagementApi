using System.Diagnostics.CodeAnalysis;
using UserManagementLambda.Extensions;
using UserManagementLambda.Logging;

namespace UserManagementLambda;

[ExcludeFromCodeCoverage]
public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSerilogLogger(_configuration);

        services.ConfigureDynamoDB(_configuration);

        services.AddControllers();

        services.ConfigureSwaggerServices();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(o =>
        {
            o.RoutePrefix = "";
            o.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API v1");
        });

        app.UseHttpsRedirection();

        app.UseRouting();

        //app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
        });
    }
}