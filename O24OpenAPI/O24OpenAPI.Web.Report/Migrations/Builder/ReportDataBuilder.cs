using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Web.Report.Domain;

namespace O24OpenAPI.Web.Report.Migrations.Builder;

public class ReportDataBuilder : O24OpenAPIEntityBuilder<ReportData>
{
    /// <summary>
    /// map entity ReportData table
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ReportData.ReportCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(ReportData.DataSourceName))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(ReportData.DataSource))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(ReportData.DataBand))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(ReportData.ParentDatatable))
            .AsString(100)
            .Nullable();
    }
}
