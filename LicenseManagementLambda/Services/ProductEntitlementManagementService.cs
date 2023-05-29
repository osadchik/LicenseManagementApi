using Common.Entities;
using Common.Exceptions;
using LicenseManagementLambda.Interfaces;
using Newtonsoft.Json;

namespace LicenseManagementLambda.Services
{
    public class ProductEntitlementManagementService : IProductEntitlementManagementService
    {
        private readonly IProductEntitlementRepository _productEntitlementRepository;
        private readonly ILicenseRepository _licenseRepository;
        private readonly HttpClient _productsHttpClient;
        private readonly HttpClient _usersHttpClient;
        private readonly ILogger<ProductEntitlementManagementService> _logger;

        /// <summary>
        /// Intiializes a new instance of <see cref="ProductEntitlementManagementService"/> class.
        /// </summary>
        /// <param name="productEntitlementRepository"><see cref="IProductEntitlementRepository"/></param>
        /// <param name="licenseRepository"><see cref="ILicenseRepository"/></param>
        /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/></param>
        /// <param name="logger">Logger instance.</param>
        public ProductEntitlementManagementService(
            IProductEntitlementRepository productEntitlementRepository,
            ILicenseRepository licenseRepository,
            IHttpClientFactory httpClientFactory,
            ILogger<ProductEntitlementManagementService> logger)
        {
            _productEntitlementRepository = productEntitlementRepository;
            _licenseRepository = licenseRepository;
            _productsHttpClient = httpClientFactory.CreateClient("ProductsAPI");
            _usersHttpClient = httpClientFactory.CreateClient("UsersAPI");
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ProductEntitlementDto> CreateEntitlementAsync(Guid licenseId, Guid userId)
        {
            var license = await _licenseRepository.GetByIdAsync(licenseId);
            if (license is null) throw new LicenseNotFoundException();

            var productsResponse = await _productsHttpClient.GetAsync($"products?id={license.ProductId}");
            if (!productsResponse.IsSuccessStatusCode)
            {
                throw new ArgumentException("Entitlement creation failed. Unable to get product details from Products API.", nameof(licenseId));
            }
            _logger.LogInformation("Received http response from Products API: {response}", productsResponse.StatusCode);

            var productDetails = JsonConvert.DeserializeObject<ProductDto>(await productsResponse.Content.ReadAsStringAsync());
            if (productDetails is null)
            {
                throw new ArgumentException("Entitlement creation failed. Unable to deserialize product entity.", nameof(licenseId));
            }
            _logger.LogInformation("Received entity: {@response}", productDetails);

            var usersResponse = await _usersHttpClient.GetAsync($"users?id={userId}");
            if (!usersResponse.IsSuccessStatusCode)
            {
                throw new ArgumentException("Entitlement creation failed. Unable to get user details from Users API.", nameof(userId));
            }
            _logger.LogInformation("Received http response from Users API: {response}", usersResponse.StatusCode);

            var userDetails = JsonConvert.DeserializeObject<UserDto>(await usersResponse.Content.ReadAsStringAsync());
            if (userDetails is null)
            {
                throw new ArgumentException("Entitlement creation failed. Unable to deserialize user entity.", nameof(licenseId));
            }
            _logger.LogInformation("Received entity: {@response}", userDetails);

            ProductEntitlementDto productEntitlementDto = new()
            {
                UserId = userId,
                LicenseId = licenseId,
                ProductName = productDetails.Name,
                ProductDescription = productDetails.Description,
                UserName = userDetails.Username
            };
            _logger.LogInformation("Successfully created a new entitlement entity: {@entitlement}", productEntitlementDto);

            return await _productEntitlementRepository.SaveAsync(productEntitlementDto);
        }

        /// <inheritdoc/>
        public Task<ProductEntitlementDto> DeleteEntitlementAsync(Guid entitlementId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ProductEntitlementDto> GetLicenseByIdAsync(Guid entitlementId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductEntitlementDto> UpdateEntitlementAsync(LicenseDto entitlementDto)
        {
            throw new NotImplementedException();
        }
    }
}
