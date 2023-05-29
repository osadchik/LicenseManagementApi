using Common.Entities;
using Common.Interfaces;

namespace LicenseManagementLambda.Interfaces
{
    /// <summary>
    /// Interface of product entitlement datastore.
    /// </summary>
    public interface IProductEntitlementRepository : IReadRepository<ProductEntitlementDto>, IWriteRepository<ProductEntitlementDto>
    {

    }
}
