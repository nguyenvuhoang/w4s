namespace O24OpenAPI.CMS.API.Application.Constants;

public class ModuleCode
{
    public const string System = "SYS";
    public const string TellerApp = "TELLERAPP";
    public const string CoreBanking = "CBS";
    public const string SmartOtp = "SMARTOPT";
    public const string Digital = "Digital";
}

public class ResourceType
{
    public const string SommeThingWrong = "00"; // Lỗi không xác định
    public const string Required = "01"; // Thiếu dữ liệu
    public const string Invalid = "02"; // Dữ liệu không hợp lệ
    public const string NotFound = "03"; // Không tìm thấy
    public const string Conflict = "04"; // Xung đột
    public const string NotAllow = "05"; // Không được phép
    public const string Unauthorized = "06"; // Chưa xác thực
    public const string Forbidden = "07"; // Cấm truy cập
    public const string Timeout = "08"; // Hết thời gian chờ
    public const string TooManyRequests = "09"; // Quá nhiều yêu cầu
    public const string InternalError = "10"; // Lỗi hệ thống
    public const string BadRequest = "11"; // Yêu cầu không hợp lệ
}

public class ErrorName
{
    public const string INVALID_SESSION_REFRESH = $"INVALID_SESSION_REFRESH";
    public const string CommandNotFound = "COMMAND_NOT_FOUND";
    public const string INVALID_WORKFLOW_DEF = "INVALID_WORKFLOW_DEF";
    public const string INVALID_WORKFLOW_STEP = "INVALID_WORKFLOW_STEP";
}
