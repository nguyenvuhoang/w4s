using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.ControlHub.Constant;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Utils;

namespace O24OpenAPI.ControlHub.Services;

public class UserDeviceService(
    IRepository<UserDevice> entityRepository,
    IRepository<UserAccount> userAccountRepository
) : IUserDeviceService
{
    private readonly IRepository<UserDevice> _entityRepository = entityRepository;
    private readonly IRepository<UserAccount> _userAccountRepository = userAccountRepository;

    public virtual async Task<UserDevice> GetByIdAsync(int id)
    {
        return await _entityRepository.GetById(id, cache => null);
    }

    public virtual async Task<IList<UserDevice>> GetAllAsync()
    {
        return await _entityRepository.GetAll(query => query);
    }

    /// <summary>
    /// Get UserDevice by UserCode
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual async Task<UserDevice> GetByUserCodeAsync(string userCode)
    {
        if (string.IsNullOrWhiteSpace(userCode))
        {
            return null;
        }

        try
        {
            var query = _entityRepository.Table;
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

    public async Task UpdateAsync(UserDevice entity)
    {
        await _entityRepository.Update(entity);
    }

    public async Task<UserDevice> AddAsync(UserDevice entity)
    {
        return await _entityRepository.InsertAsync(entity);
    }

    public async Task DeleteAsync(UserDevice entity)
    {
        await _entityRepository.InsertAsync(entity);
    }

    public async Task<IPagedList<UserDevice>> SimpleSearch(SimpleSearchModel model)
    {
        var query =
            from d in _entityRepository.Table
            where
                (!string.IsNullOrEmpty(model.SearchText) && d.UserCode.Contains(model.SearchText))
                || true
            select d;
        return await query.ToPagedList(model.PageIndex, model.PageSize);
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

        var currentDevice = await _entityRepository.Table.FirstOrDefaultAsync(d =>
            d.UserCode == userCode && d.DeviceId == deviceId && d.DeviceType == deviceType
        );

        var activeDevice = await _entityRepository.Table.FirstOrDefaultAsync(d =>
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

    /// <summary>
    /// Get all active mobile devices
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<CTHUserNotificationModel>> GetMobileDevice()
    {
        var query =
            from d in _entityRepository.Table
            join u in _userAccountRepository.Table on d.UserCode equals u.UserCode into gj
            from ua in gj.DefaultIfEmpty()
            where
                d.ChannelId == Code.Channel.MB
                && d.Status == DeviceStatus.ACTIVE
                && !string.IsNullOrEmpty(d.PushId)
            select new CTHUserNotificationModel
            {
                UserCode = d.UserCode,
                PushId = d.PushId ?? string.Empty,
                PhoneNumber =
                    ua != null && !string.IsNullOrEmpty(ua.Phone) ? ua.Phone : string.Empty,
                UserDevice = d.DeviceId,
            };

        var result = await query.ToListAsync();

        ConsoleUtil.WriteInfo($"Count Mobile Device = {result.Count}");

        return result;
    }
}
