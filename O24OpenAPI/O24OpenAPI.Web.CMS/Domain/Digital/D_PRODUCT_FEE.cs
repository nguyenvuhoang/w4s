namespace O24OpenAPI.Web.CMS.Domain;

public partial class D_PRODUCT_FEE : BaseEntity
{
    public string ProductID { get; set; }
    public string TranCode { get; set; }
    public string FeeID { get; set; }
    public decimal ContractLevelId { get; set; }
    public string CCYID { get; set; }
    public string Payer { get; set; }
    public string Islocal { get; set; }
    public string Description { get; set; }
    public string UserCreate { get; set; }
    public DateTime? CreateDated { get; set; }
    public string UserModified { get; set; }
    public DateTime? LastModified { get; set; }
    public string UserApprove { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string FeeShareId { get; set; }
    public string TransferFeeID { get; set; }
}
