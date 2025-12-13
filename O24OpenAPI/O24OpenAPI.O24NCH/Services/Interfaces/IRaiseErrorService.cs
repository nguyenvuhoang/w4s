using O24OpenAPI.Core;

namespace O24OpenAPI.O24NCH.Services;

/// <summary>
/// The raise error service interface
/// </summary>
public interface IRaiseErrorService
{
    Task<O24OpenAPIException> RaiseErrorWithKeyResource(
        string keyError,
        params object[] values
    );

    Task<O24OpenAPIException> RaiseErrorWithKeyResource(
        string keyError,
        Exception ex = null,
        params object[] values
    );

    Task<O24OpenAPIException> RequiredArg(string fieldName);
}
