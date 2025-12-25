using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Framework.Domain;

public class AuditLog : BaseEntity
{
    public string EntityName { get; set; }
    public int EntityId { get; set; }
    public string UserId { get; set; }
    public string ExecutionId { get; set; }
    public string Action { get; set; } // I, U, D, DR, CR
    public string Changes { get; set; }
}
