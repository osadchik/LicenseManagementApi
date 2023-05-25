using Common.Entities;

namespace UserIntegrationLambda.Interfaces
{
    /// <summary>
    /// Interface of user synchronization service between lambda and downstream consumer.
    /// </summary>
    public interface IUserIntegrationHandler
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task CreateUser(UserDto user);

        /// <summary>
        /// Updates existing user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateUser(UserDto user);

        /// <summary>
        /// Deletes existing user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task DeleteUser(UserDto user);
    }
}