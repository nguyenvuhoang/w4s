using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.Logger.Domain;

namespace O24OpenAPI.Logger.Migrations;

/// <summary>
/// The schema migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2025/01/01 14:00:00:0000000",
    "Create ServiceLog",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class SchemaMigration : AutoReversingMigration
{
    /// <summary>
    /// Ups this instance
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(ServiceLog)).Exists())
        {
            Create.TableFor<ServiceLog>();
        }
    }
}

/// <summary>
/// The add http log migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2025/01/01 14:00:05:0000000",
    "Create HttpLog",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AddHttpLogMigration : AutoReversingMigration
{
    /// <summary>
    /// Ups this instance
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(HttpLog)).Exists())
        {
            Create.TableFor<HttpLog>();
        }
    }
}

/// <summary>
/// The add http log migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2025/01/01 16:12:12:0000000",
    "Add WFLog Table",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AddWFLogMigration : AutoReversingMigration
{
    /// <summary>
    /// Ups this instance
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(WorkflowLog)).Exists())
        {
            Create.TableFor<WorkflowLog>();
            Create
                .UniqueConstraint("UC_WorkflowLog")
                .OnTable(nameof(WorkflowLog))
                .Columns(nameof(WorkflowLog.execution_id));
        }

        if (!Schema.Table(nameof(WorkflowStepLog)).Exists())
        {
            Create.TableFor<WorkflowStepLog>();
            Create
                .UniqueConstraint("UC_WorkflowStepLog")
                .OnTable(nameof(WorkflowStepLog))
                .Columns(
                    nameof(WorkflowStepLog.execution_id),
                    nameof(WorkflowStepLog.step_execution_id)
                );
        }
    }
}

/// <summary>
///
/// </summary> <summary>
///
/// </summary>
[O24OpenAPIMigration(
    "2025/01/01 16:14:16:0000000",
    "AlterTableServiceLogMigration",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AlterTableServiceLogMigration : AutoReversingMigration
{
    /// <summary>
    /// Ups this instance
    /// </summary>
    public override void Up()
    {
        if (
            Schema.Table(nameof(ServiceLog)).Exists()
            && !Schema.Table(nameof(ServiceLog)).Column(nameof(ServiceLog.Code)).Exists()
        )
        {
            Alter
                .Table(nameof(ServiceLog))
                .AddColumn(nameof(ServiceLog.Code))
                .AsString(100)
                .Nullable()
                .WithDefaultValue(string.Empty);
        }

        if (
            Schema.Table(nameof(ServiceLog)).Exists()
            && !Schema.Table(nameof(ServiceLog)).Column(nameof(ServiceLog.ExecutionLogId)).Exists()
        )
        {
            Alter
                .Table(nameof(ServiceLog))
                .AddColumn(nameof(ServiceLog.ExecutionLogId))
                .AsString(36)
                .Nullable();
        }
    }
}

/// <summary>
/// The add application log migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2024/01/01 16:16:21:0000000",
    "Add ApplicationLog Table",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AddApplicationLogMigration : AutoReversingMigration
{
    /// <summary>
    /// Ups this instance
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(ApplicationLog)).Exists())
        {
            Create.TableFor<ApplicationLog>();

            Create
                .Index("IX_ApplicationLog_Timestamp")
                .OnTable(nameof(ApplicationLog))
                .OnColumn(nameof(ApplicationLog.LogTimestamp));

            Create
                .Index("IX_ApplicationLog_CorrelationId")
                .OnTable(nameof(ApplicationLog))
                .OnColumn(nameof(ApplicationLog.CorrelationId));
        }
    }
}
