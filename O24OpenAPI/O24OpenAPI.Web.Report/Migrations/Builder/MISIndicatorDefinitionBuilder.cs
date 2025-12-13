using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Web.Report.Domain.MIS;

namespace O24OpenAPI.Web.Report.Migrations.Builder;

public partial class MISIndicatorDefinitionBuilder : O24OpenAPIEntityBuilder<MISIndicatorDefinition>
{
    /// <summary>
    /// Map entity MISIndicatorDefinition table
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(MISIndicatorDefinition.ReportCode))
            .AsString(50)
            .WithColumn(nameof(MISIndicatorDefinition.ColumnName))
            .AsString(100)
            .WithColumn(nameof(MISIndicatorDefinition.Condition))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MISIndicatorDefinition.Formula))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MISIndicatorDefinition.DataSource))
            .AsString(100)
            .WithColumn(nameof(MISIndicatorDefinition.Group))
            .AsString(50)
            .Nullable();
    }
}
