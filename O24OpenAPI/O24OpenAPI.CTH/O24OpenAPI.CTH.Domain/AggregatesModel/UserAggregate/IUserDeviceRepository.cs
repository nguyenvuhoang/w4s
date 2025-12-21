using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public interface IUserDeviceRepository : IRepository<UserDevice>
{
    Task<UserDevice> EnsureUserDeviceAsync(
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
   );
}
