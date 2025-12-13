namespace O24OpenAPI.Web.CMS.Domain;

public class D_CARD_USER : BaseEntity
{
    public string UserCode { get; set; }
    public string CardCode { get; set; }
    public string CardNumber { get; set; }
    public string CardHolderName { get; set; }
    public decimal CardLimit { get; set; }
    public decimal AvailableLimit { get; set; }
    public decimal Balance { get; set; }
    public string Status { get; set; }
    public bool IsPrimary { get; set; }
    public DateTime CardExpiryDate { get; set; }
    public string LinkagedAccount { get; set; }
}
