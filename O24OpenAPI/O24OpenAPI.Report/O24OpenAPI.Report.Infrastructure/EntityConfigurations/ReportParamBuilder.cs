using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Report.Domain.AggregateModels.ReportAggregate;

namespace O24OpenAPI.Report.Infrastructure.EntityConfigurations;

public partial class ReportParamBuilder : O24OpenAPIEntityBuilder<ReportParam>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ReportParam.ReportCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(ReportParam.ParamName))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(ReportParam.ControlName))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(ReportParam.ControlType))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(ReportParam.Width))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(ReportParam.Height))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(ReportParam.DataStoreType))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(ReportParam.Store))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(ReportParam.View))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(ReportParam.Key))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(ReportParam.Text))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(ReportParam.Value))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(ReportParam.LangId))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(ReportParam.Orderby))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(ReportParam.Tag))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(ReportParam.TimeReport))
            .AsString(50)
            .Nullable();
    }
}
