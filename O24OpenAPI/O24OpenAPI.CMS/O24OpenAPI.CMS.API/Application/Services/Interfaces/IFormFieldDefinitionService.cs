namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public interface IFormFieldDefinitionService
{
    /// <summary>
    /// Get localized value of a field by language, form, and field name.
    /// </summary>
    /// <param name="language">Language code, e.g., "en", "vi"</param>
    /// <param name="formId">Form identifier</param>
    /// <param name="fieldName">Field name (i18n code like agent.view.name)</param>
    /// <returns>Localized string if found, otherwise empty string</returns>
    Task<string> GetFieldValueAsync(string language, string formId, string fieldName);
}
