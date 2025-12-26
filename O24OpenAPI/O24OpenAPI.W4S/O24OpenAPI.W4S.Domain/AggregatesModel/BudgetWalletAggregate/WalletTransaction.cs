namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

using System.ComponentModel.DataAnnotations;
using O24OpenAPI.Core.Domain;

/// <summary>
/// Defines the <see cref="WalletTransaction" />
/// </summary>
public partial class WalletTransaction : BaseEntity
{
    /// <summary>
    /// Gets or sets the TransactionId
    /// </summary>
    public string? TransactionId { get; set; }

    /// <summary>
    /// Gets or sets the TransactionDate
    /// </summary>
    public DateTime TransactionDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the TransactionEndDate
    /// </summary>
    public DateTime? TransactionEndDate { get; set; }

    /// <summary>
    /// Gets or sets the TransactionWorkDate
    /// </summary>
    public DateTime? TransactionWorkDate { get; set; }

    /// <summary>
    /// Gets or sets the TransactionName
    /// </summary>
    public string TransactionName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the TransactionCode
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string? TransactionCode { get; set; }

    /// <summary>
    /// Gets or sets the CcyId
    /// </summary>
    [MaxLength(10)]
    public string? CcyId { get; set; }

    /// <summary>
    /// Gets or sets the SourceId
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string? SourceId { get; set; }

    /// <summary>
    /// Gets or sets the SourceTranRef
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string? SourceTranRef { get; set; }

    /// <summary>
    /// Gets or sets the UserId
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the UserCurApp
    /// </summary>
    [MaxLength(2000)]
    public string? UserCurApp { get; set; }

    /// <summary>
    /// Gets or sets the NextUserApp
    /// </summary>
    [MaxLength(2000)]
    public string? NextUserApp { get; set; }

    /// <summary>
    /// Gets or sets the ListUserApp
    /// </summary>
    public string? ListUserApp { get; set; }

    /// <summary>
    /// Gets or sets the TranDesc
    /// </summary>
    [Required]
    [MaxLength(400)]
    public string? TranDesc { get; set; }

    /// <summary>
    /// Gets or sets the Status
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the ApprSts
    /// </summary>
    public int ApprSts { get; set; }

    /// <summary>
    /// Gets or sets the OfflSts
    /// </summary>
    [Required]
    [MaxLength(1)]
    public string? OfflSts { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Gets or sets the DestId
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string? DestId { get; set; }

    /// <summary>
    /// Gets or sets the DestTranRef
    /// </summary>
    [MaxLength(20)]
    public string? DestTranRef { get; set; }

    /// <summary>
    /// Gets or sets the DestErrorCode
    /// </summary>
    [MaxLength(50)]
    public string? DestErrorCode { get; set; }

    /// <summary>
    /// Gets or sets the ErrorCode
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string ErrorCode { get; set; } = "";

    /// <summary>
    /// Gets or sets the ErrorDesc
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string ErrorDesc { get; set; } = "";

    /// <summary>
    /// Gets or sets a value indicating whether Online
    /// </summary>
    public bool Online { get; set; } = true;

    /// <summary>
    /// Gets or sets the Char01
    /// </summary>
    public string? Char01 { get; set; }

    /// <summary>
    /// Gets or sets the Char02
    /// </summary>
    public string? Char02 { get; set; }

    /// <summary>
    /// Gets or sets the Char03
    /// </summary>
    public string? Char03 { get; set; }

    /// <summary>
    /// Gets or sets the Char04
    /// </summary>
    public string? Char04 { get; set; }

    /// <summary>
    /// Gets or sets the Char05
    /// </summary>
    public string? Char05 { get; set; }

    /// <summary>
    /// Gets or sets the Char06
    /// </summary>
    public string? Char06 { get; set; }

    /// <summary>
    /// Gets or sets the Char07
    /// </summary>
    public string? Char07 { get; set; }

    /// <summary>
    /// Gets or sets the Char08
    /// </summary>
    public string? Char08 { get; set; }

    /// <summary>
    /// Gets or sets the Char09
    /// </summary>
    public string? Char09 { get; set; }

    /// <summary>
    /// Gets or sets the Char10
    /// </summary>
    public string? Char10 { get; set; }

    /// <summary>
    /// Gets or sets the Char11
    /// </summary>
    public string? Char11 { get; set; }

    /// <summary>
    /// Gets or sets the Char12
    /// </summary>
    public string? Char12 { get; set; }

    /// <summary>
    /// Gets or sets the Char13
    /// </summary>
    public string? Char13 { get; set; }

    /// <summary>
    /// Gets or sets the Char14
    /// </summary>
    public string? Char14 { get; set; }

    /// <summary>
    /// Gets or sets the Char15
    /// </summary>
    public string? Char15 { get; set; }

    /// <summary>
    /// Gets or sets the Char16
    /// </summary>
    public string? Char16 { get; set; }

    /// <summary>
    /// Gets or sets the Char17
    /// </summary>
    public string? Char17 { get; set; }

    /// <summary>
    /// Gets or sets the Char18
    /// </summary>
    public string? Char18 { get; set; }

    /// <summary>
    /// Gets or sets the Char19
    /// </summary>
    public string? Char19 { get; set; }

    /// <summary>
    /// Gets or sets the Char20
    /// </summary>
    public string? Char20 { get; set; }

    /// <summary>
    /// Gets or sets the Char21
    /// </summary>
    public string? Char21 { get; set; }

    /// <summary>
    /// Gets or sets the Char22
    /// </summary>
    public string? Char22 { get; set; }

    /// <summary>
    /// Gets or sets the Char23
    /// </summary>
    public string? Char23 { get; set; }

    /// <summary>
    /// Gets or sets the Char24
    /// </summary>
    public string? Char24 { get; set; }

    /// <summary>
    /// Gets or sets the Char25
    /// </summary>
    public string? Char25 { get; set; }

    /// <summary>
    /// Gets or sets the Char26
    /// </summary>
    public string? Char26 { get; set; }

    /// <summary>
    /// Gets or sets the Char27
    /// </summary>
    public string? Char27 { get; set; }

    /// <summary>
    /// Gets or sets the Char28
    /// </summary>
    public string? Char28 { get; set; }

    /// <summary>
    /// Gets or sets the Char29
    /// </summary>
    public string? Char29 { get; set; }

    /// <summary>
    /// Gets or sets the Char30
    /// </summary>
    public string? Char30 { get; set; }

    /// <summary>
    /// Gets or sets the Num01
    /// </summary>
    public decimal? Num01 { get; set; }

    /// <summary>
    /// Gets or sets the Num02
    /// </summary>
    public decimal? Num02 { get; set; }

    /// <summary>
    /// Gets or sets the Num03
    /// </summary>
    public decimal? Num03 { get; set; }

    /// <summary>
    /// Gets or sets the Num04
    /// </summary>
    public decimal? Num04 { get; set; }

    /// <summary>
    /// Gets or sets the Num05
    /// </summary>
    public decimal? Num05 { get; set; }

    /// <summary>
    /// Gets or sets the Num06
    /// </summary>
    public decimal? Num06 { get; set; }

    /// <summary>
    /// Gets or sets the Num07
    /// </summary>
    public decimal? Num07 { get; set; }

    /// <summary>
    /// Gets or sets the Num08
    /// </summary>
    public decimal? Num08 { get; set; }

    /// <summary>
    /// Gets or sets the Num09
    /// </summary>
    public decimal? Num09 { get; set; }

    /// <summary>
    /// Gets or sets the Num10
    /// </summary>
    public decimal? Num10 { get; set; }

    /// <summary>
    /// Gets or sets the Num11
    /// </summary>
    public decimal? Num11 { get; set; }

    /// <summary>
    /// Gets or sets the Num12
    /// </summary>
    public decimal? Num12 { get; set; }

    /// <summary>
    /// Gets or sets the Num13
    /// </summary>
    public decimal? Num13 { get; set; }

    /// <summary>
    /// Gets or sets the Num14
    /// </summary>
    public decimal? Num14 { get; set; }

    /// <summary>
    /// Gets or sets the Num15
    /// </summary>
    public decimal? Num15 { get; set; }

    /// <summary>
    /// Gets or sets the Num16
    /// </summary>
    public decimal? Num16 { get; set; }

    /// <summary>
    /// Gets or sets the Num17
    /// </summary>
    public decimal? Num17 { get; set; }

    /// <summary>
    /// Gets or sets the Num18
    /// </summary>
    public decimal? Num18 { get; set; }

    /// <summary>
    /// Gets or sets the Num19
    /// </summary>
    public decimal? Num19 { get; set; }

    /// <summary>
    /// Gets or sets the Num20
    /// </summary>
    public decimal? Num20 { get; set; }

    /// <summary>
    /// Gets or sets the IsBatch
    /// </summary>
    public bool? IsBatch { get; set; }

    /// <summary>
    /// Gets or sets the BatchRef
    /// </summary>
    public string? BatchRef { get; set; }

    /// <summary>
    /// Gets or sets the AuthenType
    /// </summary>
    public string? AuthenType { get; set; }

    /// <summary>
    /// Gets or sets the AuthenCode
    /// </summary>
    public string? AuthenCode { get; set; }
}
