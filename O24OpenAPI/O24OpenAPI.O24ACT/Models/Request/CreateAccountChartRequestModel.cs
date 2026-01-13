using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models.Request;

public class CreateAccountChartRequestModel : BaseTransactionModel
{
    public string AccountCategories { get; set; }
    public string AccountClassification { get; set; }
    public string AccountGroup { get; set; }
    public string AccountName { get; set; }
    public string AccountNumber { get; set; }
    public string BalanceSide { get; set; }
    public string BranchCode { get; set; }
    public string CurrencyCode { get; set; }
    public string DirectPosting { get; set; }
    public string IsVisible { get; set; }
    public string JobProcessOption { get; set; }
    public string LaosName { get; set; }
    public string PostingSide { get; set; }
    public string ReverseBalance { get; set; }
    public string ShortAccountName { get; set; }
    public int? AccountLevel { get; set; }
}
