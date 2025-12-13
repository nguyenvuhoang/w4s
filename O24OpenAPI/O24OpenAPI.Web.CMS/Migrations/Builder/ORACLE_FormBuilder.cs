using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

[DatabaseType(DataProviderType.Oracle)]

public class ORACLE_FormBuilder : O24OpenAPIEntityBuilder<Form>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Form.Info))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(Form.ListLayout))
            .AsNCLOB()
            .NotNullable()
            .WithColumn(nameof(Form.FormId))
            .AsString(500)
            .NotNullable()
            .WithColumn(nameof(Form.App))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(Form.MasterData))
            .AsNCLOB()
            .Nullable();
    }
}
