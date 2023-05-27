using Common.Entities;
using Common.Interfaces;

namespace UserIntegrationLambda.Interfaces
{
    /// <summary>
    /// Interface of Users datastore write service.
    /// </summary>
    public interface IUsersWriteRepository : IWriteRepository<UserDto>
    {

    }
}
