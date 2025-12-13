using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Web.Framework.Domain;

namespace O24OpenAPI.O24ACT.Migrations;

public class MasterMappingBuilder : O24OpenAPIEntityBuilder<MasterMapping>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(MasterMapping.MasterClass))
            .AsString(255)
            .PrimaryKey()
            .NotNullable()
            .WithColumn(nameof(MasterMapping.GLConfigClass))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MasterMapping.MasterTransClass))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MasterMapping.StatementClass))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MasterMapping.GLEntriesClass))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MasterMapping.MasterFields))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(MasterMapping.MasterBranchCodeField))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MasterMapping.MasterCurrencyCodeField))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MasterMapping.MasterGLClass))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MasterMapping.MasterGLFields))
            .AsString(int.MaxValue)
            .Nullable();
    }
}
