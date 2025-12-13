namespace O24OpenAPI.Web.CMS.Models.QR;

public class GenQRRequest : BaseTransactionModel
{
    public string Type { get; set; } = "FO";
    public string Name { get; set; }
    public Dictionary<string, object> Input { get; set; }
}
