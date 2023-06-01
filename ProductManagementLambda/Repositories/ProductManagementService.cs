using Common.Entities;
using Common.Exceptions;
using ProductManagementLambda.Interfaces;

namespace ProductManagementLambda.Repositories
{
    /// <summary>
    /// Manages product operations.
    /// </summary>
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductManagementService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="ProductManagementService"/> class.
        /// </summary>
        /// <param name="productRepository"><see cref="IProductRepository"/></param>
        /// <param name="logger">Logger instance.</param>
        public ProductManagementService(IProductRepository productRepository, ILogger<ProductManagementService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            return await _productRepository.SaveAsync(productDto);
        }

        /// <inheritdoc/>
        public async Task<ProductDto> DeleteProductAsync(Guid productId)
        {
            return await _productRepository.DeleteAsync(productId);
        }

        /// <inheritdoc/>
        public async Task<ProductDto> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product is null)
            {
                throw new ProductNotFoundException();
            }

            return product;
        }

        /// <inheritdoc/>
        public async Task<ProductDto> UpdateProductAsync(ProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(productDto.ProductId);

            if (product is null) throw new ProductNotFoundException();

            return await _productRepository.SaveAsync(productDto);
        }
    }
}
