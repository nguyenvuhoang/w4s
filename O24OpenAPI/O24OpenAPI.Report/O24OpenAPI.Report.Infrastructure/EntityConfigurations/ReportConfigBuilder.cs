using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Report.Domain.AggregateModels.ReportAggregate;

namespace O24OpenAPI.Report.Infrastructure.EntityConfigurations;

public partial class ReportConfigBuilder : O24OpenAPIEntityBuilder<ReportConfig>
{
    /// <summary>
    /// map entity ReportConfig table
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ReportConfig.Code))
            .AsString(25)
            .WithColumn(nameof(ReportConfig.Name))
            .AsString(25)
            .Nullable()
            .WithColumn(nameof(ReportConfig.Version))
            .AsString(100)
            .WithColumn(nameof(ReportConfig.CodeTemplate))
            .AsString(25)
            .WithColumn(nameof(ReportConfig.DataSource))
            .AsString(100)
            .WithColumn(nameof(ReportConfig.FullClassName))
            .AsString(100)
            .WithColumn(nameof(ReportConfig.MethodName))
            .AsString(100)
            .WithColumn(nameof(ReportConfig.Description))
            .AsString(200)
            .WithColumn(nameof(ReportConfig.IsAsync))
            .AsBoolean()
            .WithColumn(nameof(ReportConfig.MailConfigCode))
            .AsString(70)
            .WithColumn(nameof(ReportConfig.Status))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(ReportConfig.OrganizationId))
            .AsString(255)
            .Nullable();
    }
}
