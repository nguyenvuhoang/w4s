using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Logger.Domain;

namespace O24OpenAPI.Logger.Migrations.Builders;

/// <summary>
/// The workflow step log builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{WorkflowStepLog}"/>
public class WorkflowStepLogBuilder : O24OpenAPIEntityBuilder<WorkflowStepLog>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WorkflowStepLog.step_execution_id))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(WorkflowStepLog.execution_id))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(WorkflowStepLog.step_order))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WorkflowStepLog.step_code))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.sending_condition))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.p1_request))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.p1_start))
            .AsInt64()
            .NotNullable()
            .WithColumn(nameof(WorkflowStepLog.p1_finish))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.p1_status))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WorkflowStepLog.p1_error))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.p1_content))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.p2_request))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.p2_start))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.p2_finish))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.p2_status))
            .AsInt32()
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.p2_error))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.p2_error_code))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.p2_content))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.is_success))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.is_timeout))
            .AsString(10)
            .Nullable();
    }
}
