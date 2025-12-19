namespace O24OpenAPI.O24ACT.Common;

public partial class O24ActResourceCode
{
    public partial class Account
    {
        public const string AccountNumberIsExisting = "accountchart.is.existing";
    }
    public partial class Validation
    {
        public const string AccountCommonNotDefined = "Accounting.PostingError.AccountCommonNotDefined";
        public const string UnBalancePosting = "Accounting.PostingError.UnBalancePostingCurrency";
        public const string AccountLevelNotAllow = "Accounting.AccountLevelNotAllow";
        public const string ACT_NOT_ALLOW_DEBIT_BAL = "Accounting.Error.ACT_NOT_ALLOW_DEBIT_BAL";
        public const string ACT_NOT_ALLOW_CREDIT_BAL = "Accounting.Error.ACT_NOT_ALLOW_CREDIT_BAL";
    }

}
