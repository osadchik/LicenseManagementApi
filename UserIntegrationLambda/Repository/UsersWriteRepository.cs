using Amazon.DynamoDBv2.DataModel;
using Common.Entities;
using Common.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserIntegrationLambda.Repository
{
    public class UsersWriteRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<UsersWriteRepository> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="UsersWriteRepository"/> class.
        /// </summary>
        /// <param name="dynamoDbContext"><see cref="DynamoDBContext"/></param>
        /// <param name="logger">Logger instance.</param>
        public UsersWriteRepository(IDynamoDBContext dynamoDbContext, ILogger<UsersWriteRepository> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Task<UserDto> SaveAsync(UserDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return SaveInternalAsync(user);
        }

        /// <inheritdoc/>
        public Task<UserDto> DeleteAsync(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return DeleteUserInternalAsync(id);
        }

        private async Task<UserDto> SaveInternalAsync(UserDto user)
        {
            _logger.LogDebug("Trying to save user entity: {@user}", user);

            var config = new DynamoDBOperationConfig
            {
                IgnoreNullValues = false
            };

            await _dynamoDbContext.SaveAsync(user, config);
            _logger.LogInformation("Successfully created a new user entity: {@user}", user);

            return user;
        }


        private async Task<UserDto> DeleteUserInternalAsync(Guid id)
        {
            _logger.LogDebug("Trying to delete user entity with id: {id}", id);

            UserDto user = await _dynamoDbContext.LoadAsync<UserDto>(id);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            await _dynamoDbContext.DeleteAsync(id);
            _logger.LogInformation("Successfully deleted user entity: {@userDto}", user);
            return user;
        }
    }
}
