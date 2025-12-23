using System;
using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate
{
    public interface IUserPasswordRepository : IRepository<UserPassword>
    {
        Task<UserPassword?> GetByUserCodeAsync(string userCode);
        Task UpdateAsync(UserPassword entity);
    }
}
