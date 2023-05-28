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
        /// <param name="user">User to be created.</param>
        /// <returns></returns>
        Task CreateUser(UserDto user);

        /// <summary>
        /// Updates existing user.
        /// </summary>
        /// <param name="user">User to be updated.</param>
        /// <returns></returns>
        Task UpdateUser(UserDto user);

        /// <summary>
        /// Deletes existing user.
        /// </summary>
        /// <param name="uuid">User's ID.</param>
        /// <returns></returns>
        Task DeleteUser(Guid uuid);
    }
}