using LinqToDB;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.CTH.API.Application.Utils;
using O24OpenAPI.CTH.Constant;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<bool> DeleteBulkAsync();
        Task UpdateAsync(UserAccount userAccount);
        Task<List<int>> GetByRoleTypeAsync(string roletype);
    }
}
