using Common.Entities;
using Common.Interfaces;

namespace UserManagementLambda.Interfaces
{
    /// <summary>
    /// Interface of Users datastore read service.
    /// </summary>
    public interface IUsersReadRepository : IReadRepository<UserDto>
    {
        /// <summary>
        /// Gets user by username.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <returns>User dto.</returns>
        Task<IList<UserDto>> GetByUsernameAsync(string username);
    }
}
