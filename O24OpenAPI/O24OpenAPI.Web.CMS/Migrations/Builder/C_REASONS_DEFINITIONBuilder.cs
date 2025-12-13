using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

public class C_REASONS_DEFINITIONBuilder : O24OpenAPIEntityBuilder<C_REASONS_DEFINITION>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(C_REASONS_DEFINITION.ReasonID))
            .AsDecimal(20, 0)
            .NotNullable()
            .WithColumn(nameof(C_REASONS_DEFINITION.ReasonCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(C_REASONS_DEFINITION.ReasonName))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(C_REASONS_DEFINITION.Description))
            .AsString(250)
            .Nullable()
            .WithColumn(nameof(C_REASONS_DEFINITION.ReasonAction))
            .AsString(3)
            .NotNullable()
            .WithColumn(nameof(C_REASONS_DEFINITION.ReasonType))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(C_REASONS_DEFINITION.ReasonEvent))
            .AsString(3)
            .NotNullable()
            .WithColumn(nameof(C_REASONS_DEFINITION.Status))
            .AsString(3)
            .NotNullable();
    }
}
