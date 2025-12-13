using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Framework.Domain;

/// <summary>
/// The 24 open api service class
/// </summary>
/// <seealso cref="BaseEntity"/>
public class O24OpenAPIService : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the step code
    /// </summary>
    public string StepCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the full class name
    /// </summary>
    public string FullClassName { get; set; }

    /// <summary>
    /// Gets or sets the value of the method name
    /// </summary>
    public string MethodName { get; set; }

    /// <summary>
    /// Gets or sets the value of the should await
    /// </summary>
    public bool ShouldAwait { get; set; } = false;

    /// <summary>
    /// Gets or sets the value of the is inquiry
    /// </summary>
    public bool IsInquiry { get; set; } = false;
    public bool IsModuleExecute { get; set; } = false;
}
