using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;


/// <summary>
/// The workflow step log builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{WorkflowStepLog}"/>

[DatabaseType(DataProviderType.Oracle)]
public class ORACLE_WorkflowStepLogBuilder : O24OpenAPIEntityBuilder<WorkflowStepLog>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WorkflowStepLog.ExecutionId))
            .AsString(40)
            .NotNullable()
            .WithColumn(nameof(WorkflowStepLog.CacheExecutionId))
            .AsString(40)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.StepExecutionId))
            .AsString(40)
            .NotNullable()
            .WithColumn(nameof(WorkflowStepLog.StepCode))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(WorkflowStepLog.Status))
            .AsString(20)
            .WithColumn(nameof(WorkflowStepLog.UserId))
            .AsString(40)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.TxContextData))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.RequestData))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.ResponseData))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.ServiceId))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.ServerIp))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.UtcSendTime))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.ProcessIn))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.ReversalExecutionId))
            .AsString(40)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.ApprovalExecutionId))
            .AsString(40)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.ProcessingVersion))
            .AsString(40)
            .Nullable()
            .WithColumn(nameof(WorkflowStepLog.WorkflowScheme))
            .AsNCLOB()
            .Nullable();
    }
}
