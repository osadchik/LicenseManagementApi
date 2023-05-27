﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UserManagementLambda.Extensions
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

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "users-api/{documentName}/swagger.json";
            })
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Users API implementation for License Management Service.");
                    c.RoutePrefix = "users-api";
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