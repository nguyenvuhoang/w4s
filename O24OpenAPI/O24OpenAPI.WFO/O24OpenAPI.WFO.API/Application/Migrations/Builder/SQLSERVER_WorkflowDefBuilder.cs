using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Migrations.Builder;

[DatabaseType(DataProviderType.SqlServer)]
public class SQLSERVER_WorkflowDefBuilder : O24OpenAPIEntityBuilder<WorkflowDef>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WorkflowDef.WorkflowId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(WorkflowDef.WorkflowName))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowDef.Description))
            .AsString(4000)
            .Nullable()
            .WithColumn(nameof(WorkflowDef.ChannelId))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(WorkflowDef.Status))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(WorkflowDef.Timeout))
            .AsInt64()
            .NotNullable()
            .WithColumn(nameof(WorkflowDef.IsReverse))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(WorkflowDef.TemplateResponse))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowDef.WorkflowEvent))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowDef.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(WorkflowDef.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}
