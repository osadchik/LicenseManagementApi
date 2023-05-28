using Common.Entities;
using Common.Interfaces;

namespace ProductManagementLambda.Interfaces
{
    /// <summary>
    /// Interface of Products datastore service.
    /// </summary>
    public interface IProductRepository : IReadRepository<ProductDto>, IWriteRepository<ProductDto>
    {
    }
}
