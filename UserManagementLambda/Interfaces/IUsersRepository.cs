using UserManagementLambda.Entities;

namespace UserManagementLambda.Interfaces
{
    /// <summary>
    /// Interface used to access Users datastore.
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Creates or updates a user in datastore.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Saved user dto.</returns>
        Task<UserDto> SaveAsync(UserDto user);

        /// <summary>
        /// Gets user by id.
        /// </summary>
        /// <param name="id">User unique indentifier.</param>
        /// <returns>User dto.</returns>
        Task<UserDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Deletes user from the datastore.
        /// </summary>
        /// <param name="id">User unique identifier.</param>
        /// <returns>Deleted user dto.</returns>
        Task<UserDto> DeleteAsync(Guid id);
    }
}
