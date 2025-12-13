using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Migrations;
using FileUtils = O24OpenAPI.Data.Utils.FileUtils;

namespace O24OpenAPI.Web.CMS.Migrations.DataMigration;

[O24OpenAPIMigration(
    "2025/08/20 09:15:11:0000000",
    "Insert LearnApi Search, Update for WorkflowDef, WorkflowStep",
    MigrationProcessType.Installation
)]
[Environment([EnvironmentType.All])]
[DatabaseType(DataProviderType.SqlServer)]
public class LearnApiMigration : BaseMigration
{
    public override void Up()
    {
        var path = "Migrations/DataJson/Update_LearnApi/LearnApiData.json";
        var list = FileUtils.ReadJson<LearnApi>(path).GetAwaiter().GetResult();
        SeedListData(list, CMSConditionField.LearnApiCondition).Wait();
    }
}
