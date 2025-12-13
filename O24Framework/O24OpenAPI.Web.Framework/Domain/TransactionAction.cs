using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Framework.Domain;

public class TransactionAction : BaseEntity
{
    public string TransCode { get; set; }

    public string FieldNames { get; set; }

    public string Action { get; set; }

    public bool HasStatement { get; set; }
}
