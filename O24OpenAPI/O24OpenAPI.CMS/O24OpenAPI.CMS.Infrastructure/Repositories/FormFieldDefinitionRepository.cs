using LinKit.Core.Abstractions;
using LinqToDB;
using Newtonsoft.Json;
using O24OpenAPI.CMS.Domain.AggregateModels.FormAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

namespace O24OpenAPI.CMS.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class FormFieldDefinitionRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<FormFieldDefinition>(dataProvider, staticCacheManager),
        IFormFieldDefinitionRepository
{
    public async Task<string> GetFieldValueAsync(string language, string formId, string fieldName)
    {
        string? fieldJson = await Table
            .Where(x => x.FormId == formId && x.FieldName == fieldName)
            .Select(x => x.FieldValue)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(fieldJson))
        {
            return string.Empty;
        }

        try
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(fieldJson);
            if (dict != null && dict.TryGetValue(language, out string? value))
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
