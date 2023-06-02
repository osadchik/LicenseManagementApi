using Common.Extensions;
using Common.Interfaces;
using LicenseManagementLambda.Interfaces;
using LicenseManagementLambda.Options;
using LicenseManagementLambda.Repositories;
using LicenseManagementLambda.Services;
using System.Diagnostics.CodeAnalysis;

namespace LicenseManagementLambda.Builders
{
    /// <summary>
    /// Provides a creation service of <see cref="IServiceProvider"/> instance.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ServiceProviderBuilder
    {
        private readonly IConfiguration _configuration;

        public ServiceProviderBuilder()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Creates a <see cref="IServiceProvider"/> containing services provided for solution.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns><see cref="ServiceProvider"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IServiceProvider Build(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.ConfigureLambdaVariables<LambdaParameters>(_configuration);
            serviceCollection.ConfigureLogging();
            serviceCollection.ConfigureDynamoDB(_configuration);

            AddServices(serviceCollection);

            var usersUrl = _configuration?.GetSection("Parameters:UsersApiUrl").Value 
                ?? throw new ArgumentException(nameof(_configuration));

            var productsUrl = _configuration?.GetSection("Parameters:ProductsApiUrl").Value
                ?? throw new ArgumentException(nameof(_configuration));

            serviceCollection.AddHttpClient("ProductsAPI", httpClient =>
            {
                httpClient.BaseAddress = new Uri(productsUrl);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            serviceCollection.AddHttpClient("UsersAPI", httpClient =>
            {
                httpClient.BaseAddress = new Uri(usersUrl);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<ILicenseManagementService, LicenseManagementService>();
            services.AddScoped<ILicenseRepository, LicenseRepository>();
            services.AddScoped<IProductEntitlementManagementService, ProductEntitlementManagementService>();
            services.AddScoped<IProductEntitlementRepository, ProductEntitlementRepository>();
            services.AddScoped<ISqsEventProcessingService, SqsEventProcessingService>();
        }
    }
}
