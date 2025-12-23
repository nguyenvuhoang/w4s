using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public interface IUserRightChannelRepository : IRepository<UserRightChannel>
{
    Task<HashSet<string>> GetSetChannelInRoleAsync(int roleId);
    Task<HashSet<string>> GetSetChannelInRoleAsync(int[] roleId);
    Task<List<int>> GetListRoleIdByChannelAsync(string channelId);
}
