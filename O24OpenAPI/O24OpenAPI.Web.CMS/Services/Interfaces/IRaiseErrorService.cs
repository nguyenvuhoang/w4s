namespace O24OpenAPI.Web.CMS.Services;

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
