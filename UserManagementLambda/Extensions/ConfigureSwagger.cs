﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UserManagementLambda.Extensions
{
    public static class ConfigureSwagger
    {
        public static IServiceCollection ConfigureSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Users API implementation for License Management Service.",
                    Version = "v1",
                });

                AddSwaggerXmlComments(c);
            });

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API implementation for License Management Service.");
                    c.RoutePrefix = string.Empty;
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
