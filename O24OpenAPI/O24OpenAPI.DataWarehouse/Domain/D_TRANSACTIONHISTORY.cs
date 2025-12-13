using O24OpenAPI.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace O24OpenAPI.DataWarehouse.Domain;

public class D_TRANSACTIONHISTORY : BaseEntity
{
    public string TRANSACTIONID { get; set; }

    public DateTime TRANSACTIONDATE { get; set; } = DateTime.Now;
    public DateTime? TRANSACTIONENDDATE { get; set; }
    public DateTime? TRANSACTIONWORKDATE { get; set; }
    public string TRANSACTIONNAME { get; set; } = string.Empty;
    [Required]
    [MaxLength(20)]
    public string TRANSACTIONCODE { get; set; }

    [MaxLength(10)]
    public string CCYID { get; set; }

    [Required]
    [MaxLength(20)]
    public string SOURCEID { get; set; }

    [Required]
    [MaxLength(20)]
    public string SOURCETRANREF { get; set; }

    [Required]
    [MaxLength(50)]
    public string USERID { get; set; }

    [MaxLength(2000)]
    public string USERCURAPP { get; set; }

    [MaxLength(2000)]
    public string NEXTUSERAPP { get; set; }

    public string LISTUSERAPP { get; set; }

    [Required]
    [MaxLength(400)]
    public string TRANDESC { get; set; }

    [Required]
    [MaxLength(10)]
    public string STATUS { get; set; }

    public int APPRSTS { get; set; }

    [Required]
    [MaxLength(1)]
    public string OFFLSTS { get; set; }

    public bool DELETED { get; set; }

    [Required]
    [MaxLength(20)]
    public string DESTID { get; set; }

    [MaxLength(20)]
    public string DESTTRANREF { get; set; }

    [MaxLength(50)]
    public string DESTERRORCODE { get; set; }

    [Required]
    [MaxLength(10)]
    public string ERRORCODE { get; set; } = "";

    [Required]
    [MaxLength(1000)]
    public string ERRORDESC { get; set; } = "";

    public bool ONLINE { get; set; } = true;

    // CHAR01 -> CHAR30
    public string CHAR01 { get; set; }
    public string CHAR02 { get; set; }
    public string CHAR03 { get; set; }
    public string CHAR04 { get; set; }
    public string CHAR05 { get; set; }
    public string CHAR06 { get; set; }
    public string CHAR07 { get; set; }
    public string CHAR08 { get; set; }
    public string CHAR09 { get; set; }
    public string CHAR10 { get; set; }
    public string CHAR11 { get; set; }
    public string CHAR12 { get; set; }
    public string CHAR13 { get; set; }
    public string CHAR14 { get; set; }
    public string CHAR15 { get; set; }
    public string CHAR16 { get; set; }
    public string CHAR17 { get; set; }
    public string CHAR18 { get; set; }
    public string CHAR19 { get; set; }
    public string CHAR20 { get; set; }
    public string CHAR21 { get; set; }
    public string CHAR22 { get; set; }
    public string CHAR23 { get; set; }
    public string CHAR24 { get; set; }
    public string CHAR25 { get; set; }
    public string CHAR26 { get; set; }
    public string CHAR27 { get; set; }
    public string CHAR28 { get; set; }
    public string CHAR29 { get; set; }
    public string CHAR30 { get; set; }

    // NUM01 -> NUM20
    public decimal? NUM01 { get; set; }
    public decimal? NUM02 { get; set; }
    public decimal? NUM03 { get; set; }
    public decimal? NUM04 { get; set; }
    public decimal? NUM05 { get; set; }
    public decimal? NUM06 { get; set; }
    public decimal? NUM07 { get; set; }
    public decimal? NUM08 { get; set; }
    public decimal? NUM09 { get; set; }
    public decimal? NUM10 { get; set; }
    public decimal? NUM11 { get; set; }
    public decimal? NUM12 { get; set; }
    public decimal? NUM13 { get; set; }
    public decimal? NUM14 { get; set; }
    public decimal? NUM15 { get; set; }
    public decimal? NUM16 { get; set; }
    public decimal? NUM17 { get; set; }
    public decimal? NUM18 { get; set; }
    public decimal? NUM19 { get; set; }
    public decimal? NUM20 { get; set; }

    [MaxLength(1)]
    public string ISBATCH { get; set; }

    [MaxLength(50)]
    public string BATCHREF { get; set; }

    [MaxLength(50)]
    public string AUTHENTYPE { get; set; }

    [MaxLength(50)]
    public string AUTHENCODE { get; set; }
}
