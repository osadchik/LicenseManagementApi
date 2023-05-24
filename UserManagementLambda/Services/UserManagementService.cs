using Common.Entities;
using Microsoft.Extensions.Options;
using UserManagementLambda.Interfaces;
using UserManagementLambda.Options;

namespace UserManagementLambda.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ISnsService _snsService;
        private readonly LambdaEnvironmentVariables _environmentVariables;
        private readonly ILogger<UserManagementService> _logger;

        public UserManagementService(IUsersRepository usersRepository, ISnsService snsService, IOptions<LambdaEnvironmentVariables> environmentVariables, ILogger<UserManagementService> logger)
        {
            _usersRepository = usersRepository;
            _snsService = snsService;
            _environmentVariables = environmentVariables.Value;
            _logger = logger;
        }

        public async Task<UserDto> CreateUser(UserDto user)
        {
            await _usersRepository.SaveAsync(user);

            var userMessage = new BaseMessage<UserDto>(user.Uuid.ToString(), ProcessAction.Create) 
            {
                Content = user
            };

            await _snsService.PublishToTopicAsync(_environmentVariables.SnsTopicArn, userMessage);

            return user;
        }

        public async Task<UserDto> DeleteUser(Guid uuid)
        {
            var user = await _usersRepository.DeleteAsync(uuid);

            var userMessage = new BaseMessage<UserDto>(user.Uuid.ToString(), ProcessAction.Delete)
            {
                Content = user
            };

            await _snsService.PublishToTopicAsync(_environmentVariables.SnsTopicArn, userMessage);

            return user;
        }

        public Task<UserDto> GetUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> GetUserByUuid(Guid uuid)
        {
            var user = await _usersRepository.GetByIdAsync(uuid);

            return user;
        }

        public async Task<UserDto> UpdateUser(UserDto user)
        {
            await _usersRepository.SaveAsync(user);

            var userMessage = new BaseMessage<UserDto>(user.Uuid.ToString(), ProcessAction.Update)
            {
                Content = user
            };

            await _snsService.PublishToTopicAsync(_environmentVariables.SnsTopicArn, userMessage);

            return user;
        }
    }
}
