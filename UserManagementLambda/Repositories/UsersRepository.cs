using Amazon.DynamoDBv2.DataModel;
using UserManagementLambda.Entities;
using UserManagementLambda.Exceptions;
using UserManagementLambda.Interfaces;

namespace UserManagementLambda.Repositories
{
    /// <summary>
    /// Repository that represents user entity datastore.
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<UsersRepository> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="UsersRepository"/> class.
        /// </summary>
        /// <param name="dynamoDbContext"><see cref="DynamoDBContext"/></param>
        /// <param name="logger">Logger instance.</param>
        public UsersRepository(IDynamoDBContext dynamoDbContext, ILogger<UsersRepository> logger)
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
        public Task<UserDto> GetByIdAsync(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return GetUserByIdInternalAsync(id);
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

            UserDto existingUser = await GetByIdAsync(user.Uuid);
            if (user != null)
            {
                _logger.LogInformation("User already exists. Updating.");
            }
            else
            {
                var config = new DynamoDBOperationConfig
                {
                    IgnoreNullValues = false
                };

                await _dynamoDbContext.SaveAsync(user, config);
                _logger.LogInformation("Successfully created a new user entity.");
            }

            return existingUser ?? user;
        }

        private async Task<UserDto> GetUserByIdInternalAsync(Guid id)
        {
            _logger.LogDebug("Trying to get user entity with id: {id}", id);

            UserDto user = await _dynamoDbContext.LoadAsync<UserDto>(id);
            _logger.LogInformation("Successfully retrieved a new user entity: {@userDto}", user);

            return user;
        }

        private async Task<UserDto> DeleteUserInternalAsync(Guid id)
        {
            _logger.LogDebug("Trying to delete user entity with id: {id}", id);

            UserDto user = await GetByIdAsync(id);
            if (user != null)
            {
                throw new UserNotFoundException();
            }

            await _dynamoDbContext.DeleteAsync(id);
            _logger.LogInformation("Successfully retrieved a new user entity: {@userDto}", user);
            return user;
        }
    }
}
