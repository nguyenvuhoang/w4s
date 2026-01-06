namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Defines the <see cref="WalletTransaction" />
/// </summary>
/// 
[Auditable]
public partial class WalletTransaction : BaseEntity
{
    /// <summary>
    /// Gets or sets the TransactionId
    /// </summary>
    public string? TRANSACTIONID { get; set; }

    /// <summary>
    /// Gets or sets the TransactionDate
    /// </summary>
    public DateTime TRANSACTIONDATE { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the TransactionEndDate
    /// </summary>
    public DateTime? TRANSACTIONENDDATE { get; set; }

    /// <summary>
    /// Gets or sets the TransactionWorkDate
    /// </summary>
    public DateTime? TRANSACTIONWORKDATE { get; set; }

    /// <summary>
    /// Gets or sets the TransactionName
    /// </summary>
    public string TRANSACTIONNAME { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the TransactionCode
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string? TRANSACTIONCODE { get; set; }

    /// <summary>
    /// Gets or sets the CcyId
    /// </summary>
    [MaxLength(10)]
    public string? CCYID { get; set; }

    /// <summary>
    /// Gets or sets the SourceId
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string? SOURCEID { get; set; }

    /// <summary>
    /// Gets or sets the SourceTranRef
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string? SOURCETRANREF { get; set; }

    /// <summary>
    /// Gets or sets the UserId
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string? USERID { get; set; }

    /// <summary>
    /// Gets or sets the UserCurApp
    /// </summary>
    [MaxLength(2000)]
    public string? USERCURAPP { get; set; }

    /// <summary>
    /// Gets or sets the NextUserApp
    /// </summary>
    [MaxLength(2000)]
    public string? NEXTUSERAPP { get; set; }

    /// <summary>
    /// Gets or sets the ListUserApp
    /// </summary>
    public string? LISTUSERAPP { get; set; }

    /// <summary>
    /// Gets or sets the TranDesc
    /// </summary>
    [Required]
    [MaxLength(400)]
    public string? TRANDESC { get; set; }

    /// <summary>
    /// Gets or sets the Status
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string? STATUS { get; set; }

    /// <summary>
    /// Gets or sets the ApprSts
    /// </summary>
    public int APPRSTS { get; set; }

    /// <summary>
    /// Gets or sets the OfflSts
    /// </summary>
    [Required]
    [MaxLength(1)]
    public string? OFFLSTS { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Deleted
    /// </summary>
    public bool DELETED { get; set; }

    /// <summary>
    /// Gets or sets the DestId
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string? DESTID { get; set; }

    /// <summary>
    /// Gets or sets the DestTranRef
    /// </summary>
    [MaxLength(20)]
    public string? DESTTRANREF { get; set; }

    /// <summary>
    /// Gets or sets the DestErrorCode
    /// </summary>
    [MaxLength(50)]
    public string? DESTERRORCODE { get; set; }

    /// <summary>
    /// Gets or sets the ErrorCode
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string ERRORCODE { get; set; } = "";

    /// <summary>
    /// Gets or sets the ErrorDesc
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string ERRORDESC { get; set; } = "";

    /// <summary>
    /// Gets or sets a value indicating whether Online
    /// </summary>
    public bool ONLINE { get; set; } = true;

    /// <summary>
    /// Gets or sets the Char01
    /// </summary>
    public string? CHAR01 { get; set; }

    /// <summary>
    /// Gets or sets the Char02
    /// </summary>
    public string? CHAR02 { get; set; }

    /// <summary>
    /// Gets or sets the Char03
    /// </summary>
    public string? CHAR03 { get; set; }

    /// <summary>
    /// Gets or sets the Char04
    /// </summary>
    public string? CHAR04 { get; set; }

    /// <summary>
    /// Gets or sets the Char05
    /// </summary>
    public string? CHAR05 { get; set; }

    /// <summary>
    /// Gets or sets the Char06
    /// </summary>
    public string? CHAR06 { get; set; }

    /// <summary>
    /// Gets or sets the Char07
    /// </summary>
    public string? CHAR07 { get; set; }

    /// <summary>
    /// Gets or sets the Char08
    /// </summary>
    public string? CHAR08 { get; set; }

    /// <summary>
    /// Gets or sets the Char09
    /// </summary>
    public string? CHAR09 { get; set; }

    /// <summary>
    /// Gets or sets the Char10
    /// </summary>
    public string? CHAR10 { get; set; }

    /// <summary>
    /// <summary>
    /// Gets or sets the Char11
    /// </summary>
    public string? CHAR11 { get; set; }

    /// <summary>
    /// Gets or sets the Char12
    /// </summary>
    public string? CHAR12 { get; set; }

    /// <summary>
    /// Gets or sets the Char13
    /// </summary>
    public string? CHAR13 { get; set; }

    /// <summary>
    /// Gets or sets the Char14
    /// </summary>
    public string? CHAR14 { get; set; }

    /// <summary>
    /// Gets or sets the Char15
    /// </summary>
    public string? CHAR15 { get; set; }

    /// <summary>
    /// Gets or sets the Char16
    /// </summary>
    public string? CHAR16 { get; set; }

    /// <summary>
    /// Gets or sets the Char17
    /// </summary>
    public string? CHAR17 { get; set; }

    /// <summary>
    /// Gets or sets the Char20                                                                                                                                       
    /// <summary>
    /// Gets or sets the Char18
    /// </summary>
    public string? CHAR18 { get; set; }

    /// <summary>
    /// Gets or sets the Char19
    /// </summary>
    public string? CHAR19 { get; set; }

    /// </summary>
    public string? CHAR20 { get; set; }

    /// <summary>
    /// Gets or sets the Char21
    /// </summary>
    public string? CHAR21 { get; set; }

    /// <summary>
    /// Gets or sets the Char22
    /// </summary>
    public string? CHAR22 { get; set; }

    /// <summary>
    /// Gets or sets the Char23
    /// </summary>
    public string? CHAR23 { get; set; }

    /// <summary>
    /// Gets or sets the Char24
    /// </summary>
    public string? CHAR24 { get; set; }

    /// <summary>
    /// Gets or sets the Char25
    /// </summary>
    public string? CHAR25 { get; set; }

    /// <summary>
    /// <summary>
    /// Gets or sets the Char26
    /// </summary>
    public string? CHAR26 { get; set; }

    /// <summary>
    /// Gets or sets the Char27
    /// </summary>
    public string? CHAR27 { get; set; }

    /// <summary>
    /// Gets or sets the Char28
    /// </summary>
    public string? CHAR28 { get; set; }

    /// <summary>
    /// Gets or sets the Char29
    /// </summary>
    public string? CHAR29 { get; set; }

    /// <summary>
    /// Gets or sets the Char30
    /// </summary>
    public string? CHAR30 { get; set; }

    /// <summary>
    /// Gets or sets the Num01
    /// </summary>
    public decimal? NUM01 { get; set; }

    /// <summary>
    /// Gets or sets the Num02
    /// </summary>
    public decimal? NUM02 { get; set; }

    /// <summary>
    /// Gets or sets the Num03
    /// </summary>
    public decimal? NUM03 { get; set; }

    /// <summary>
    /// Gets or sets the Num04
    /// </summary>
    public decimal? NUM04 { get; set; }

    /// <summary>
    /// Gets or sets the Num05
    /// </summary>
    public decimal? NUM05 { get; set; }

    /// <summary>
    /// Gets or sets the Num06
    /// </summary>
    public decimal? NUM06 { get; set; }

    /// <summary>
    /// Gets or sets the Num07
    /// </summary>
    public decimal? NUM07 { get; set; }

    /// <summary>
    /// Gets or sets the Num08
    /// </summary>
    public decimal? NUM08 { get; set; }

    /// <summary>
    /// Gets or sets the Num09
    /// </summary>
    public decimal? NUM09 { get; set; }

    /// <summary>
    /// Gets or sets the Num10
    /// </summary>
    public decimal? NUM10 { get; set; }

    /// <summary>
    /// Gets or sets the Num11
    /// </summary>
    public decimal? NUM11 { get; set; }

    /// <summary>
    /// Gets or sets the Num12
    /// </summary>
    public decimal? NUM12 { get; set; }

    /// <summary>
    /// Gets or sets the Num13
    /// </summary>
    public decimal? NUM13 { get; set; }

    /// <summary>
    /// Gets or sets the Num14
    /// </summary>
    public decimal? NUM14 { get; set; }

    /// <summary>
    /// Gets or sets the Num15
    /// </summary>
    public decimal? NUM15 { get; set; }

    /// <summary>
    /// Gets or sets the Num16
    /// </summary>
    public decimal? NUM16 { get; set; }

    /// <summary>
    /// Gets or sets the Num17
    /// </summary>
    public decimal? NUM17 { get; set; }

    /// <summary>
    /// Gets or sets the Num18
    /// </summary>
    public decimal? NUM18 { get; set; }

    /// <summary>
    /// Gets or sets the Num19
    /// </summary>
    public decimal? NUM19 { get; set; }

    /// <summary>
    /// Gets or sets the Num20
    /// </summary>
    public decimal? NUM20 { get; set; }

    /// <summary>
    /// Gets or sets the IsBatch
    /// </summary>
    public bool? ISBATCH { get; set; }

    /// <summary>
    /// Gets or sets the BatchRef
    /// </summary>
    public string? BATCHREF { get; set; }

    /// <summary>
    /// Gets or sets the AuthenType
    /// </summary>
    public string? AUTHENTYPE { get; set; }

    /// <summary>
    /// Gets or sets the AuthenCode
    /// </summary>
    public string? AUTHENCODE { get; set; }
}
