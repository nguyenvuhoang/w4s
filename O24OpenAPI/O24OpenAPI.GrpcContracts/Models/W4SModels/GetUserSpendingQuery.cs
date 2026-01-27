using LinKit.Core.Cqrs;

namespace O24OpenAPI.GrpcContracts.Models.W4SModels;

public class GetUserSpendingQuery(string? userCode, DateTime fromDate, DateTime toDate)
    : IQuery<object>
{
    public string? UserCode { get; set; } = userCode;
    public DateTime FromDate { get; set; } = fromDate;
    public DateTime ToDate { get; set; } = toDate;
}
