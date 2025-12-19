using O24OpenAPI.Framework.Localization;

namespace O24OpenAPI.CMS.API.Application.Utils;

/// <summary>
///
/// </summary>
public static class ErrorServices
{
    /// <summary>
    /// Return error by error resource
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="errorName"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static async Task<string> GetError(
        this ILocalizationService localizationService,
        string errorName,
        string[] param = null
    )
    {
        string errorReturn = await localizationService.GetResource(errorName);
        if (param != null && param.Length > 0)
        {
            errorReturn = string.Format(errorReturn, param);
        }

        return errorReturn;
    }

    /// <summary>
    /// Return error by error resource
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="errorName"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static async Task<string> GetError(
        this ILocalizationService localizationService,
        string errorName,
        string param = ""
    )
    {
        string errorReturn = await localizationService.GetResource(errorName);
        if (!string.IsNullOrEmpty(param))
        {
            errorReturn = string.Format(errorReturn, param);
        }

        return errorReturn;
    }
}
