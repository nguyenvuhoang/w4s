using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models.Request;

public class AccountChartDefaultModel : BaseTransactionModel
{
    public string AccountNumber { get; set; }
}
