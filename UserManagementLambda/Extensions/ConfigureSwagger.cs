using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UserManagementLambda.Extensions
{
    public static class ConfigureSwagger
    {
        public static IServiceCollection ConfigureSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "User Management Lambda",
                    Description = "Users API Lambda implementation for License Management Service."
                });

                AddSwaggerXmlComments(options);
            });

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "users-api/swagger/{documentName}/swagger.json";
            })
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Users API implementation for License Management Service.");
                    c.RoutePrefix = "users-api/swagger";
                });

            return app;
        }

        private static void AddSwaggerXmlComments(SwaggerGenOptions options)
        {
            foreach (var xmlDocFile in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml"))
            {
                options.IncludeXmlComments(xmlDocFile);
            }
        }
    }
}
