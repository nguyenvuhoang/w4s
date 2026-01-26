using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public interface IUserRightRepository : IRepository<UserRight>
{
    Task<UserRight> GetByRoleIdAndCommandIdAsync(int roleId, string commandId);
    Task<UserRight> AddUserRightAsync(UserRight entity);
    Task UpdateAsync(UserRight userRight);
}
