using O24OpenAPI.Core;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Localization;

namespace O24OpenAPI.Web.Framework.Exceptions;

public class O24Exception : Exception
{
    public static async Task<O24OpenAPIException> CreateAsync(
        string resourceCode,
        string lang = "en",
        params object[] values
    )
    {
        lang = lang.Coalesce("en");
        var _localizationService = EngineContext.Current.Resolve<ILocalizationService>();
        var error = await _localizationService.GetByName(resourceCode, lang);

        if (error == null)
        {
            return new O24OpenAPIException(resourceCode, resourceCode);
        }

        var message =
            values.Length > 0 ? string.Format(error.ResourceValue, values) : error.ResourceValue;
        try
        {
            await message.WriteErrorAsync(resourceCode);
        }
        catch { }
        return new O24OpenAPIException(error.ResourceCode, message);
    }

    public static async Task<ExceptionWithNextAction> CreateWithNextActionAsync(
        string resourceName,
        string nextAction,
        string lang = "en",
        params object[] values
    )
    {
        lang = lang.Coalesce("en");
        var _localizationService = EngineContext.Current.Resolve<ILocalizationService>();
        var error = await _localizationService.GetByName(resourceName, lang);

        if (error == null)
        {
            return new ExceptionWithNextAction(resourceName, resourceName, nextAction);
        }
        var message =
            values.Length > 0 ? string.Format(error.ResourceValue, values) : error.ResourceValue;
        try
        {
            await message.WriteErrorAsync(resourceName);
        }
        catch { }
        return new ExceptionWithNextAction(error.ResourceCode, message, nextAction);
    }
}
