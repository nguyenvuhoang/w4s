using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CMS.Domain.AggregateModels.FormAggregate;

public interface IFormFieldDefinitionRepository : IRepository<FormFieldDefinition>
{
    Task<string> GetFieldValueAsync(string language, string formId, string fieldName);
}
