using Common.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using UserIntegrationLambda.Interfaces;
using UserIntegrationLambda.Validation;

namespace UserIntegrationLambda.Services
{
    /// <summary>
    /// User synchronization service between lambda and downstream consumer
    /// </summary>
    public class UserIntegrationHandler : IUserIntegrationHandler
    {
        private readonly UserValidator _userValidator = new();

        private readonly IUsersWriteRepository _usersWriteRepository;
        private readonly ILogger<UserIntegrationHandler> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="UserIntegrationHandler"/> class.
        /// </summary>
        /// <param name="usersWriteRepository">Performs write operations with the datastore.</param>
        /// <param name="logger">Logger instance.</param>
        public UserIntegrationHandler(IUsersWriteRepository usersWriteRepository, ILogger<UserIntegrationHandler> logger)
        {
            _usersWriteRepository = usersWriteRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task CreateUser(UserDto user)
        {
            _userValidator.ValidateAndThrow(user);
            await _usersWriteRepository.SaveAsync(user);
        }

        /// <inheritdoc/>
        public async Task DeleteUser(Guid uuid)
        {
            await _usersWriteRepository.DeleteAsync(uuid);
        }

        /// <inheritdoc/>
        public async Task UpdateUser(UserDto user)
        {
            _userValidator.ValidateAndThrow(user);
            await _usersWriteRepository.SaveAsync(user);
        }
    }
}