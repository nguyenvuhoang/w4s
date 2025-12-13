using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

public class RoleTypeBuilder : O24OpenAPIEntityBuilder<RoleType>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(RoleType.RoleTypeCode))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(RoleType.RoleTypeName))
            .AsString(200)
            .NotNullable()
            .WithColumn(nameof(RoleType.Description))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(RoleType.SortOrder))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(RoleType.ServiceID))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(RoleType.IsActive))
            .AsBoolean()
            .NotNullable();
    }
}
