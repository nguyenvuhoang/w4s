namespace O24OpenAPI.ACT.Domain.Common;

/// <summary>
/// Constants
/// </summary>
public static class Constants
{
    #region SPARAMGROUP
    /// <summary>
    /// ACT
    /// </summary>
    public const string ACT = "ACT";
    /// <summary>
    /// FORMAT
    /// </summary>
    public const string FORMAT = "FORMAT";
    #endregion
    #region SPARAMGROUP_ACT
    /// <summary>
    /// C_GEN_ACNO
    /// </summary>
    public const string C_GenerateAccountNumber = "GS";
    /// <summary>
    /// C_GEN_BACNO
    /// </summary>
    public const string C_GenerateBankAccountNumber = "GB";
    #endregion

    /// <summary>
    /// Lenght column deposit account number
    /// </summary>
    public const int ACCOUNT_NUMBER_LENGHT = 13;

    /// <summary>
    /// Lenght column account chart bank account number
    /// </summary>
    public const int BANK_ACCOUNT_NUMBER_LENGHT = 21;

    /// <summary>
    /// POSTING = "UpdatePosting"
    /// </summary>
    public const string POSTING = "UpdatePosting";

    /// <summary>
    /// ICIC
    /// </summary>
    public const StringComparison ICIC = StringComparison.InvariantCultureIgnoreCase;

    #region  Constanst for BALANCESIDE
    /// <summary>
    /// BALANCESIDE
    /// </summary>
    public static class BalanceSide
    {
        /// <summary>
        /// DEBIT
        /// </summary>
        public const string Debit = "D";

        /// <summary>
        /// CREDIT
        /// </summary>
        public const string Credit = "C";

        /// <summary>
        /// BOTH
        /// </summary>
        public const string Both = "B";
    }


    #endregion

    #region  Constanst for POSTINGSIDE
    /// <summary>
    ///
    /// </summary>
    public static class PostingSide
    {
        /// <summary>
        /// POSTINGSIDE_ALLOW_DEBIT
        /// </summary>
        public const string Debit = "D";
        /// <summary>
        /// POSTINGSIDE_ALLOW_CREDIT
        /// </summary>
        public const string Credit = "C";
        /// <summary>
        /// POSTINGSIDE_ALLOW_BOTH
        /// </summary>
        public const string Both = "A";
    }


    #endregion
    /// <summary>
    /// REVERSEBALANCE
    /// </summary>
    public static class DirectPosting
    {
        /// <summary>
        /// REVERSEBALANCE_YES
        /// </summary>
        public const string Yes = "Y";
        /// <summary>
        /// REVERSEBALANCE_NO _N
        /// </summary>
        public const string No = "N";
    }
    #region  Constanst for ReverseBalance
    /// <summary>
    /// REVERSEBALANCE
    /// </summary>
    public static class ReverseBalance
    {
        /// <summary>
        /// REVERSEBALANCE_YES
        /// </summary>
        public const string Yes = "Y";
        /// <summary>
        /// REVERSEBALANCE_NO _N
        /// </summary>
        public const string No = "N";
    }

    #endregion

    #region  Constanst for INVISIBLE ( IN ACTIVE)
    /// <summary>
    ///
    /// </summary>
    public static class Invisible
    {
        /// <summary>
        /// INVISIBLE_Y
        /// </summary>
        public const string Yes = "Y";
        /// <summary>
        /// INVISIBLE_N
        /// </summary>
        public const string No = "N";
    }


    #endregion

    #region  Constanst for checkmsgtransaction

    /// <summary>
    /// Checkmsgtransaction
    /// </summary>
    public static class Checkmsgtransaction
    {
        /// <summary>
        /// INVISIBLE_Y
        /// </summary>
        public const string Credit = "C";
        /// <summary>
        /// INVISIBLE_N
        /// </summary>
        public const string Debit = "D";
    }

    #endregion
    /// <summary>
    /// List module
    /// </summary>
    public static class Module
    {
        /// <summary>
        /// Deposit = "DPT"
        /// </summary>
        public const string Deposit = "DPT";
        /// <summary>
        /// FixedAsset
        /// </summary>
        public const string FixedAsset = "FAC";

        /// <summary>
        /// Credit = "CRD"
        /// </summary>
        public const string Credit = "CRD";

        /// <summary>
        /// Customer = "CTM"
        /// </summary>
        public const string Customer = "CTM";

        /// <summary>
        /// IFC = "IFC"
        /// </summary>
        public const string IFC = "IFC";

        /// <summary>
        /// Administration = "ADM"
        /// </summary>
        public const string Administration = "ADM";

        /// <summary>
        /// Payment = "PMT"
        /// </summary>
        public const string Payment = "PMT";

        /// <summary>
        /// Accouting = "ACT"
        /// </summary>
        public const string Accounting = "ACT";
        /// <summary>
        /// Cash = "CSH"
        /// </summary>
        public const string Cash = "CSH";

        /// <summary>
        /// Mortgage = "MTG"
        /// </summary>
        public const string Mortgage = "MTG";

        /// <summary>
        /// Job = "JOB"
        /// </summary>
        public const string Job = "JOB";

        /// <summary>
        /// UserManagement = "UMG"
        /// </summary>
        public const string UserManagement = "UMG";

        /// <summary>
        /// FrontOffice = "FOF"
        /// </summary>
        public const string FrontOffice = "FOF";
        /// <summary>
        /// FX
        /// </summary>
        public const string FX = "FX";

    }

    /// <summary>
    /// Constant Status
    /// </summary>
    public static class Status
    {
        /// <summary>
        /// Pending = "P"
        /// </summary>
        public const string Pending = "P";

        /// <summary>
        /// Close = "C"
        /// </summary>
        public const string Close = "C";

        /// <summary>
        /// Normal = "N"
        /// </summary>
        public const string Normal = "N";

        /// <summary>
        /// New = "W"
        /// </summary>
        public const string New = "W";

        /// <summary>
        /// Reverse = "R"
        /// </summary>
        public const string Reverse = "R";

        /// <summary>
        /// Active = "A"
        /// </summary>
        public const string Active = "A";

        /// <summary>
        /// Success = "S"
        /// </summary>
        public const string Success = "S";

        /// <summary>
        /// Reject = "R"
        /// </summary>
        public const string Reject = "R";

        /// <summary>
        /// Block = "B"
        /// </summary>
        public const string Block = "B";

        /// <summary>
        /// Dormant = "D"
        /// </summary>
        public const string Dormant = "D";

        /// <summary>
        /// Maturity = "M"
        /// </summary>
        public const string Maturity = "M";

        /// <summary>
        /// Yes = "Y"
        /// </summary>
        public const string Yes = "Y";

        /// <summary>
        /// No = "N"
        /// </summary>
        public const string No = "N";
    }

    /// <summary>
    /// PostingErrorCode
    /// </summary>
    public static class PostingErrorCode
    {

        /// <summary>
        /// Success
        /// </summary>
        public const string Success = "0";

        /// <summary>
        /// Warning
        /// </summary>
        public const string Error = "1";

        /// <summary>
        /// Warning
        /// </summary>
        public const string Warning = "2";
        /// <summary>
        /// Pending
        /// </summary>
        public const string Pending = "3";

        /// <summary>
        /// Approve
        /// </summary>
        public const string Approve = "4";

        /// <summary>
        /// Reject
        /// </summary>
        public const string Reject = "5";
        /// <summary>
        /// Delete
        /// </summary>
        public const string Delete = "6";
        /// <summary>
        /// Reversal
        /// </summary>
        public const string Reversal = "7";
        /// <summary>
        /// New
        /// </summary>
        public const string New = "8";
    }

    /// <summary>
    /// AccountStatementStatus
    /// </summary>
    public static class AccountStatementStatus
    {
        /// <summary>
        /// Success
        /// </summary>
        public const string Normal = "N";

        /// <summary>
        /// Warning
        /// </summary>
        public const string Reversal = "R";
    }

    /// <summary>
    /// EntryAction
    /// </summary>
    public static class EntryAction
    {
        /// <summary>
        /// Debit
        /// </summary>
        public const string Debit = "D";

        /// <summary>
        /// Credit
        /// </summary>
        public const string Credit = "C";
    }
    public static class IsMultiCurrency
    {
        public const string Yes = "Y";
        public const string No = "N";
    }
}
