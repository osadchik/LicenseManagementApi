﻿using Common.Entities;
using Common.Exceptions;
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
        private readonly IProductEntitlementManagementService _productEntitlementManagementService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<LicenseManagementService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="LicenseManagementService"/> class.
        /// </summary>
        /// <param name="licenseRepository"><see cref="ILicenseRepository"/></param>
        /// <param name="httpCLientFactory"><see cref="IHttpClientFactory"/></param>
        /// <param name="lambdaParameters"><see cref="LambdaParameters"/></param>
        /// <param name="logger">Logger instance.</param>
        public LicenseManagementService(
            ILicenseRepository licenseRepository,
            IHttpClientFactory httpCLientFactory,
            ILogger<LicenseManagementService> logger,
            IProductEntitlementManagementService productEntitlementManagementService)
        {
            _licenseRepository = licenseRepository;
            _httpClient = httpCLientFactory.CreateClient("ProductsAPI");
            _logger = logger;
            _productEntitlementManagementService = productEntitlementManagementService;
        }

        /// <inheritdoc/>
        public async Task<LicenseDto> CreateLicenseAsync(Guid productId, LicenseCreateModel licenseModel)
        {
            _logger.LogDebug("Trying to create a new license entity from: {@model}", licenseModel);

            _logger.LogDebug("Checking the product. Target URL is {httpClientUrl}", _httpClient.BaseAddress);
            var response = await _httpClient.GetAsync($"products?id={productId}");
            _logger.LogInformation("Received http response from products API: {@response}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                throw new ProductNotFoundException("Unable to create license: product doesn't exist");
            }

            var licenseDto = licenseModel.MapToDto(productId);
            return await _licenseRepository.SaveAsync(licenseDto);
        }

        /// <inheritdoc/>
        public async Task<LicenseDto> DeleteLicenseAsync(Guid licenseId)
        {
            var license = await _licenseRepository.DeleteAsync(licenseId);

            _productEntitlementManagementService.DeleteLicenseDetails(license);

            return license;
        }

        /// <inheritdoc/>
        public async Task<LicenseDto> GetLicenseByIdAsync(Guid licenseId)
        {
            var license = await _licenseRepository.GetByIdAsync(licenseId);

            if (license is null)
            {
                throw new LicenseNotFoundException();
            }

            return license;
        }

        /// <inheritdoc/>
        public async Task<LicenseDto> UpdateLicenseAsync(LicenseDto licenseDto)
        {
            var currentLicense = await _licenseRepository.GetByIdAsync(licenseDto.LicenseId);

            if (currentLicense is null) throw new LicenseNotFoundException();

            if (licenseDto.ProductId != currentLicense.ProductId)
            {
                _logger.LogDebug("Product update requested. Checking the product with ID: {id}", currentLicense.ProductId);

                var response = await _httpClient.GetAsync($"products?id={currentLicense.ProductId}");
                _logger.LogDebug("Received http response from products API: {@response}", response);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ProductNotFoundException("Unable to update license: product doesn't exist");
                }
            }

            return await _licenseRepository.SaveAsync(licenseDto);
        }
    }
}
