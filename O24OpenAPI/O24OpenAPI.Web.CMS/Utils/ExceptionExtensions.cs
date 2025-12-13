using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Utils;

public static class ExceptionExtensions
{
    /// <summary>
    ///
    /// </summary>
    public static JToken HandleException(
        this Exception ex,
        string reference = "",
        string reference_id = ""
    )
    {
        // var _logger = EngineContext.Current.Resolve<ILoggerService>();
        Console.WriteLine(ex.StackTrace);
        // _logger.LogError(ex, reference, reference_id).GetAwaiter().GetResult();
        return ex.Message.BuildWorkflowResponseError();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="reference"></param>
    /// <param name="reference_id"></param>
    /// <returns></returns>
    public static async Task<JToken> HandleExceptionAsync(
        this Exception ex,
        string reference = "",
        string reference_id = ""
    )
    {
        await Task.CompletedTask;
        // var _logger = EngineContext.Current.Resolve<ILoggerService>();
        // await _logger.LogError(ex, reference, reference_id);
        return ex.Message.BuildWorkflowResponseError();
    }
}
