using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.RulesAggregate;

/// <summary>
/// AccountChart
/// </summary>
public partial class AccountingRuleDefinition : BaseEntity
{
    /// <summary>
    /// txcode
    /// </summary>
    public string? TransactionCode { get; set; }

    /// <summary>
    /// txcode
    /// </summary>
    public string? StepCode { get; set; }

    /// <summary>
    /// txcode
    /// </summary>
    public string? NodeData { get; set; }

    /// <summary>
    /// actype
    /// </summary>
    public string? AccountType { get; set; }

    /// <summary>
    /// acname
    /// </summary>
    public string? AccountName { get; set; }

    /// <summary>
    /// txcode
    /// </summary>
    public string? EntryCondition { get; set; }

    /// <summary>
    /// enparmcd
    /// </summary>
    public string? EntryParameterCode { get; set; }

    /// <summary>
    /// enparmval
    /// </summary>
    public string? EntryParameterValue { get; set; }

    /// <summary>
    /// dorc
    /// </summary>
    public string? DebitOrCredit { get; set; }

    /// <summary>
    /// valtype
    /// </summary>
    public string? ValueTypeOfAmount { get; set; }

    /// <summary>
    /// valtag
    /// </summary>
    public string? TagOfField { get; set; }

    /// <summary>
    /// mval
    /// </summary>
    public string? DirectValue { get; set; }

    /// <summary>
    /// valname
    /// </summary>
    public string? ValueName { get; set; }

    /// <summary>
    /// acgrp
    /// </summary>
    public int AccountingEntryGroup { get; set; }

    /// <summary>
    /// acidx
    /// </summary>
    public int AccountingEntryIndex { get; set; }

    /// <summary>
    /// acctype
    /// </summary>
    public string? AccountingEntry { get; set; }

    /// <summary>
    /// mdlcode
    /// </summary>
    public string? ModuleCode { get; set; }

    /// <summary>
    /// mdlkeyval
    /// </summary>
    public string? AccountMasterTag { get; set; }

    /// <summary>
    /// bamt
    /// </summary>
    public string? BaseAmount { get; set; }

    /// <summary>
    /// brw
    /// </summary>
    public string? WorkingBranch { get; set; }

    /// <summary>
    /// brm
    /// </summary>
    public string? MasterBranch { get; set; }

    /// <summary>
    /// ccr
    /// </summary>
    public string? Currency { get; set; }

    /// <summary>
    /// prn
    /// </summary>
    public string? Voucher { get; set; }

    /// <summary>
    /// iscompress
    /// </summary>
    public bool IsCompress { get; set; }

    /// <summary>
    /// clrtype
    /// </summary>
    public string? ClearingType { get; set; }

    /// <summary>
    /// fxclrtype
    /// </summary>
    public string? FXClearingType { get; set; }

    /// <summary>
    /// defacno
    /// </summary>
    public string? DefAccountNumber { get; set; }
}
