using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.CTH.Constant;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserDeviceRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<UserDevice>(dataProvider, staticCacheManager), IUserDeviceRepository
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

    public async Task<UserDevice> EnsureUserDeviceAsync(
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
        var now = DateTime.UtcNow;

        var currentDevice = await Table.FirstOrDefaultAsync(d =>
            d.UserCode == userCode && d.DeviceId == deviceId && d.DeviceType == deviceType
        );

        var activeDevice = await Table.FirstOrDefaultAsync(d =>
            d.UserCode == userCode && d.Status == DeviceStatus.ACTIVE
        );

        if (currentDevice?.Status == DeviceStatus.ACTIVE)
        {
            currentDevice.UserAgent = userAgent ?? currentDevice.UserAgent;
            currentDevice.IpAddress = ipAddress ?? currentDevice.IpAddress;
            currentDevice.PushId = pushId ?? currentDevice.PushId;
            currentDevice.LastSeenDateUtc = now;

            await UpdateAsync(currentDevice);
            return currentDevice;
        }

        if (channelId != Code.Channel.BO)
        {
            if (!isResetDevice && activeDevice != null && activeDevice.DeviceId != deviceId)
            {
                throw await O24Exception.CreateWithNextActionAsync(
                    O24CTHResourceCode.Operation.HaveLoggged,
                    $"VERIFY|usercode={userCode}",
                    language,
                    [deviceId, loginName]
                );
            }
        }

        if (currentDevice == null)
        {
            currentDevice = new UserDevice
            {
                UserCode = userCode,
                DeviceId = deviceId ?? Guid.NewGuid().ToString(),
                DeviceType = deviceType ?? string.Empty,
                ChannelId = channelId,
                Status = DeviceStatus.ACTIVE,
                PushId = pushId,
                UserAgent = userAgent ?? string.Empty,
                IpAddress = ipAddress ?? string.Empty,
                OsVersion = osVersion ?? string.Empty,
                AppVersion = appVersion ?? string.Empty,
                DeviceName = deviceName ?? string.Empty,
                Brand = brand ?? string.Empty,
                IsEmulator = isEmulator,
                IsRootedOrJailbroken = isRooted,
                LastSeenDateUtc = now,
                Network = network ?? string.Empty,
                Memory = memory ?? string.Empty,
            };

            if (activeDevice != null && activeDevice.DeviceId != currentDevice.DeviceId)
            {
                activeDevice.Status = DeviceStatus.INACTIVE;
                await UpdateAsync(activeDevice);
            }

            return await AddAsync(currentDevice);
        }
        currentDevice.DeviceId = deviceId;
        currentDevice.Status = DeviceStatus.ACTIVE;
        currentDevice.UserAgent = userAgent ?? currentDevice.UserAgent;
        currentDevice.IpAddress = ipAddress ?? currentDevice.IpAddress;
        currentDevice.PushId = pushId ?? currentDevice.PushId;
        currentDevice.Network = network ?? currentDevice.Network;
        currentDevice.Memory = memory ?? currentDevice.Memory;
        currentDevice.LastSeenDateUtc = now;

        if (activeDevice != null && activeDevice.DeviceId != currentDevice.DeviceId)
        {
            activeDevice.Status = DeviceStatus.INACTIVE;
            await UpdateAsync(activeDevice);
        }

        await UpdateAsync(currentDevice);
        return currentDevice;
    }

    public async Task UpdateAsync(UserDevice entity)
    {
        await Update(entity);
    }

    public async Task<UserDevice> AddAsync(UserDevice entity)
    {
        return await InsertAsync(entity);
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

    public virtual async Task<UserDevice> GetByUserCodeAsync(string userCode)
    {
        if (string.IsNullOrWhiteSpace(userCode))
        {
            return null;
        }

        try
        {
            var query = Table;
            return query == null
                ? throw new InvalidOperationException("UserDevice repository table is null.")
                : await query
                    .Where(d => d.UserCode == userCode && d.Status == Code.ShowStatus.ACTIVE)
                    .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw;
        }
    }
}
