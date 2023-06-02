using Common.Entities;
using Common.Interfaces;

namespace LicenseManagementLambda.Interfaces
{
    /// <summary>
    /// Interface of product entitlement datastore.
    /// </summary>
    public interface IProductEntitlementRepository : IReadRepository<ProductEntitlementDto>, IWriteRepository<ProductEntitlementDto>
    {
        /// <summary>
        /// Return entitlements corresponding to user.
        /// </summary>
        /// <param name="userId">User's unique identifier.</param>
        /// <returns><see cref="ProductEntitlementDto"/></returns>

        Task<IList<ProductEntitlementDto>> GetByUserIdAsync(Guid userId);

        /// <summary>
        /// Return entitlements corresponding to product.
        /// </summary>
        /// <param name="productId">Product's unique identifier.</param>
        /// <returns><see cref="ProductEntitlementDto"/></returns>
        Task<IList<ProductEntitlementDto>> GetByProductIdAsync(Guid productId);
    }
}
