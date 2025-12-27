using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Migrations.Builder;

[DatabaseType(DataProviderType.Oracle)]
public class WorkflowInfoBuilder : O24OpenAPIEntityBuilder<WorkflowInfo>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WorkflowInfo.execution_id))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(WorkflowInfo.input))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.correlation_id))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.input_string))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.workflow_id))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(WorkflowInfo.status))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WorkflowInfo.error))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.created_on))
            .AsInt64()
            .NotNullable()
            .WithColumn(nameof(WorkflowInfo.finish_on))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.is_timeout))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.is_processing))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.is_success))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.workflow_type))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.response_content))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.reversed_execution_id))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.reversed_by_execution_id))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.is_disputed))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.archiving_time))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.purging_time))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.approved_execution_id))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.transaction_number))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.transaction_date))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(WorkflowInfo.value_date))
            .AsString(50)
            .Nullable();
    }
}
