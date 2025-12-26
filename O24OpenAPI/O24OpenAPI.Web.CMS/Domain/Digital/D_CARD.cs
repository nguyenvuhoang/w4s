namespace O24OpenAPI.Web.CMS.Domain;

public partial class D_CARD : BaseEntity
{
    public string CardCode { get; set; }
    public string CardLogo { get; set; }
    public string CardName { get; set; }
    public string CardType { get; set; }
    public decimal CardLimit { get; set; }
    public string CardServiceCode { get; set; }
}
