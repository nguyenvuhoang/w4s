//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.CTH.Constant;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
//using O24OpenAPI.CTH.API.Application.Constants;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class EnsureUserDeviceCommnad: BaseTransactionModel, ICommand<UserDevice>
//    {

//    }

//    public class EnsureUserDeviceHandler(IUserDeviceRepository userDeviceRepository) : ICommandHandler<EnsureUserDeviceCommnad, UserDevice>
//    {
//        public async Task<UserDevice> HandleAsync(EnsureUserDeviceCommnad request, CancellationToken cancellationToken = default)
//        {
//        var now = DateTime.UtcNow;

//            var currentDevice = await userDeviceRepository.Table.FirstOrDefaultAsync(d =>
//                d.UserCode == userCode && d.DeviceId == deviceId && d.DeviceType == deviceType
//            );

//            var activeDevice = await userDeviceRepository.Table.FirstOrDefaultAsync(d =>
//                d.UserCode == userCode && d.Status == DeviceStatus.ACTIVE
//            );

//            if (currentDevice?.Status == DeviceStatus.ACTIVE)
//            {
//                currentDevice.UserAgent = userAgent ?? currentDevice.UserAgent;
//                currentDevice.IpAddress = ipAddress ?? currentDevice.IpAddress;
//                currentDevice.PushId = pushId ?? currentDevice.PushId;
//                currentDevice.LastSeenDateUtc = now;

//                await UpdateAsync(currentDevice);
//                return currentDevice;
//            }

//            if (channelId != Code.Channel.BO)
//            {
//                if (!isResetDevice && activeDevice != null && activeDevice.DeviceId != deviceId)
//                {
//                    throw await O24Exception.CreateWithNextActionAsync(
//                        O24CTHResourceCode.Operation.HaveLoggged,
//                        $"VERIFY|usercode={userCode}",
//                        language,
//                        [deviceId, loginName]
//                    );
//                }
//            }

//            if (currentDevice == null)
//            {
//                currentDevice = new UserDevice
//                {
//                    UserCode = userCode,
//                    DeviceId = deviceId ?? Guid.NewGuid().ToString(),
//                    DeviceType = deviceType ?? string.Empty,
//                    ChannelId = channelId,
//                    Status = DeviceStatus.ACTIVE,
//                    PushId = pushId,
//                    UserAgent = userAgent ?? string.Empty,
//                    IpAddress = ipAddress ?? string.Empty,
//                    OsVersion = osVersion ?? string.Empty,
//                    AppVersion = appVersion ?? string.Empty,
//                    DeviceName = deviceName ?? string.Empty,
//                    Brand = brand ?? string.Empty,
//                    IsEmulator = isEmulator,
//                    IsRootedOrJailbroken = isRooted,
//                    LastSeenDateUtc = now,
//                    Network = network ?? string.Empty,
//                    Memory = memory ?? string.Empty,
//                };

//                if (activeDevice != null && activeDevice.DeviceId != currentDevice.DeviceId)
//                {
//                    activeDevice.Status = DeviceStatus.INACTIVE;
//                    await UpdateAsync(activeDevice);
//                }

//                return await AddAsync(currentDevice);
//            }
//            currentDevice.DeviceId = deviceId;
//            currentDevice.Status = DeviceStatus.ACTIVE;
//            currentDevice.UserAgent = userAgent ?? currentDevice.UserAgent;
//            currentDevice.IpAddress = ipAddress ?? currentDevice.IpAddress;
//            currentDevice.PushId = pushId ?? currentDevice.PushId;
//            currentDevice.Network = network ?? currentDevice.Network;
//            currentDevice.Memory = memory ?? currentDevice.Memory;
//            currentDevice.LastSeenDateUtc = now;

//            if (activeDevice != null && activeDevice.DeviceId != currentDevice.DeviceId)
//            {
//                activeDevice.Status = DeviceStatus.INACTIVE;
//                await UpdateAsync(activeDevice);
//            }

//            await UpdateAsync(currentDevice);
//            return currentDevice;
//        }
//    }
//}
