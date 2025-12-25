using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Framework.Domain;

namespace O24OpenAPI.Framework.Migrations.Builder;

public class AuditLogBuilder : O24OpenAPIEntityBuilder<AuditLog>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn("EntityName").AsString(255).NotNullable();
        table.WithColumn("EntityId").AsInt64().NotNullable();
        table.WithColumn("UserId").AsString(255).NotNullable();
        table.WithColumn("ExecutionId").AsString(255).NotNullable();
        table.WithColumn("Action").AsString(100).NotNullable();
        table.WithColumn("Changes").AsString().Nullable();
    }
}
