using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Framework.Domain;

public class BaseMasterGL : BaseEntity
{
    public string SysAccountName { get; set; }
    public string GLAccount { get; set; }
}
