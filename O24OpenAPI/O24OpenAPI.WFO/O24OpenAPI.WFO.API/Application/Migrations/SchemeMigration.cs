using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.WFO.Domain.AggregateModels.ServiceAggregate;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Migrations;

[O24OpenAPIMigration(
    "2024/01/01 06:01:00:0000000",
    "6. Create SchemeMigration (Business Table)",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class SchemeMigration : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(WorkflowDef)).Exists())
        {
            Create.TableFor<WorkflowDef>();

            Create
                .UniqueConstraint("UC_WorkflowDef_WorkflowId_ChannelId")
                .OnTable(nameof(WorkflowDef))
                .Columns(nameof(WorkflowDef.WorkflowId), nameof(WorkflowDef.ChannelId));
        }

        if (!Schema.Table(nameof(WorkflowStep)).Exists())
        {
            Create.TableFor<WorkflowStep>();
            Create
                .UniqueConstraint("UC_WorkflowStep")
                .OnTable(nameof(WorkflowStep))
                .Columns(
                    nameof(WorkflowStep.WorkflowId),
                    nameof(WorkflowStep.StepCode),
                    nameof(WorkflowStep.StepOrder)
                );
        }

        if (!Schema.Table(nameof(ServiceInstance)).Exists())
        {
            Create.TableFor<ServiceInstance>();
            Create
                .UniqueConstraint("UC_ServiceInstance")
                .OnTable(nameof(ServiceInstance))
                .Columns(nameof(ServiceInstance.InstanceID), nameof(ServiceInstance.ServiceCode));
        }

        if (!Schema.Table(nameof(WorkflowInfo)).Exists())
        {
            Create.TableFor<WorkflowInfo>();
            Create
                .UniqueConstraint("UC_WorkflowInfo")
                .OnTable(nameof(WorkflowInfo))
                .Columns(nameof(WorkflowInfo.execution_id));
            Create
                .Index("IDX_correlation_id")
                .OnTable(nameof(WorkflowInfo))
                .OnColumn(nameof(WorkflowInfo.correlation_id));
        }

        if (!Schema.Table(nameof(WorkflowStepInfo)).Exists())
        {
            Create.TableFor<WorkflowStepInfo>();
            Create
                .UniqueConstraint("UC_WorkflowStepInfo")
                .OnTable(nameof(WorkflowStepInfo))
                .Columns(
                    nameof(WorkflowStepInfo.execution_id),
                    nameof(WorkflowStepInfo.step_execution_id)
                );
        }
    }
}
