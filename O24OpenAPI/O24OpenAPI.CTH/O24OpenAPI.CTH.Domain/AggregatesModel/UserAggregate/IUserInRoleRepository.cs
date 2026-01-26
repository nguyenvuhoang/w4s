using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public interface IUserInRoleRepository : IRepository<UserInRole>
{
    Task<List<UserInRole>> ListOfRole(string usercode);
    Task DeleteByUserCodeAsync(string userCode);
    Task<List<UserInRole>> GetListRoleByUserCodeAsync(string userCode);
    Task<List<UserInRole>> GetUserInRolesByRoleIdAsync(int roleId);
}
