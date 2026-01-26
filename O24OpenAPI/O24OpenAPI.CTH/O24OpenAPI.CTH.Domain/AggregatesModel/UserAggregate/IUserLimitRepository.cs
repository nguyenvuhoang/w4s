using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public interface IUserLimitRepository : IRepository<UserLimit>
{
    Task<UserLimit> GetUserLimitToUpdate(int roleId, string commandId, string currencyCode, string limitType);
}
