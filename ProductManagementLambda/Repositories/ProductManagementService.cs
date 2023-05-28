using Common.Entities;
using ProductManagementLambda.Interfaces;

namespace ProductManagementLambda.Repositories
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductManagementService> _logger;

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
            return await _productRepository.GetByIdAsync(productId);
        }

        /// <inheritdoc/>
        public async Task<ProductDto> UpdateProductAsync(ProductDto productDto)
        {
            return await _productRepository.SaveAsync(productDto);
        }
    }
}
