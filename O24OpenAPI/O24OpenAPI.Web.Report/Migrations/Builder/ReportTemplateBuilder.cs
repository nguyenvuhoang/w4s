using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Web.Report.Domain;

namespace O24OpenAPI.Web.Report.Migrations.Builder;

public partial class ReportTemplateBuilder : O24OpenAPIEntityBuilder<ReportTemplate>
{
    /// <summary>
    /// map entity ReportTemplate table
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
           .WithColumn(nameof(ReportTemplate.Code)).AsString(25).NotNullable()
           .WithColumn(nameof(ReportTemplate.Version)).AsString(100).Nullable()
           .WithColumn(nameof(ReportTemplate.App)).AsString(70).Nullable()
           .WithColumn(nameof(ReportTemplate.Status)).AsString(4000).Nullable()
           .WithColumn(nameof(ReportTemplate.Orientation)).AsString(100).Nullable()
           .WithColumn(nameof(ReportTemplate.PageWidth)).AsString(100).Nullable()
           .WithColumn(nameof(ReportTemplate.PageHeight)).AsString(100).Nullable()
           .WithColumn(nameof(ReportTemplate.PaperSize)).AsString(100).Nullable()
           .WithColumn(nameof(ReportTemplate.Watermark)).AsString(int.MaxValue).Nullable()
           .WithColumn(nameof(ReportTemplate.Border)).AsString(100).Nullable()
           .WithColumn(nameof(ReportTemplate.Margins)).AsString(100).Nullable()
           .WithColumn(nameof(ReportTemplate.FileContent)).AsString(int.MaxValue).Nullable()
           .WithColumn(nameof(ReportTemplate.Description)).AsString(int.MaxValue).Nullable()
           .WithColumn(nameof(ReportTemplate.OrganizationId)).AsString(int.MaxValue).Nullable();
    }
}
