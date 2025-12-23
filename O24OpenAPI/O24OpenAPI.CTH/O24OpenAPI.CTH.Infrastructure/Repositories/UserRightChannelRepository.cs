using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserRightChannelRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserRightChannel>(eventPublisher, dataProvider, staticCacheManager),
        IUserRightChannelRepository
{
    public async Task<List<UserRightChannel>> GetByRoleIdAsync(int roleId)
    {
        return await Table.Where(x => x.RoleId == roleId).ToListAsync();
    }

    public async Task<List<int>> GetListRoleIdByChannelAsync(string channelId)
    {
        return await Table.Where(s => s.ChannelId == channelId).Select(s => s.RoleId).ToListAsync();
    }

    public async Task<HashSet<string>> GetSetChannelInRoleAsync(int roleId)
    {
        return await Table
            .Where(s => s.RoleId == roleId)
            .Select(s => s.ChannelId)
            .AsAsyncEnumerable()
            .ToHashSetAsync();
    }

    public async Task<HashSet<string>> GetSetChannelInRoleAsync(int[] roleId)
    {
        HashSet<string> result = new();
        foreach (var role in roleId)
        {
            var set = await Table
                .Where(s => s.RoleId == role)
                .Select(s => s.ChannelId)
                .AsAsyncEnumerable()
                .ToHashSetAsync();

            result.UnionWith(set);
        }
        return result;
    }
}
