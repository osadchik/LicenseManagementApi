using Amazon.DynamoDBv2.DataModel;
using Common.Entities;
using Common.Exceptions;
using LicenseManagementLambda.Interfaces;

namespace LicenseManagementLambda.Repositories
{
    public class LicenseRepository : ILicenseRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<LicenseRepository> _logger;

        public LicenseRepository(IDynamoDBContext dynamoDbContext, ILogger<LicenseRepository> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        public async Task<LicenseDto> GetByIdAsync(Guid id)
        {
            _logger.LogDebug("Trying to get license entity with id: {id}", id);

            LicenseDto license = await _dynamoDbContext.LoadAsync<LicenseDto>(id);
            _logger.LogInformation("Successfully retrieved license entity: {@entity}", license);

            return license;
        }

        public async Task<LicenseDto> DeleteAsync(string id)
        {
            _logger.LogDebug("Trying to delete license entity with id: {id}", id);

            var config = new DynamoDBOperationConfig
            {
                IgnoreNullValues = false
            };

            LicenseDto license = await _dynamoDbContext.LoadAsync<LicenseDto>(id, config);
            if (license == null)
            {
                throw new LicenseNotFoundException();
            }

            await _dynamoDbContext.DeleteAsync<LicenseDto>(id);
            _logger.LogInformation("Successfully deleted license entity: {@entity}}", license);

            return license;
        }

        public async Task<LicenseDto> SaveAsync(LicenseDto license)
        {
            _logger.LogDebug("Trying to save license entity: {@entity}", license);

            var config = new DynamoDBOperationConfig
            {
                IgnoreNullValues = false
            };

            await _dynamoDbContext.SaveAsync(license, config);
            _logger.LogInformation("Successfully save a user entity: {@entity}", license);

            return license;
        }
    }
}
