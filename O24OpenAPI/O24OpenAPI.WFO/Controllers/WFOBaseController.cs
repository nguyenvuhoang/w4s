using O24OpenAPI.Client.Workflow;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.WFO.Controllers;

public class WFOBaseController : BaseController
{
    protected async Task<TResult> InvokeAsync<TResult>(Func<Task<WorkflowResponse>> controllerCall)
    {
        try
        {
            var response = await controllerCall();

            if (response == null)
            {
                response.error_code = "ERR_NULL_RESPONSE";
                response.error_message = "Response controllerCall is null";
            }

            if (typeof(TResult) == typeof(string))
            {
                return (TResult)(object)response.data;
            }

            return (TResult)(object)response;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            var result = new WorkflowResponse
            {
                error_code = "ERR_UNKNOWN",
                error_message = ex.Message,
            };
            return (TResult)(object)result;
        }
    }
}
