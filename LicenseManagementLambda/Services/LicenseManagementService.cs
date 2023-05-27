using Common.Entities;
using Common.Mappers;
using LicenseManagementLambda.Interfaces;

namespace LicenseManagementLambda.Services
{
    public class LicenseManagementService : ILicenseManagementService
    {
        private readonly ILicenseRepository _licenseRepository;
        private ILogger<LicenseManagementService> _logger;

        public LicenseManagementService(ILicenseRepository licenseRepository, ILogger<LicenseManagementService> logger)
        {
            _licenseRepository = licenseRepository;
            _logger = logger;
        }

        public async Task<LicenseDto> CreateLicenseAsync(Guid productId, LicenseModel licenseModel)
        {
            var licenseDto = licenseModel.MapToDto(productId);
            return await _licenseRepository.SaveAsync(licenseDto);
        }

        public async Task<LicenseDto> DeleteLicenseAsync(Guid licenseId)
        {
            return await _licenseRepository.DeleteAsync(licenseId.ToString());
        }

        public async Task<LicenseDto> GetLicenseByIdAsync(Guid licenseId)
        {
            return await _licenseRepository.GetByIdAsync(licenseId);
        }

        public async Task<LicenseDto> UpdateLicenseAsync(LicenseDto licenseDto)
        {
            return await _licenseRepository.SaveAsync(licenseDto);
        }
    }
}
