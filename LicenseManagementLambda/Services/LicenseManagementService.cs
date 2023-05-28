using Common.Entities;
using Common.Mappers;
using LicenseManagementLambda.Interfaces;
using System.Net.Http;

namespace LicenseManagementLambda.Services
{
    public class LicenseManagementService : ILicenseManagementService
    {
        private readonly ILicenseRepository _licenseRepository;
        private ILogger<LicenseManagementService> _logger;

        public LicenseManagementService(ILicenseRepository licenseRepository, HttpClient httpCLient, ILogger<LicenseManagementService> logger)
        {
            _licenseRepository = licenseRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<LicenseDto> CreateLicenseAsync(Guid productId, LicenseModel licenseModel)
        {
            _logger.LogDebug("");

            var licenseDto = licenseModel.MapToDto(productId);
            return await _licenseRepository.SaveAsync(licenseDto);
        }

        /// <inheritdoc/>
        public async Task<LicenseDto> DeleteLicenseAsync(Guid licenseId)
        {
            return await _licenseRepository.DeleteAsync(licenseId.ToString());
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
