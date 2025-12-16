using System.ComponentModel.DataAnnotations;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Web.CMS.Models.ContextModels;

namespace O24OpenAPI.Web.CMS.Services;

/// <summary>
/// The raise error service class
/// </summary>
public class RaiseErrorService : IRaiseErrorService
{
    /// <summary>
    /// The web ui object context model
    /// </summary>
    private readonly JWebUIObjectContextModel _context;

    /// <summary>
    /// Raises the error with key resource using the specified key error
    /// </summary>
    /// <param name="keyError">The key error</param>
    /// <param name="values">The values</param>
    /// <returns>A task containing the 24 open api exception</returns>
    private readonly ILocalizationService _localizationService;

    public RaiseErrorService(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
        _context = EngineContext.Current.Resolve<JWebUIObjectContextModel>();
    }

    public async Task<O24OpenAPIException> RaiseErrorWithKeyResource(
        string keyError,
        params object[] values
    )
    {
        var error = await _localizationService.GetByName(
            keyError,
            _context.InfoUser.GetUserLogin().Lang
        );

        if (error == null)
        {
            return new O24OpenAPIException(keyError);
        }

        return values.Length > 0
            ? new O24OpenAPIException(
                error.ResourceCode,
                string.Format(error.ResourceValue, values)
            )
            : new O24OpenAPIException(error.ResourceCode, error.ResourceValue);
    }

    public async Task<O24OpenAPIException> RaiseErrorWithKeyResource(
        string keyError,
        Exception ex = null,
        params object[] values
    )
    {
        var error = await _localizationService.GetByName(
            keyError,
            _context.InfoUser.GetUserLogin().Lang
        );
        if (error == null)
        {
            return new O24OpenAPIException(ex.Message, values);
        }

        return values.Length > 0
            ? new O24OpenAPIException(
                error.ResourceCode,
                string.Format(error.ResourceValue, values)
            )
            : new O24OpenAPIException(error.ResourceCode, error.ResourceValue);
    }

    public async Task<O24OpenAPIException> RequiredArg(string fieldName)
    {
        string errorCode = "Common.Value.Required";
        var error = await _localizationService.GetByName(
            errorCode,
            _context.InfoUser.GetUserLogin().Lang
        );
        if (error == null)
        {
            error = await _localizationService.GetByName(errorCode, "en");
        }
        return new O24OpenAPIException(
            error.ResourceCode,
            string.Format(error.ResourceValue, fieldName)
        );
    }
}

public static class ErrorExtension
{
    public static async Task<O24OpenAPIException> Required(this string fieldName)
    {
        var service = EngineContext.Current.Resolve<IRaiseErrorService>();
        return await service.RequiredArg(fieldName);
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class O24RequiredAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            try
            {
                var service = EngineContext.Current.Resolve<IRaiseErrorService>();
                var exception = service
                    .RequiredArg(validationContext.DisplayName)
                    .GetAwaiter()
                    .GetResult();
                return new ValidationResult(exception.Message);
            }
            catch
            {
                return new ValidationResult(
                    $"The {validationContext.DisplayName} field is required."
                );
            }
        }
        return ValidationResult.Success;
    }
}
