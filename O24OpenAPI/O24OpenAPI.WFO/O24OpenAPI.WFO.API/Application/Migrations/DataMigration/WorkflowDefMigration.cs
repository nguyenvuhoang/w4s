using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Data.Utils;
using O24OpenAPI.WFO.API.Application.Migrations;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Migrations.DataMigration;

[O24OpenAPIMigration(
    "2025/11/28 14:00:43:0000000",
    "Init Data WorkflowDef",
    MigrationProcessType.Installation
)]
[Environment([EnvironmentType.All])]
public class WorkflowDefMigration : BaseMigration
{
    public override void Up()
    {
        var path = "Migrations/DataJson/All/WorkflowDefData.json";
        var list = FileUtils.ReadJson<WorkflowDef>(path).GetAwaiter().GetResult();
        SeedListData(list, WFOConditionField.WorkflowDefCondition).Wait();
    }
}
