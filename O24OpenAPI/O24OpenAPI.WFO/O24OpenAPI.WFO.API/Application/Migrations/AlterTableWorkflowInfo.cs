using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Migrations;

[O24OpenAPIMigration(
    "2024/01/03 06:06:06:0000000",
    "Add column correlation_id",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AlterTableWorkflowInfo : AutoReversingMigration
{
    public override void Up()
    {
        if (
            Schema.Table(nameof(WorkflowInfo)).Exists()
            && !Schema
                .Table(nameof(WorkflowInfo))
                .Column(nameof(WorkflowInfo.correlation_id))
                .Exists()
        )
        {
            Alter
                .Table(nameof(WorkflowInfo))
                .AddColumn(nameof(WorkflowInfo.correlation_id))
                .AsString(255)
                .Nullable();

            Create
                .Index("IDX_correlation_id")
                .OnTable(nameof(WorkflowInfo))
                .OnColumn(nameof(WorkflowInfo.correlation_id));
        }
    }
}
