using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Data.Utils;
using O24OpenAPI.Framework.Domain;

namespace O24OpenAPI.NCH.Migrations.DataMigration;

[O24OpenAPIMigration(
    "2025/08/12 09:33:33:0000000",
    "Update Data O24OpenAPIService NCH_GET_UNREAD_COUNT",
    MigrationProcessType.Installation
)]
[Environment([EnvironmentType.All])]
[DatabaseType(DataProviderType.SqlServer)]
public class O24OpenAPIServiceMigration : BaseMigration
{
    public override void Up()
    {
        var path =
            "Migrations/DataJson/O24OpenAPIService_NCH_GET_UNREAD_COUNT/O24OpenAPIServiceData.json";
        var list = FileUtils.ReadJson<O24OpenAPIService>(path).GetAwaiter().GetResult();
        SeedListData(list, DataConditionField.O24OpenAPIServiceCondition).Wait();
    }
}
