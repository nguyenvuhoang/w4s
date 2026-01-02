using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

/// <summary>
/// AccountStatement
/// </summary>
public partial class AccountStatement : BaseStatement
{
    public string TransId { get; set; } = Guid.NewGuid().ToString();
}
