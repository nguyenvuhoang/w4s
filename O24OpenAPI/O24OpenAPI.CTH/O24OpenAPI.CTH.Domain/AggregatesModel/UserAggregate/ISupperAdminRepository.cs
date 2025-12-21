using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public interface ISupperAdminRepository : IRepository<SupperAdmin>
{
    Task<SupperAdmin> IsExit();
}
