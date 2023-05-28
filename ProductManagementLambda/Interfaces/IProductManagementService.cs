using Common.Entities;

namespace ProductManagementLambda.Interfaces
{
    /// <summary>
    /// Interface of product CRUD management service.
    /// </summary>
    public interface IProductManagementService
    {
        /// <summary>
        /// Gets product from the datastore.
        /// </summary>
        /// <param name="productId">Product's unqiue indentifier.</param>
        /// <returns>Entity.</returns>
        Task<ProductDto> GetProductByIdAsync(Guid productId);

        /// <summary>
        /// Manages product creation operation.
        /// </summary>
        /// <param name="productDto"><see cref="ProductDto"/></param>
        /// <returns>Created entity.</returns>
        Task<ProductDto> CreateProductAsync(ProductDto productDto);

        /// <summary>
        /// Manages product deletion operation.
        /// </summary>
        /// <param name="productId"><see cref="ProductDto"/></param>
        /// <returns>Deleted entity.</returns>
        Task<ProductDto> DeleteProductAsync(Guid productId);

        /// <summary>
        /// Manages product update operation.
        /// </summary>
        /// <param name="productDto"><see cref="ProductDto"/></param>
        /// <returns>Updated entity.</returns>
        Task<ProductDto> UpdateProductAsync(ProductDto productDto);
    }
}
