using Common.Entities;
using Common.Exceptions;
using Common.Interfaces;
using Microsoft.Extensions.Options;
using UserManagementLambda.Interfaces;
using UserManagementLambda.Options;

namespace UserManagementLambda.Services
{
    /// <summary>
    /// User synchronization adapter between service and SNS.
    /// </summary>
    public class UserManagementService : IUserManagementService
    {
        private readonly IUsersReadRepository _usersRepository;
        private readonly ISnsClient _snsService;
        private readonly LambdaParameters _environmentVariables;
        private readonly ILogger<UserManagementService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="UserManagementService"/> class.
        /// </summary>
        /// <param name="usersRepository"><see cref="IUsersReadRepository"/></param>
        /// <param name="snsService"><see cref="ISnsClient"/></param>
        /// <param name="environmentVariables">Lambda environment variables.</param>
        /// <param name="logger">Logger instance.</param>
        public UserManagementService(IUsersReadRepository usersRepository, ISnsClient snsService, IOptions<LambdaParameters> environmentVariables, ILogger<UserManagementService> logger)
        {
            _usersRepository = usersRepository;
            _snsService = snsService;
            _environmentVariables = environmentVariables.Value;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<UserDto> CreateUser(UserDto user)
        {
            var userMessage = new BaseMessage<UserDto>(user.Uuid.ToString(), ProcessAction.Create) 
            {
                Content = user
            };

            await _snsService.PublishToTopicAsync(_environmentVariables.SnsTopicArn, userMessage);

            return user;
        }

        /// <inheritdoc/>
        public async Task DeleteUser(Guid uuid)
        {
            var userMessage = new BaseMessage<UserDto>(uuid.ToString(), ProcessAction.Delete);

            await _snsService.PublishToTopicAsync(_environmentVariables.SnsTopicArn, userMessage);
        }

        /// <inheritdoc/>
        public async Task<UserDto> GetUserByUserName(string userName)
        {
            var user = await _usersRepository.GetByUsernameAsync(userName);

            return user;
        }

        /// <inheritdoc/>
        public async Task<UserDto> GetUserByUuid(Guid uuid)
        {
            var user = await _usersRepository.GetByIdAsync(uuid);

            if (user is null)
            {
                throw new UserNotFoundException();
            }

            return user;
        }

        /// <inheritdoc/>
        public async Task<UserDto> UpdateUser(UserDto user)
        {
            var existingUser = await _usersRepository.GetByIdAsync(user.Uuid);

            if (existingUser is null) throw new UserNotFoundException();

            var userMessage = new BaseMessage<UserDto>(user.Uuid.ToString(), ProcessAction.Update)
            {
                Content = user
            };

            await _snsService.PublishToTopicAsync(_environmentVariables.SnsTopicArn, userMessage);

            return user;
        }
    }
}
