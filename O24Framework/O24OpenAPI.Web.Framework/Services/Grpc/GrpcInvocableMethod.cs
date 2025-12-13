namespace O24OpenAPI.Web.Framework.Services.Grpc;

/// <summary>
///
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class GrpcInvocableMethod : Attribute
{
    /// <summary>
    ///
    /// </summary>
    public string MethodDescription { get; set; }
}
