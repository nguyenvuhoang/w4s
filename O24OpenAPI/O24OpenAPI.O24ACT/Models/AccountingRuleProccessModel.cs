using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public partial class AccountingRuleProccessModel : BaseTransactionModel
{
    /// <summary>
    /// EntryJournals
    /// </summary>
    public List<TemporaryPosting> EntryJournals { get; set; } = new List<TemporaryPosting> { };

    /// <summary>
    /// Interest Fee Charge Model
    /// </summary>
    /// <summary>
    /// WorkingBranhCode
    /// </summary>
    public string WorkingBranhCode { get; set; }

    /// <summary>
    /// brm
    /// </summary>
    public string MasterBranch { get; set; }

    /// <summary>
    /// iscompress
    /// </summary>
    public bool IsCompress { get; set; }

    /// <summary>
    /// clrtype
    /// </summary>
    public string ClearingType { get; set; }

    /// <summary>
    /// fxclrtype
    /// </summary>
    public string FXClearingType { get; set; }

    /// <summary>
    /// acctype
    /// </summary>
    public string AccountingEntry { get; set; }
}
