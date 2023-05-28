using Common.Entities;
using Common.Mappers;
using LicenseManagementLambda.Interfaces;
using LicenseManagementLambda.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LicenseManagementLambda.Services
{
    /// <summary>
    /// Manages license CRUD requests.
    /// </summary>
    public class LicenseManagementService : ILicenseManagementService
    {
        private readonly ILicenseRepository _licenseRepository;
        private readonly HttpClient _httpClient;
        private readonly LambdaParameters _lambdaParameters;
        private ILogger<LicenseManagementService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="LicenseManagementService"/> class.
        /// </summary>
        /// <param name="licenseRepository"><see cref="ILicenseRepository"/></param>
        /// <param name="httpCLient"><see cref="HttpClient"/></param>
        /// <param name="lambdaParameters"><see cref="LambdaParameters"/></param>
        /// <param name="logger">Logger instance.</param>
        public LicenseManagementService(ILicenseRepository licenseRepository, IHttpClientFactory httpCLientFactory, IOptions<LambdaParameters> lambdaParameters, ILogger<LicenseManagementService> logger)
        {
            _licenseRepository = licenseRepository;
            _httpClient = httpCLientFactory.CreateClient("ProductsAPI");
            _lambdaParameters = lambdaParameters.Value;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<LicenseDto> CreateLicenseAsync(Guid productId, LicenseModel licenseModel)
        {
            _logger.LogDebug("Trying to create a new license entity from: {@model}", licenseModel);

            _logger.LogDebug("Checking the product. Target URL is {httpClientUrl}", _httpClient.BaseAddress);
            var response = await _httpClient.GetAsync($"products?id={productId}");
            _logger.LogDebug("Received http response from products API: {@response}", response);

            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Unable to create license: product doesn't exist", nameof(productId));
            }

            ProductDto product = JsonConvert.DeserializeObject<ProductDto>(await response.Content.ReadAsStringAsync());
            _logger.LogInformation("Successfully retrieved product: {@product}", product);

            var licenseDto = licenseModel.MapToDto(productId);
            return await _licenseRepository.SaveAsync(licenseDto);
        }

        /// <inheritdoc/>
        public async Task<LicenseDto> DeleteLicenseAsync(Guid licenseId)
        {
            return await _licenseRepository.DeleteAsync(licenseId);
        }

        /// <inheritdoc/>
        public async Task<LicenseDto> GetLicenseByIdAsync(Guid licenseId)
        {
            return await _licenseRepository.GetByIdAsync(licenseId);
        }

        /// <inheritdoc/>
        public async Task<LicenseDto> UpdateLicenseAsync(LicenseDto licenseDto)
        {
            return await _licenseRepository.SaveAsync(licenseDto);
        }
    }
}
