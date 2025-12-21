using O24OpenAPI.Core.SeedWork;
using System;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate
{
    public interface IUserAvatarRepository : IRepository<UserAvatar>
    {
        Task<UserAvatar> GetByUserCodeAsync(string request);

    }
}
