using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.WFO.Domain;

namespace O24OpenAPI.WFO.Migrations;

[O24OpenAPIMigration(
    "2025/11/26 06:06:06:0000000",
    "Add column execution_servcie to WorkflowStepInfo ",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AlterTableWorkflowStepInfo : AutoReversingMigration
{
    public override void Up()
    {
        if (
            Schema.Table(nameof(WorkflowStepInfo)).Exists()
            && !Schema
                .Table(nameof(WorkflowStepInfo))
                .Column(nameof(WorkflowStepInfo.execution_servcie))
                .Exists()
        )
        {
            Alter
                .Table(nameof(WorkflowStepInfo))
                .AddColumn(nameof(WorkflowStepInfo.execution_servcie))
                .AsString(255)
                .Nullable();
        }
    }
}
