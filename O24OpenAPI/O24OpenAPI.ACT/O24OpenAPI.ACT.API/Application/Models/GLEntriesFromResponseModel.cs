using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.ACT.API.Application.Models;

public class GLEntriesFromResponseModel : BaseO24OpenAPIModel
{
    public int AccountingGroup { get; set; }
    public string CurrencyCode { get; set; }
    public string BranchCode { get; set; }
    public decimal Amount { get; set; }
    public string DorC { get; set; }
    public string Condition { get; set; }
    public string GLAccount { get; set; }
}
