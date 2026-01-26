using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.Kit.Models;

/// <summary>
/// BO Request Model
/// </summary>
public class SignatureModel : BaseO24OpenAPIModel
{
    public string OperationName { get; set; } = string.Empty;
    public string RequestData { get; set; } = string.Empty;
}
