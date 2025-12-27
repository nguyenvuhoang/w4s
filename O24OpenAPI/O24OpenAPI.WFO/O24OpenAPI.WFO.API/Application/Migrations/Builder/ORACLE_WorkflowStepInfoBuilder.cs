using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Migrations.Builder;

[DatabaseType(DataProviderType.Oracle)]
public class ORACLE_WorkflowStepInfoBuilder : O24OpenAPIEntityBuilder<WorkflowStepInfo>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WorkflowStepInfo.step_execution_id))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(WorkflowStepInfo.execution_id))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(WorkflowStepInfo.step_order))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WorkflowStepInfo.step_code))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.sending_condition))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.p1_request))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.p1_start))
            .AsInt64()
            .NotNullable()
            .WithColumn(nameof(WorkflowStepInfo.p1_finish))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.p1_status))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WorkflowStepInfo.p1_error))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.p1_content))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.p2_request))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.p2_start))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.p2_finish))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.p2_status))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.p2_error))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.p2_error_code))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.p2_content))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.is_success))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.is_timeout))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(WorkflowStepInfo.execution_servcie))
            .AsString(255)
            .Nullable();
    }
}
