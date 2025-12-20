using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Framework.Domain;

public class O24OpenAPIService : BaseEntity
{
    public string StepCode { get; set; }
    public string FullClassName { get; set; }
    public string MethodName { get; set; }
    public string MediatorKey { get; set; } = string.Empty;
    public bool ShouldAwait { get; set; } = false;
    public bool IsInquiry { get; set; } = false;
    public bool IsModuleExecute { get; set; } = false;
}
