using Common.Entities;

namespace UserManagementLambda.Interfaces
{
    /// <summary>
    /// Interface of Users datastore read service.
    /// </summary>
    public interface IUsersReadRepository
    {
        /// <summary>
        /// Gets user by id.
        /// </summary>
        /// <param name="id">User unique indentifier.</param>
        /// <returns>User dto.</returns>
        Task<UserDto> GetByIdAsync(Guid id);
    }
}
