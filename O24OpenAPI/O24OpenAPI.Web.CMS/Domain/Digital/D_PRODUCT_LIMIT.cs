namespace O24OpenAPI.Web.CMS.Domain;

public partial class D_PRODUCT_LIMIT : BaseEntity
{
    public string ProductID { get; set; }
    public string TranCode { get; set; }
    public string CCYID { get; set; }
    public string BranchID { get; set; }
    public string LimitType { get; set; }
    public decimal ContractLevelId { get; set; }
    public decimal TranLimit { get; set; }
    public int CountLimit { get; set; }
    public decimal TotalLimit { get; set; }
    public string UnitType { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public string UserCreated { get; set; }
    public DateTime? DateCreated { get; set; }
    public string UserModified { get; set; }
    public DateTime? LastModified { get; set; }
    public string UserApproved { get; set; }
    public DateTime? DateApproved { get; set; }
    public decimal BiometricLimit { get; set; }
}
