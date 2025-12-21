using O24OpenAPI.Core.SeedWork;
using System;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate
    {
        public interface IUserInRoleRepository : IRepository<UserInRole>
        {
        public async Task<bool> DeleteBulkAsync(List<UserInRole> listUserInRole)
        {
            await BulkDelete(listUserInRole);
            return true;
        }
    }
    }
