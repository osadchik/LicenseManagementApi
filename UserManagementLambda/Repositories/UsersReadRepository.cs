using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Common.Entities;
using Common.Exceptions;
using UserManagementLambda.Interfaces;

namespace UserManagementLambda.Repositories
{
    /// <summary>
    /// Repository that represents user entity datastore.
    /// </summary>
    public class UsersReadRepository : IUsersReadRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<UsersReadRepository> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="UsersReadRepository"/> class.
        /// </summary>
        /// <param name="dynamoDbContext"><see cref="DynamoDBContext"/></param>
        /// <param name="logger">Logger instance.</param>
        public UsersReadRepository(IDynamoDBContext dynamoDbContext, ILogger<UsersReadRepository> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
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
        public Task<UserDto> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            return GetUserByUsernameInternalAsync(username);
        }

        private async Task<UserDto> GetUserByIdInternalAsync(Guid id)
        {
            _logger.LogDebug("Trying to get user entity with id: {id}", id);

            UserDto user = await _dynamoDbContext.LoadAsync<UserDto>(id);

            _logger.LogInformation("Successfully retrieved a new user entity: {@userDto}", user);

            return user;
        }

        private async Task<UserDto> GetUserByUsernameInternalAsync(string username)
        {
            _logger.LogDebug("Trying to get user entity with username: {username}", username);

            var scanCondition = new ScanCondition("Username", ScanOperator.Equal, username);
            var searchResult = await _dynamoDbContext.ScanAsync<UserDto>(new[] { scanCondition })
                .GetRemainingAsync();

            if (searchResult is null || searchResult.FirstOrDefault() is null)
            {
                throw new UserNotFoundException();
            }

            _logger.LogInformation("Successfully retrieved a new user entity: {@userDto}", searchResult);

            return searchResult.First();
        }
    }
}
