using Common.Entities;

namespace UserIntegrationLambda.Interfaces
{
    /// <summary>
    /// Interface of Users datastore write service.
    /// </summary>
    public interface IUsersWriteRepository
    {
        /// <summary>
        /// Creates or updates a user in datastore.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Saved user dto.</returns>
        Task<UserDto> SaveAsync(UserDto user);

        /// <summary>
        /// Deletes user from the datastore.
        /// </summary>
        /// <param name="id">User unique identifier.</param>
        /// <returns>Deleted user dto.</returns>
        Task<UserDto> DeleteAsync(Guid id);
    }
}
