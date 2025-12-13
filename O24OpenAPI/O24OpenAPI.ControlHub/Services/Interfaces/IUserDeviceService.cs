using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Core;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface IUserDeviceService
{
    Task<UserDevice> GetByIdAsync(int id);
    Task<UserDevice> GetByUserCodeAsync(string userCode);
    Task<IList<UserDevice>> GetAllAsync();

    Task<UserDevice> AddAsync(UserDevice entity);
    Task UpdateAsync(UserDevice entity);
    Task DeleteAsync(UserDevice entity);
    Task<IPagedList<UserDevice>> SimpleSearch(SimpleSearchModel model);

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
    Task<List<CTHUserNotificationModel>> GetMobileDevice();

}
