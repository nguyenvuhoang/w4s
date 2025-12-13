using O24OpenAPI.Core;

namespace O24OpenAPI.O24NCH.Services.Services;

public class RaiseErrorService : IRaiseErrorService
{
    public Task<O24OpenAPIException> RaiseErrorWithKeyResource(string keyError, params object[] values)
    {
        throw new NotImplementedException();
    }

    public Task<O24OpenAPIException> RaiseErrorWithKeyResource(string keyError, Exception ex = null, params object[] values)
    {
        throw new NotImplementedException();
    }

    public Task<O24OpenAPIException> RequiredArg(string fieldName)
    {
        throw new NotImplementedException();
    }
}
