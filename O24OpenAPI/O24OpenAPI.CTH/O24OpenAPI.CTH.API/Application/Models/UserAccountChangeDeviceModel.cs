using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models;

public class UserAccountChangeDeviceModel : BaseTransactionModel
{
    public string UserCode { get; set; }
    public string Phone { get; set; }
    public DateTime DOB { get; set; }
    public string LicenseType { get; set; }
    public string LicenseId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string Modelname { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string PushId { get; set; } = string.Empty;
    public string OsVersion { get; set; } = string.Empty;
    public string AppVersion { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public bool IsEmulator { get; set; } = false;
    public bool IsRootedOrJailbroken { get; set; } = false;
}
