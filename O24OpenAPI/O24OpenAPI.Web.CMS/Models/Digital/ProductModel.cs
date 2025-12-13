namespace O24OpenAPI.Web.CMS.Models.Digital;

public class ProductModel : BaseTransactionModel
{
    public string ProductID { get; set; }
    public string ProductName { get; set; }
    public string CustType { get; set; }
    public string Description { get; set; }
    public string ProductType { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string GRP_ID { get; set; }
    public string Status { get; set; }
    public string UserCreated { get; set; }
    public DateTime? DateCreated { get; set; }
    public string UserModified { get; set; }
    public DateTime? LastModified { get; set; }
    public string UserApproved { get; set; }
    public DateTime? DateApproved { get; set; }
}

public class DeleteProductModel : BaseTransactionModel
{
    public string ProductID { get; set; }
    public List<string> ListProductID { get; set; } = new List<string>();
}
