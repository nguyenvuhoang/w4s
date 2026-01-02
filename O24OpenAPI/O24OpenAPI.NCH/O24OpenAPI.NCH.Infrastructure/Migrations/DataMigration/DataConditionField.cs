using O24OpenAPI.Framework.Domain;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.Migrations.DataMigration;

public class DataConditionField
{
    public static readonly List<string> MailTemplateCondition = [nameof(MailTemplate.TemplateId)];
    public static readonly List<string> O24OpenAPIServiceCondition =
    [
        nameof(O24OpenAPIService.StepCode),
    ];
}
