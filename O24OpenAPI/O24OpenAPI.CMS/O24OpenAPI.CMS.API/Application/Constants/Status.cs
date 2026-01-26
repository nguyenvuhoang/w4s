namespace O24OpenAPI.CMS.API.Application.Constants;

public class TranStatus
{
    public const string PENDING = "P";
    public const string COMPLETED = "C";
    public const string FAILED = "F";
    public const string REJECTED = "R";
    public const string CANCELLED = "X";
    public const string EXPIRED = "E";
    public const string REVERSED = "V";
    public const string REVERSAL_FAILED = "VF";
    public const string REVERSAL_PENDING = "VP";
    public const string REVERSAL_COMPLETED = "VC";
    public const string ERROR = "E";
}

public class SessionStatus
{
    public const string Login = "I";
    public const string Logout = "O";
    public const string Locked = "L";
    public const string Expired = "E";
    public const string StaticToken = "S";
}

public class WorkflowStatus
{
    public const bool Active = true;
    public const bool Inactive = false;
}
