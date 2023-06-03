using Common.Constants;
using Common.Entities;
using Common.Exceptions;
using Common.Interfaces;
using Microsoft.Extensions.Options;
using ProductManagementLambda.Interfaces;
using ProductManagementLambda.Options;

namespace ProductManagementLambda.Services
{
    /// <summary>
    /// Manages product operations.
    /// </summary>
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductRepository _productRepository;
        private readonly ISnsClient _snsClient;
        private readonly LambdaParameters _environmentVariables;
        private readonly ILogger<ProductManagementService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="ProductManagementService"/> class.
        /// </summary>
        /// <param name="productRepository"><see cref="IProductRepository"/></param>
        /// <param name="logger">Logger instance.</param>
        public ProductManagementService(IProductRepository productRepository, ISnsClient snsClient, IOptions<LambdaParameters> environmentVariables, ILogger<ProductManagementService> logger)
        {
            _productRepository = productRepository;
            _snsClient = snsClient;
            _environmentVariables = environmentVariables.Value;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            var productMessage = new BaseMessage<ProductDto>(productDto.ProductId.ToString(), EntityTypes.Product, ProcessAction.Create)
            {
                Content = productDto
            };
            await _snsClient.PublishToTopicAsync(_environmentVariables.SnsTopicArn, productMessage);

            return await _productRepository.SaveAsync(productDto);
        }

        /// <inheritdoc/>
        public async Task<ProductDto> DeleteProductAsync(Guid productId)
        {
            var productMessage = new BaseMessage<ProductDto>(productId.ToString(), EntityTypes.Product, ProcessAction.Delete);
            await _snsClient.PublishToTopicAsync(_environmentVariables.SnsTopicArn, productMessage);

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

            var productMessage = new BaseMessage<ProductDto>(productDto.ProductId.ToString(), EntityTypes.Product, ProcessAction.Update)
            {
                Content = productDto
            };
            await _snsClient.PublishToTopicAsync(_environmentVariables.SnsTopicArn, productMessage);

            return await _productRepository.SaveAsync(productDto);
        }
    }
}
