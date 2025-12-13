namespace O24OpenAPI.Web.CMS.Domain;

public class D_SAVING_PRODUCT : BaseEntity
{
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public string Description { get; set; }
    public string UserCode { get; set; }
    public bool IsAllowRegister { get; set; }

    public string ProductUrl { get; set; }

}
