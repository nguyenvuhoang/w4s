using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Constant;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserDeviceRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserDevice>(eventPublisher, dataProvider, staticCacheManager),
        IUserDeviceRepository
{
    public async Task<UserDevice?> GetActiveByUserCodeAsync(string userCode)
    {
        if (string.IsNullOrWhiteSpace(userCode))
        {
            return null;
        }

        return await Table
            .Where(d => d.UserCode == userCode && d.Status == Code.ShowStatus.ACTIVE)
            .FirstOrDefaultAsync();
    }

    public async Task<UserDevice?> GetByUserAndDeviceAsync(
        string userCode,
        string deviceId,
        string deviceType
    )
    {
        return await Table.FirstOrDefaultAsync(d =>
            d.UserCode == userCode && d.DeviceId == deviceId && d.DeviceType == deviceType
        );
    }

    public async Task<UserDevice?> GetActiveByUserCodeAnyDeviceAsync(string userCode)
    {
        return await Table.FirstOrDefaultAsync(d =>
            d.UserCode == userCode && d.Status == DeviceStatus.ACTIVE
        );
    }

    public async Task<List<UserDevice>> GetActiveMobileDevicesWithPushIdAsync()
    {
        return await Table
            .Where(d =>
                d.ChannelId == Code.Channel.MB
                && d.Status == DeviceStatus.ACTIVE
                && !string.IsNullOrEmpty(d.PushId)
            )
            .ToListAsync();
    }

    public Task<UserDevice> EnsureUserDeviceAsync(
        string userCode,
        string loginName,
        string deviceId,
        string deviceType,
        string userAgent,
        string ipAddress,
        string channelId,
        string pushId,
        string osVersion,
        string appVersion,
        string deviceName,
        string brand,
        bool isEmulator,
        bool isRooted,
        string language,
        bool isResetDevice,
        string network = "",
        string memory = ""
    )
    {
        throw new NotImplementedException();
    }
}
