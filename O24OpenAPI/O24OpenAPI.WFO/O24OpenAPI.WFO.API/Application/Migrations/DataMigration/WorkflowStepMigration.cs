using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Data.Utils;
using O24OpenAPI.WFO.API.Application.Migrations;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Migrations.DataMigration;

[O24OpenAPIMigration(
    "2025/11/28 15:00:43:0000000",
    "Init Data WorkflowStep",
    MigrationProcessType.Installation
)]
[Environment([EnvironmentType.All])]
public class WorkflowStepMigration : BaseMigration
{
    public override void Up()
    {
        var path = "Migrations/DataJson/All/WorkflowStepData.json";
        var list = FileUtils.ReadJson<WorkflowStep>(path).GetAwaiter().GetResult();
        SeedListData(list, WFOConditionField.WorkflowStepCondition).Wait();
    }
}
