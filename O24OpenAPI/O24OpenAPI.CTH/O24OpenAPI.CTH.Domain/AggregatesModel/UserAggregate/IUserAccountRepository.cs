using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public interface IUserAccountRepository : IRepository<UserAccount>
{
    Task<int> GetNextRoleIdAsync();

    Task UpdateAsync(UserAccount userAccount);
    Task<UserAccount> GetByUserCodeAsync(string userCode);
}
