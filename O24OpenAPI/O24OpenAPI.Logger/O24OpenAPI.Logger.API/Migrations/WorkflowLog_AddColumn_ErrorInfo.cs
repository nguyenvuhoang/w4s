using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Logger.Domain.AggregateModels.WorkflowLogAggregate;

namespace O24OpenAPI.Logger.API.Migrations;

[O24OpenAPIMigration(
    "2025/01/02 14:00:01:0000000",
    "WorkflowLog_AddColumn_ErrorInfo",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class WorkflowLog_AddColumn_ErrorInfo : AutoReversingMigration
{
    public override void Up()
    {
        if (
            Schema.Table(nameof(WorkflowLog)).Exists()
            && !Schema.Table(nameof(WorkflowLog)).Column(nameof(WorkflowLog.error_info)).Exists()
        )
        {
            Alter
                .Table(nameof(WorkflowLog))
                .AddColumn(nameof(WorkflowLog.error_info))
                .AsString(4000)
                .Nullable();
        }
    }
}
