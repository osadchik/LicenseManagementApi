using Common.Entities;

namespace LicenseManagementLambda.Interfaces
{
    public interface ILicenseManagementService
    {
        Task<LicenseDto> GetLicenseByIdAsync(Guid licenseId);

        Task<LicenseDto> CreateLicenseAsync(Guid productId, LicenseModel licenseModel);

        Task<LicenseDto> DeleteLicenseAsync(Guid licenseId);

        Task<LicenseDto> UpdateLicenseAsync(LicenseDto licenseDto);
    }
}
