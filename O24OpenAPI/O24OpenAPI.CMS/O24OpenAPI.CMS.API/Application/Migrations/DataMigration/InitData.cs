using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using FileUtils = O24OpenAPI.Data.Utils.FileUtils;

namespace O24OpenAPI.CMS.API.Application.Migrations.DataMigration;

[O24OpenAPIMigration(
    "2025/09/04 14:32:43:0000000",
    "Init Data CoreApiKeys",
    MigrationProcessType.Installation
)]
[Environment([EnvironmentType.All])]
public class InitData : BaseMigration
{
    public override void Up()
    {
        var path = "Migrations/DataJson/CoreApiKeys/CoreApiKeysData.json";
        var list = FileUtils.ReadJson<CoreApiKeys>(path).GetAwaiter().GetResult();
        SeedListData(list, CMSConditionField.CoreApiKeysCondition).Wait();
    }
}
