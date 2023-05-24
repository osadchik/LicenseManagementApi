using Common.Entities;
using Common.Interfaces;
using Microsoft.Extensions.Options;
using UserManagementLambda.Interfaces;
using UserManagementLambda.Options;

namespace UserManagementLambda.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUsersReadRepository _usersRepository;
        private readonly ISnsClient _snsService;
        private readonly LambdaEnvironmentVariables _environmentVariables;
        private readonly ILogger<UserManagementService> _logger;

        public UserManagementService(IUsersReadRepository usersRepository, ISnsClient snsService, IOptions<LambdaEnvironmentVariables> environmentVariables, ILogger<UserManagementService> logger)
        {
            _usersRepository = usersRepository;
            _snsService = snsService;
            _environmentVariables = environmentVariables.Value;
            _logger = logger;
        }

        public async Task<UserDto> CreateUser(UserDto user)
        {
            var userMessage = new BaseMessage<UserDto>(user.Uuid.ToString(), ProcessAction.Create) 
            {
                Content = user
            };

            await _snsService.PublishToTopicAsync(_environmentVariables.SnsTopicArn, userMessage);

            return user;
        }

        public async Task DeleteUser(Guid uuid)
        {
            var userMessage = new BaseMessage<UserDto>(uuid.ToString(), ProcessAction.Delete);

            await _snsService.PublishToTopicAsync(_environmentVariables.SnsTopicArn, userMessage);
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
            var userMessage = new BaseMessage<UserDto>(user.Uuid.ToString(), ProcessAction.Update)
            {
                Content = user
            };

            await _snsService.PublishToTopicAsync(_environmentVariables.SnsTopicArn, userMessage);

            return user;
        }
    }
}
