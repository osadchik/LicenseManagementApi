using Common.Extensions;
using Common.Interfaces;
using Common.Middleware;
using Common.Services;
using System.Diagnostics.CodeAnalysis;
using UserManagementLambda.Extensions;
using UserManagementLambda.Interfaces;
using UserManagementLambda.Options;
using UserManagementLambda.Repositories;
using UserManagementLambda.Services;

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
        services.ConfigureLambdaVariables<LambdaParameters>(_configuration);

        services.ConfigureLogging();

        services.ConfigureDynamoDB(_configuration);
        services.ConfigureSns();

        services.AddControllers();

        services.ConfigureSwaggerServices();

        services.AddScoped<ISnsClient, SnsClient>();
        services.AddScoped<IUsersReadRepository, UsersReadRepository>();
        services.AddScoped<IUserManagementService, UserManagementService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseMiddleware<UnhandledExceptionLoggingMiddleware>();

        app.UseEndpoints(endpoints => 
        {
            //endpoints.MapDefaultControllerRoute();
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "users-api/{controller}/{id?}");
        });

        app.UseSwagger(_configuration);
    }
}