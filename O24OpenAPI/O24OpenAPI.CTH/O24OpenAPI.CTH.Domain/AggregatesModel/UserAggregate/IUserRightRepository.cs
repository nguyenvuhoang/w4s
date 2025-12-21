using O24OpenAPI.Core.SeedWork;
using System;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRightRepository : IRepository<UserRight>
    {
        Task<HashSet<string>> GetSetChannelInRoleAsync(int roleId);
        Task<HashSet<string>> GetSetChannelInRoleAsync(int[] roleId);
    }
}