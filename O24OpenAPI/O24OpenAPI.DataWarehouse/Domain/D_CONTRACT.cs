using O24OpenAPI.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace O24OpenAPI.DataWarehouse.Domain;

public class D_CONTRACT : BaseEntity
{
    [Required]
    public string ContractNumber { get; set; }
    public string ContractCode { get; set; }
    [Required]
    public string CustomerCode { get; set; }
    public string ContractType { get; set; }
    [Required]
    public string UserType { get; set; }
    public string ProductID { get; set; }
    [Required]
    public string BranchID { get; set; }
    [Required]
    public DateTime CreateDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    public DateTime? LastModify { get; set; }
    [Required]
    public string UserCreate { get; set; }
    public string UserLastModify { get; set; }
    [Required]
    public string UserApprove { get; set; }
    public DateTime? ApproveDate { get; set; }
    [Required]
    public string Status { get; set; }
    public string IsSpecialMan { get; set; }
    public string IsReceiverList { get; set; }
    public string IsAutoRenew { get; set; }
    public string Description { get; set; }
    [Required]
    public int ContractLevelId { get; set; }
    public string Mer_Code { get; set; }
    public string ShopName { get; set; }
    public string LocalShopName { get; set; }
    public string ParentContract { get; set; }
    public string ControlType { get; set; }
    public string TransactionID { get; set; }
    public int PolicyId { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public DateTime? CreatedOnUtc { get; set; } = DateTime.UtcNow;
}
