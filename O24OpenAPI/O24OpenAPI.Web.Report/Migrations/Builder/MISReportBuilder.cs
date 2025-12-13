using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Web.Report.Domain.MIS;

namespace O24OpenAPI.Web.Report.Migrations.Builder;

public partial class MISReportBuilder : O24OpenAPIEntityBuilder<MISReport>
{
    /// <summary>
    /// Map entity MISReport table
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(MISReport.ReportCode))
            .AsString(50)
            .WithColumn(nameof(MISReport.Item))
            .AsString(100)
            .WithColumn(nameof(MISReport.Order))
            .AsInt32()
            .WithColumn(nameof(MISReport.ValueBasis))
            .AsString(100)
            .WithColumn(nameof(MISReport.Condition))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MISReport.DataSource))
            .AsString(100)
            .WithColumn(nameof(MISReport.Group))
            .AsString(50)
            .Nullable();
    }
}
