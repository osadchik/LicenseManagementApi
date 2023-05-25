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
        public async Task DeleteUser(UserDto user)
        {
            // TODO: Delete by uuid, won't work for now
            await _usersWriteRepository.DeleteAsync(user.Uuid);
        }


        /// <inheritdoc/>
        public async Task UpdateUser(UserDto user)
        {
            _userValidator.ValidateAndThrow(user);
            await _usersWriteRepository.SaveAsync(user);
        }
    }
}