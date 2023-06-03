using Common.Constants;
using Common.Entities;
using Common.Exceptions;
using LicenseManagementLambda.Interfaces;
using Newtonsoft.Json;

namespace LicenseManagementLambda.Services
{
    internal class ProductEntitlementManagementService : IProductEntitlementManagementService
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
        public async Task<ProductEntitlementDto> CreateEntitlementAsync(Guid licenseId, Guid productId, Guid userId)
        {
            var license = await _licenseRepository.GetByIdAsync(licenseId);
            if (license is null) throw new LicenseNotFoundException();

            var productsResponse = await _productsHttpClient.GetAsync($"products?id={productId}");
            productsResponse.EnsureSuccessStatusCode();

            _logger.LogInformation("Received http response from Products API: {response}", productsResponse.StatusCode);

            var productDetails = JsonConvert.DeserializeObject<ProductDto>(await productsResponse.Content.ReadAsStringAsync());
            if (productDetails is null)
            {
                throw new ProductNotFoundException("Entitlement creation failed. Unable to deserialize product entity.");
            }
            _logger.LogInformation("Received entity: {@response}", productDetails);

            var usersResponse = await _usersHttpClient.GetAsync($"users/getById?id={userId}");
            usersResponse.EnsureSuccessStatusCode();
            _logger.LogInformation("Received http response from Users API: {response}", usersResponse.StatusCode);

            var userDetails = JsonConvert.DeserializeObject<UserDto>(await usersResponse.Content.ReadAsStringAsync());
            if (userDetails is null)
            {
                throw new UserNotFoundException("Entitlement creation failed. Unable to deserialize user entity.");
            }
            _logger.LogInformation("Received entity: {@response}", userDetails);

            ProductEntitlementDto productEntitlementDto = new()
            {
                UserId = userId.ToString(),
                LicenseId = licenseId.ToString(),
                ProductId = productId.ToString(),
                ProductName = productDetails.Name,
                ProductDescription = productDetails.Description,
                UserName = userDetails.Username
            };
            _logger.LogInformation("Successfully created a new entitlement entity: {@entitlement}", productEntitlementDto);

            return await _productEntitlementRepository.SaveAsync(productEntitlementDto);
        }

        /// <inheritdoc/>
        public async Task<ProductEntitlementDto> DeleteEntitlementAsync(Guid entitlementId)
        {
            return await _productEntitlementRepository.DeleteAsync(entitlementId);
        }

        /// <inheritdoc/>
        public async Task<ProductEntitlementDto> GetEntitlementByIdAsync(Guid entitlementId)
        {
            return await _productEntitlementRepository.GetByIdAsync(entitlementId);
        }

        /// <inheritdoc/>
        public async Task<ProductEntitlementDto> UpdateEntitlementAsync(ProductEntitlementDto entitlementDto)
        {
            return await _productEntitlementRepository.SaveAsync(entitlementDto);
        }

        public async Task UpdateUserDetails(BaseMessage<UserDto> details)
        {
            _logger.LogDebug("Received user details: {@details}", details);
            UserDto content = details.Content;
            IList<ProductEntitlementDto> entitlements = await _productEntitlementRepository.GetByUserIdAsync(details.EntityId);

            switch (details.Action)
            {
                case ProcessAction.Delete:
                    foreach (ProductEntitlementDto entry in entitlements)
                    {
                        entry.UserId = EntityTypes.Deleted;
                        entry.UserName = EntityTypes.Deleted;
                        await _productEntitlementRepository.SaveAsync(entry);
                    }
                    break;

                case ProcessAction.Update:
                    foreach (ProductEntitlementDto entry in entitlements)
                    {
                        entry.UserId = details.EntityId;
                        entry.UserName = content.Username;
                        await _productEntitlementRepository.SaveAsync(entry);
                    }
                    break;

                default:
                    break;
            }
        }

        public async Task UpdateProductDetails(BaseMessage<ProductDto> details)
        {
            _logger.LogDebug("Received product details: {@details}", details);
            ProductDto content = details.Content;
            IList<ProductEntitlementDto> entitlements = await _productEntitlementRepository.GetByProductIdAsync(details.EntityId);

            switch (details.Action)
            {
                case ProcessAction.Delete:
                    foreach (ProductEntitlementDto entry in entitlements)
                    {
                        entry.ProductId = EntityTypes.Deleted;
                        entry.ProductDescription = EntityTypes.Deleted;
                        entry.ProductName = EntityTypes.Deleted;
                        await _productEntitlementRepository.SaveAsync(entry);
                    }
                    break;

                case ProcessAction.Update:
                    foreach (ProductEntitlementDto entry in entitlements)
                    {
                        entry.ProductId = content.ProductId.ToString();
                        entry.ProductDescription = content.Description;
                        entry.ProductName = content.Name;
                        await _productEntitlementRepository.SaveAsync(entry);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
