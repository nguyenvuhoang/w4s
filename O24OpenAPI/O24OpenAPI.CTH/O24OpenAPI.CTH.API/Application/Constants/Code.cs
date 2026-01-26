namespace O24OpenAPI.CTH.API.Application.Constants;

public partial class Code
{
    public partial class Transaction
    {
        public const string INTERBANK = "INTERBANK_TRANSFER";
        public const string DOMESTIC = "DOMESTIC_TRANSFER";
    }
    public partial class LogType
    {
        public const string INSERT = "I";
        public const string UPDATE = "U";
    }
    public partial class ReverseStatus
    {
        public const bool YES = true;
        public const bool NO = false;
    }
    public partial class UpdateType
    {
        public const string CREATE = "C";
        public const string DELETE = "D";
        public const string UPDATE = "U";
    }
    public partial class ShowStatus
    {
        public const string YES = "Y";
        public const string NO = "N";
        public const string ACTIVE = "A";
    }
    public partial class UserType
    {
        public const string BO = "BO";
    }
    public partial class Channel
    {
        public const string BO = "BO";
        public const string AM = "AM";
        public const string MB = "MB";
        public const string TELLERAPP = "TELLERAPP";
    }
}
