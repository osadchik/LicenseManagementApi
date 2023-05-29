using Common.Entities;

namespace LicenseManagementLambda.Interfaces
{
    /// <summary>
    /// Interface of license CRUD management service.
    /// </summary>
    public interface ILicenseManagementService
    {
        /// <summary>
        /// Gets license from the datastore.
        /// </summary>
        /// <param name="licenseId">License's unqiue indentifier.</param>
        /// <returns>License entity.</returns>
        Task<LicenseDto> GetLicenseByIdAsync(Guid licenseId);

        /// <summary>
        /// Manages license creation operation.
        /// </summary>
        /// <param name="productId">Product's unqiue indentifier.</param>
        /// <param name="licenseModel"><see cref="LicenseCreateModel"/></param>
        /// <returns>Created entity.</returns>
        Task<LicenseDto> CreateLicenseAsync(Guid productId, LicenseCreateModel licenseModel);

        /// <summary>
        /// Manages license deletion operation.
        /// </summary>
        /// <param name="licenseId"><see cref="LicenseDto"/></param>
        /// <returns>Deleted entity.</returns>
        Task<LicenseDto> DeleteLicenseAsync(Guid licenseId);

        /// <summary>
        /// Manages license update operation.
        /// </summary>
        /// <param name="licenseDto"><see cref="LicenseDto"/></param>
        /// <returns>Updated entity.</returns>
        Task<LicenseDto> UpdateLicenseAsync(LicenseDto licenseDto);
    }
}
