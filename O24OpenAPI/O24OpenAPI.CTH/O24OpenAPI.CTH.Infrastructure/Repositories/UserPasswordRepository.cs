using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserPasswordRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserPassword>(eventPublisher, dataProvider, staticCacheManager),
        IUserPasswordRepository
{
    public async Task<UserPassword?> GetByUserCodeAsync(string userCode)
    {
        return await Table.Where(s => s.UserId == userCode).FirstOrDefaultAsync();
    }

    public async Task<UserPassword?> GetByChannelAndUserAsync(string channelId, string userCode)
    {
        return await Table
            .Where(s => s.ChannelId == channelId && s.UserId == userCode)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteByUserIdAsync(string userId)
    {
        var entity = await Table.FirstOrDefaultAsync(x => x.UserId == userId);
        if (entity != null)
        {
            await Delete(entity);
        }
    }

    public async Task UpdateAsync(UserPassword entity)
    {
        await Update(entity);
    }
}
