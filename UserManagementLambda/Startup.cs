using Common.Extensions;
using Common.Middleware;
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
        AddLambdaEnvironmentVariables(services);

        services.ConfigureLogging();

        services.ConfigureDynamoDB(_configuration);
        services.ConfigureSns();

        services.AddControllers();

        services.ConfigureSwaggerServices();

        services.AddScoped<ISnsService, SnsService>();
        services.AddScoped<IUsersRepository, UsersRepository>();
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
            endpoints.MapControllers();
        });

        app.UseSwagger(_configuration);
    }

    private static void AddLambdaEnvironmentVariables(IServiceCollection services)
    {
        services.Configure<LambdaEnvironmentVariables>(act =>
        {
            var snsTopicArn = Environment.GetEnvironmentVariable("SNS_Topic_ARN");
            act.SnsTopicArn = snsTopicArn
                              ?? throw new ArgumentNullException("SNS Topic ARN is null. Please, check the lambda environment variables configuration.");
        });
    }
}