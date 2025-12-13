using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.Web.Framework.Domain;

namespace O24OpenAPI.O24NCH.Migrations.DataMigration;

public class DataConditionField
{
    public static readonly List<string> MailTemplateCondition =
    [
        nameof(MailTemplate.TemplateId)
    ];
    public static readonly List<string> O24OpenAPIServiceCondition =
    [
        nameof(O24OpenAPIService.StepCode)
    ];
}
