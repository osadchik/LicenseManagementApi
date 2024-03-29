﻿using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Common.Entities;
using Common.Exceptions;
using LicenseManagementLambda.Interfaces;

namespace LicenseManagementLambda.Repositories
{
    /// <summary>
    /// Product entitlement datastore service.
    /// </summary>
    internal class ProductEntitlementRepository : IProductEntitlementRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<ProductEntitlementRepository> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="ProductEntitlementRepository"/> class.
        /// </summary>
        /// <param name="dynamoDbContext"><see cref="IDynamoDBContext"/></param>
        /// <param name="logger">Logger instance.</param>
        public ProductEntitlementRepository(IDynamoDBContext dynamoDbContext, ILogger<ProductEntitlementRepository> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ProductEntitlementDto> GetByIdAsync(Guid id)
        {
            _logger.LogDebug("Trying to get product entitlement entity with id: {id}", id);

            ProductEntitlementDto entitlement = await _dynamoDbContext.LoadAsync<ProductEntitlementDto>(id);
            if (entitlement is null)
            {
                throw new EntitlementNotFoundException();
            }

            _logger.LogInformation("Successfully retrieved product entitlement entity: {@entity}", entitlement);

            return entitlement;
        }

        /// <inheritdoc/>
        public async Task<ProductEntitlementDto> DeleteAsync(Guid id)
        {
            _logger.LogDebug("Trying to delete product entitlement entity with id: {id}", id);

            var config = new DynamoDBOperationConfig
            {
                IgnoreNullValues = false
            };

            ProductEntitlementDto entitlement = await _dynamoDbContext.LoadAsync<ProductEntitlementDto>(id, config);
            if (entitlement is null)
            {
                throw new EntitlementNotFoundException();
            }

            await _dynamoDbContext.DeleteAsync<ProductEntitlementDto>(id);
            _logger.LogInformation("Successfully deleted product entitlement entity: {@entity}}", entitlement);

            return entitlement;
        }

        /// <inheritdoc/>
        public async Task<ProductEntitlementDto> SaveAsync(ProductEntitlementDto entitlement)
        {
            _logger.LogDebug("Trying to save product entitlement entity: {@entity}", entitlement);

            var config = new DynamoDBOperationConfig
            {
                IgnoreNullValues = false
            };

            await _dynamoDbContext.SaveAsync(entitlement, config);
            _logger.LogInformation("Successfully saved a new product entitlement entity: {@entity}", entitlement);

            return entitlement;
        }

        /// <inheritdoc/>
        public async Task<IList<ProductEntitlementDto>> GetByUserIdAsync(string userId)
        {
            _logger.LogDebug("Trying to get product entitlement entity by user ID: {user}", userId);

            List<ScanCondition> scanConditions = new() { new ScanCondition("UserId", ScanOperator.Equal, userId) };
            var scanResult = await _dynamoDbContext.ScanAsync<ProductEntitlementDto>(scanConditions)
                .GetRemainingAsync();

            _logger.LogInformation("Retrieved product entitlements: {@searchResult}", scanResult);

            if (scanResult is null || !scanResult.Any())
            {
                throw new EntitlementNotFoundException();
            }

            return scanResult;
        }

        /// <inheritdoc/>
        public async Task<IList<ProductEntitlementDto>> GetByProductIdAsync(string productId)
        {
            _logger.LogDebug("Trying to get product entitlement entity by product ID: {product}", productId);

            List<ScanCondition> scanConditions = new() { new ScanCondition("ProductId", ScanOperator.Equal, productId) };
            var scanResult = await _dynamoDbContext.ScanAsync<ProductEntitlementDto>(scanConditions)
                .GetRemainingAsync();

            _logger.LogInformation("Retrieved product entitlements: {@searchResult}", scanResult);

            if (scanResult is null || !scanResult.Any())
            {
                throw new EntitlementNotFoundException();
            }

            return scanResult;
        }

        /// <inheritdoc/>
        public async Task<IList<ProductEntitlementDto>> GetByLicenseIdAsync(string licenseId)
        {
            _logger.LogDebug("Trying to get product entitlement entity by license ID: {license}", licenseId);

            List<ScanCondition> scanConditions = new() { new ScanCondition("LicenseId", ScanOperator.Equal, licenseId) };
            var scanResult = await _dynamoDbContext.ScanAsync<ProductEntitlementDto>(scanConditions)
                .GetRemainingAsync();

            _logger.LogInformation("Retrieved product entitlements: {@searchResult}", scanResult);

            if (scanResult is null || !scanResult.Any())
            {
                throw new EntitlementNotFoundException();
            }

            return scanResult;
        }
    }
}
