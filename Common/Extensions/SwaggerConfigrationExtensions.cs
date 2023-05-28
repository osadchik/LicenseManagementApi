using Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Extensions
{
    public static class SwaggerConfigrationExtensions
    {
        public static IServiceCollection ConfigureSwaggerServices(this IServiceCollection services, OpenApiInfo swaggerInfo)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", swaggerInfo);

                AddSwaggerXmlComments(options);
            });

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, string projectPrefix, IConfiguration configuration)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = $"{projectPrefix}/{{documentName}}/swagger.json";
            })
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Users API implementation for License Management Service.");
                    c.RoutePrefix = projectPrefix;
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
