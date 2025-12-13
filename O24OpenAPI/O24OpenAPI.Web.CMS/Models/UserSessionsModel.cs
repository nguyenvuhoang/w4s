using O24OpenAPI.Web.CMS.Constant;

namespace O24OpenAPI.Web.CMS.Models;

public class ValidateSessionModel : BaseO24OpenAPIModel
{
    public ValidateSessionModel() { }

    public ValidateSessionModel(string errorCode)
    {
        ErrorCode = errorCode;
    }

    public ValidateSessionModel(UserSessions userSessions, bool isValid = true)
    {
        IsValid = isValid;
        IpAddress = userSessions?.Wsip ?? string.Empty;
        DeviceName = userSessions?.Wsname ?? string.Empty;
        ErrorCode = userSessions?.Acttype == SessionStatus.Login ? "CBS.InvalidSession" : "";
    }

    public bool IsValid { get; set; } = true;
    public string IpAddress { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string ErrorMsg { get; set; } = string.Empty;
    public string ErrorCode { get; set; } = string.Empty;
}
