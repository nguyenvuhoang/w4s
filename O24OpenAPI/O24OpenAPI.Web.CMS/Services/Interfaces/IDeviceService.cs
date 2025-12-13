namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IDeviceService
{
    Task<D_DEVICE> GetByUserIdAndAppType(string userId, string appType);
    Task InsertDevice(D_DEVICE device);
}
