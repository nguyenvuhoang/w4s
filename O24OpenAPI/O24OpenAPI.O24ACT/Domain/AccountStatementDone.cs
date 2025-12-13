using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24ACT.Domain;

/// <summary>
/// AccountStatement
/// </summary>
public partial class AccountStatementDone : BaseStatement
{
    public string TransId { get; set; } = Guid.NewGuid().ToString();

}
