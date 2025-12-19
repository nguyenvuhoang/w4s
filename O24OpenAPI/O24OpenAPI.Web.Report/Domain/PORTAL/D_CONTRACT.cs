using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Report.Domain.PORTAL;

public class D_CONTRACT : BaseEntity
{
    public string ContractNumber { get; set; }
    public string ContractCode { get; set; }
    public string CustomerCode { get; set; }
    public string ContractType { get; set; }
    public string UserType { get; set; }
    public string ProductID { get; set; }
    public string BranchID { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? LastModify { get; set; }
    public string UserCreate { get; set; }
    public string UserLastModify { get; set; }
    public string UserApprove { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string Status { get; set; }
    public string IsSpecialMan { get; set; }
    public string IsReceiverList { get; set; }
    public string IsAutoRenew { get; set; }
    public string Description { get; set; }
    public decimal ContractLevelId { get; set; }
    public string Mer_Code { get; set; }
    public string ShopName { get; set; }
    public string LocalShopName { get; set; }
    public string ParentContract { get; set; }
    public string ControlType { get; set; }
    public string TransactionID { get; set; }
}
