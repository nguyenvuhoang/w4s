using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

[DatabaseType(DataProviderType.Oracle)]

public class ORACLE_LearnApiExecutionLogBuilder : O24OpenAPIEntityBuilder<LearnApiExecutionLog>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(LearnApiExecutionLog.ExecuteId))
            .AsString(40)
            .NotNullable()
            .WithColumn(nameof(LearnApiExecutionLog.UserId))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(LearnApiExecutionLog.WorkflowId))
            .AsString(200)
            .NotNullable()
            .WithColumn(nameof(LearnApiExecutionLog.Input))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(LearnApiExecutionLog.Output))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(LearnApiExecutionLog.Status))
            .AsString(20)
            .NotNullable()
            .WithColumn(nameof(LearnApiExecutionLog.CreateOn))
            .AsDateTime2()
            .WithColumn(nameof(LearnApiExecutionLog.FinishOn))
            .AsDateTime2()
            .WithColumn(nameof(LearnApiExecutionLog.WorkflowFunc))
            .AsString(40)
            .Nullable()
            .WithColumn(nameof(LearnApiExecutionLog.TableName))
            .AsString(40)
            .Nullable()
            .WithColumn(nameof(LearnApiExecutionLog.IdFieldName))
            .AsString(40)
            .Nullable()
            .WithColumn(nameof(LearnApiExecutionLog.Module))
            .AsString(40)
            .Nullable()
            .WithColumn(nameof(LearnApiExecutionLog.TxCode))
            .AsString(40)
            .Nullable();
    }
}
