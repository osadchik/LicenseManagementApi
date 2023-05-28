using Common.Entities;
using Common.Interfaces;

namespace LicenseManagementLambda.Interfaces
{
    /// <summary>
    /// Interface of License datastore service.
    /// </summary>
    public interface ILicenseRepository : IReadRepository<LicenseDto>, IWriteRepository<LicenseDto>
    {

    }
}
