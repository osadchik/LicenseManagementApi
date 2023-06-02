using Common.Entities;

namespace UserManagementLambda.Interfaces
{
    /// <summary>
    /// Interface of user synchronization adapter. 
    /// </summary>
    public interface IUserManagementService
    {
        /// <summary>
        /// Gets user by uuid from the datastore.
        /// </summary>
        /// <param name="uuid">User's ID.</param>
        /// <returns></returns>
        Task<UserDto> GetUserByUuid(Guid uuid);

        /// <summary>
        /// Gets user by username from the datastore.
        /// </summary>
        /// <param name="userName">User's name.</param>
        /// <returns></returns>
        Task<IList<UserDto>> GetUserByUserName(string userName);

        /// <summary>
        /// Sends user creation request to the SNS topic.
        /// </summary>
        /// <param name="user"><see cref="UserDto"/></param>
        /// <returns></returns>
        Task<UserDto> CreateUser(UserDto user);

        /// <summary>
        /// Sends user updation request to the SNS topic.
        /// </summary>
        /// <param name="user"><see cref="UserDto"/></param>
        /// <returns></returns>
        Task<UserDto> UpdateUser(UserDto user);

        /// <summary>
        /// Sends user deletion request to the SNS topic.
        /// </summary>
        /// <param name="uuid">User's ID.</param>
        /// <returns></returns>
        Task DeleteUser(Guid uuid);
    }
}
