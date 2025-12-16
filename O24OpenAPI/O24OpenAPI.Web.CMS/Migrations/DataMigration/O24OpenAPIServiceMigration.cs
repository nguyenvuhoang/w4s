using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Framework.Domain;
using FileUtils = O24OpenAPI.Data.Utils.FileUtils;

namespace O24OpenAPI.Web.CMS.Migrations.DataMigration;

[O24OpenAPIMigration(
    "2025/08/25 11:15:11:0000000",
    "Insert O24OpenAPIService",
    MigrationProcessType.Installation
)]
[Environment([EnvironmentType.All])]
[DatabaseType(DataProviderType.Oracle)]
public class O24OpenAPIServiceMigration : BaseMigration
{
    public override void Up()
    {
        var path = "Migrations/DataJson/O24OpenAPIService/O24OpenAPIServiceData.json";
        var list = FileUtils.ReadJson<O24OpenAPIService>(path).GetAwaiter().GetResult();
        SeedListData(list, CMSConditionField.O24OpenAPIServiceCondition).Wait();
    }
}
