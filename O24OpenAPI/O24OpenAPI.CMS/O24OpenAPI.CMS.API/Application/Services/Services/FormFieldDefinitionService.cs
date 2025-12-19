using LinqToDB;
using Newtonsoft.Json;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

public class FormFieldDefinitionService(IRepository<FormFieldDefinition> formFieldRepo)
    : IFormFieldDefinitionService
{
    private readonly IRepository<FormFieldDefinition> _formFieldRepo = formFieldRepo;

    public async Task<string> GetFieldValueAsync(string language, string formId, string fieldName)
    {
        var fieldJson = await _formFieldRepo
            .Table.Where(x => x.FormId == formId && x.FieldName == fieldName)
            .Select(x => x.FieldValue)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(fieldJson))
        {
            return string.Empty;
        }

        try
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(fieldJson);
            if (dict != null && dict.TryGetValue(language, out var value))
            {
                return value ?? string.Empty;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return string.Empty;
    }
}
