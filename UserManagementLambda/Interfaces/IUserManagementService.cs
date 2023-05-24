using Common.Entities;

namespace UserManagementLambda.Interfaces
{
    public interface IUserManagementService
    {
        Task<UserDto> GetUserByUuid(Guid uuid);

        Task<UserDto> GetUserByUserName(string userName);

        Task<UserDto> CreateUser(UserDto user);

        Task<UserDto> UpdateUser(UserDto user);

        Task DeleteUser(Guid uuid);
    }
}
