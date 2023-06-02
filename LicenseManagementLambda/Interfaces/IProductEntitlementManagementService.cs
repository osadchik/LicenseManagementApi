using Common.Entities;

namespace LicenseManagementLambda.Interfaces
{
    internal interface IProductEntitlementManagementService
    {
        /// <summary>
        /// Gets entitlement from the datastore.
        /// </summary>
        /// <param name="entitlementId">Entitlement's unqiue indentifier.</param>
        /// <returns>Product entitlement entity.</returns>
        Task<ProductEntitlementDto> GetEntitlementByIdAsync(Guid entitlementId);

        /// <summary>
        /// Manages entitlement creation operation.
        /// </summary>
        /// <param name="licenseId">License's unqiue indentifier.</param>
        /// <param name="userId">Users's unqiue indentifier.</param>
        /// <returns>Created entity.</returns>
        Task<ProductEntitlementDto> CreateEntitlementAsync(Guid licenseId, Guid userId);

        /// <summary>
        /// Manages entitlement deletion operation.
        /// </summary>
        /// <param name="entitlementId">Entitlement's unqiue indentifier.</param>
        /// <returns>Deleted entity.</returns>
        Task<ProductEntitlementDto> DeleteEntitlementAsync(Guid entitlementId);

        /// <summary>
        /// Manages entitlement update operation.
        /// </summary>
        /// <param name="entitlementDto"><see cref="ProductEntitlementDto"/></param>
        /// <returns>Updated entity.</returns>
        Task<ProductEntitlementDto> UpdateEntitlementAsync(ProductEntitlementDto entitlementDto);

        /// <summary>
        /// Updates entitlement user information.
        /// </summary>
        /// <param name="details"><see cref="BaseMessage{UserDto}"/></param>
        /// <returns>Task.</returns>
        Task UpdateUserDetails(BaseMessage<UserDto> details);

        /// <summary>
        /// Updates entitlement product information.
        /// </summary>
        /// <param name="details"><see cref="BaseMessage{ProductDto}"/></param>
        /// <returns>Task.</returns>
        Task UpdateProductDetails(BaseMessage<ProductDto> details);
    }
}