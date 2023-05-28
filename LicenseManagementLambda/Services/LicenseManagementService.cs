using Common.Entities;
using Common.Mappers;
using LicenseManagementLambda.Interfaces;
using LicenseManagementLambda.Options;
using Microsoft.Extensions.Options;

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
        public LicenseManagementService(ILicenseRepository licenseRepository, HttpClient httpCLient, IOptions<LambdaParameters> lambdaParameters, ILogger<LicenseManagementService> logger)
        {
            _licenseRepository = licenseRepository;
            _httpClient = httpCLient;
            _lambdaParameters = lambdaParameters.Value;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<LicenseDto> CreateLicenseAsync(Guid productId, LicenseModel licenseModel)
        {
            _logger.LogDebug("Trying to create a new license entity from: {@model}", licenseModel);

            _httpClient.BaseAddress = new Uri(_lambdaParameters.ProductsApiUrl, UriKind.Relative);
            var response = await _httpClient.GetAsync($"/products/{productId}");
            _logger.LogDebug("Received http response fromr products API: {@response}", response);

            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Unable to create license. Product doesn't exist", nameof(productId));
            }

            ProductDto product = null;
            _logger.LogInformation("Successfully retrieved product: {productId}", productId);

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
