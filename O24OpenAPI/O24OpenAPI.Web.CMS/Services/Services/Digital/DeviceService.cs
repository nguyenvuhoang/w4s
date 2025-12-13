using LinqToDB;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services.Digital;

public class DeviceService(IRepository<D_DEVICE> deviceRepository) : IDeviceService
{
    private readonly IRepository<D_DEVICE> _deviceRepository = deviceRepository;

    public async Task InsertDevice(D_DEVICE device)
    {
        await _deviceRepository.Insert(device);
    }

    public async Task<D_DEVICE> GetByUserIdAndAppType(string userId, string appType)
    {
        return await _deviceRepository
            .Table.Where(device =>
                device.UserCode == userId
                && device.AppType == appType
                && device.Status == DeviceStatus.ACTIVE
            )
            .FirstOrDefaultAsync();
    }
}
