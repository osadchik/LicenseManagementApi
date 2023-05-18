using Common.Extensions;
using System.Diagnostics.CodeAnalysis;
using UserManagementLambda.Extensions;
using UserManagementLambda.Interfaces;
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
        services.ConfigureLogging();

        services.ConfigureDynamoDB(_configuration);
        services.ConfigureSns();

        services.AddControllers();

        services.ConfigureSwaggerServices();

        services.AddScoped<ISnsService, SnsService>();
        services.AddScoped<IUsersRepository, UsersRepository>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints => 
        {
            endpoints.MapControllers();
        });

        app.UseSwagger(_configuration);
    }
}