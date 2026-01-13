using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.O24ACT.Models;

public class TemporaryPosting : BaseO24OpenAPIModel
{
    /// <summary>
    /// AccountNumber
    /// </summary>
    public string AccountNumber { get; set; } = "";

    /// <summary>
    /// BranchAccountNumber
    /// </summary>
    public string BranchGLBankAccountNumber { get; set; }

    /// <summary>
    /// CurrencyCodeGLBankAccountNumber
    /// </summary>
    public string CurrencyCodeGLBankAccountNumber { get; set; }

    /// <summary>
    /// CurrencyCode
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    public decimal OriginalAmount { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// acname
    /// </summary>
    public string AccountName { get; set; }

    /// <summary>
    /// dorc
    /// </summary>
    public string DebitOrCredit { get; set; }

    /// <summary>
    /// BalanceSide
    /// </summary>
    public string BalanceSide { get; set; }

    /// <summary>
    /// AccountLevel
    /// </summary>
    public int AccountLevel { get; set; }

    /// <summary>
    /// acgrp
    /// </summary>
    public int AccountingEntryGroup { get; set; } = 1;

    /// <summary>
    /// acidx
    /// </summary>
    public int AccountingEntryIndex { get; set; } = 1;

    /// <summary>
    /// bamt
    /// </summary>
    public string BaseAmount { get; set; }

    /// <summary>
    /// brm
    /// </summary>
    public string MasterBranchCode { get; set; }

    /// <summary>
    /// IsCashAccount
    /// </summary>
    public bool IsCashAccount { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    public string ClearingType { get; set; } = "O";

    /// <summary>
    ///
    /// </summary>
    public string SysAccountName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool PostGL { get; set; } = false;

    /// <summary>
    /// Group of account map from sending template
    /// </summary>
    public int GroupOfSendingTemplate { get; set; }

    /// <summary>
    /// Group of account map from sending template
    /// </summary>
    public string TransId { get; set; }

    /// <summary>
    /// Group of account map from sending template
    /// </summary>
    public string TransTableName { get; set; }
}
