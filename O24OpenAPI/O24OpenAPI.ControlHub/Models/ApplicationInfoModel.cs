using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class ApplicationInfoModel : BaseTransactionModel
{
    public ApplicationInfoModel() { }
}


public class ApplicationInfoResponseModel(
    string userCode,
    string avatar,
    object userCommand,
    string fullName,
    string loginName,
    DateTime? lastLoginTime,
    string contractNumber,
    bool isBiometricSupported,
    bool isSmartOTPActive,
    bool? isLogin,
    string userBanner,
    List<UserInRole> role,
    bool? isFirstLogin)
{
    public string UserCode { get; set; } = userCode;
    public string Avatar { get; set; } = avatar;
    public object UserCommand { get; set; } = userCommand;
    public string Name { get; set; } = fullName;
    public string LoginName { get; set; } = loginName;
    public bool? IsFirstLogin { get; set; } = isFirstLogin;
    public string ContractNumber { get; set; } = contractNumber;
    public bool IsBiometricSupported { get; set; } = isBiometricSupported;
    public bool IsSmartOTPActive { get; set; } = isSmartOTPActive;
    public bool? IsLogin { get; set; } = isLogin;
    public List<UserInRole> Role { get; set; } = role;
    public string UserBanner { get; set; } = userBanner;
    public DateTime LastLoginTime { get; set; } = lastLoginTime ?? DateTime.MinValue;
}
