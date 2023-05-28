using Amazon.DynamoDBv2.DataModel;
using Common.Entities;
using Common.Exceptions;
using ProductManagementLambda.Interfaces;

namespace ProductManagementLambda.Repositories
{
    /// <summary>
    /// Products datastore service.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(IDynamoDBContext dynamoDbContext, ILogger<ProductRepository> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            _logger.LogDebug("Trying to get product entity with id: {id}", id);

            ProductDto product = await _dynamoDbContext.LoadAsync<ProductDto>(id);
            _logger.LogInformation("Successfully retrieved product entity: {@entity}", product);

            return product;
        }

        public async Task<ProductDto> DeleteAsync(string id)
        {
            _logger.LogDebug("Trying to delete product entity with id: {id}", id);

            var config = new DynamoDBOperationConfig
            {
                IgnoreNullValues = false
            };

            ProductDto product = await _dynamoDbContext.LoadAsync<ProductDto>(id, config);
            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            await _dynamoDbContext.DeleteAsync<ProductDto>(id);
            _logger.LogInformation("Successfully deleted product entity: {@entity}}", product);

            return product;
        }

        public async Task<ProductDto> SaveAsync(ProductDto product)
        {
            _logger.LogDebug("Trying to save product entity: {@entity}", product);

            var config = new DynamoDBOperationConfig
            {
                IgnoreNullValues = false
            };

            await _dynamoDbContext.SaveAsync(product, config);
            _logger.LogInformation("Successfully save a product entity: {@entity}", product);

            return product;
        }
    }
}
