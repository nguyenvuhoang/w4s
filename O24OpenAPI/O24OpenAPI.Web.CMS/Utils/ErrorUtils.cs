namespace O24OpenAPI.Web.CMS.Utils;

public class ErrorUtils
{
    public static async Task<O24OpenAPIException> CreateException(
        string errorCode,
        params object[] values
    )
    {
        var service = EngineContext.Current.Resolve<IRaiseErrorService>();
        return await service.RaiseErrorWithKeyResource(errorCode, values);
    }

    public static async Task<O24OpenAPIException> BuildException(
        string errorCode,
        string errorRegion,
        params object[] values
    )
    {
        var service = EngineContext.Current.Resolve<IRaiseErrorService>();
        return await service.RaiseErrorWithKeyResource($"{errorRegion}.{errorCode}", values);
    }

    public static async Task<O24OpenAPIException> CreateException(
        string errorCode,
        Exception ex = null,
        params object[] values
    )
    {
        var service = EngineContext.Current.Resolve<IRaiseErrorService>();
        return await service.RaiseErrorWithKeyResource(errorCode, ex, values);
    }

    public static async Task<O24OpenAPIException> Required(string fieldName)
    {
        var service = EngineContext.Current.Resolve<IRaiseErrorService>();
        return await service.RequiredArg(fieldName);
    }

    public static string GetErrorName(string errorRegion, string errorName)
    {
        return $"{errorRegion}.{errorName}";
    }
}
