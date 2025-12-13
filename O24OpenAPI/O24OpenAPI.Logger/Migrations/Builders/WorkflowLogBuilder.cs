using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Logger.Domain;

namespace O24OpenAPI.Logger.Migrations.Builders;

/// <summary>
/// The workflow log builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{WorkflowLog}"/>
public class WorkflowLogBuilder : O24OpenAPIEntityBuilder<WorkflowLog>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WorkflowLog.execution_id))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(WorkflowLog.input))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.input_string))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.workflow_id))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(WorkflowLog.status))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WorkflowLog.error))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.created_on))
            .AsInt64()
            .NotNullable()
            .WithColumn(nameof(WorkflowLog.finish_on))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowLog.is_timeout))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.is_processing))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.is_success))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.workflow_type))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.response_content))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.reversed_execution_id))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.reversed_by_execution_id))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.is_disputed))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.archiving_time))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowLog.purging_time))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowLog.approved_execution_id))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.transaction_number))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.transaction_date))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(WorkflowLog.value_date))
            .AsString(50)
            .Nullable();
    }
}
