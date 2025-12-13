using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

[DatabaseType(DataProviderType.Oracle)]

public class ORACLE_D_NOTIFICATION_TEMPLATEBuilder : O24OpenAPIEntityBuilder<D_NOTIFICATION_TEMPLATE>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(D_NOTIFICATION_TEMPLATE.TemplateID))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(D_NOTIFICATION_TEMPLATE.Title))
            .AsString(200)
            .NotNullable()
            .WithColumn(nameof(D_NOTIFICATION_TEMPLATE.Body))
            .AsNCLOB()
            .WithColumn(nameof(D_NOTIFICATION_TEMPLATE.LearnApiSending))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(D_NOTIFICATION_TEMPLATE.Data))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(D_NOTIFICATION_TEMPLATE.IsShowButton))
            .AsBoolean()
            .NotNullable();
    }
}
