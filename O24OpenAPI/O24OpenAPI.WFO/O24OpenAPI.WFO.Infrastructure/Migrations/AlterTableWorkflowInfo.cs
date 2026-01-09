using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.Infrastructure.Migrations;

[O24OpenAPIMigration(
    "2025/01/03 06:08:06:0000000",
    "Add column error_info",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AlterTableWorkflowInfo : AutoReversingMigration
{
    public override void Up()
    {
        if (
            Schema.Table(nameof(WorkflowInfo)).Exists()
            && !Schema.Table(nameof(WorkflowInfo)).Column(nameof(WorkflowInfo.error_info)).Exists()
        )
        {
            Alter
                .Table(nameof(WorkflowInfo))
                .AddColumn(nameof(WorkflowInfo.error_info))
                .AsString(4000)
                .Nullable();
        }
    }
}
