using Common.Entities;
using Common.Interfaces;

namespace LicenseManagementLambda.Interfaces
{
    public interface ILicenseRepository : IReadRepository<LicenseDto>, IWriteRepository<LicenseDto>
    {

    }
}
