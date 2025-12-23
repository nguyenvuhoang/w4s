using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Data.Utils;
using O24OpenAPI.O24NCH.Domain;

namespace O24OpenAPI.O24NCH.Migrations.DataMigration;

[O24OpenAPIMigration(
    "2025/10/20 12:49:43:0000000",
    "Init Data Migration SMSTemplate",
    MigrationProcessType.Installation
)]
[Environment([EnvironmentType.All])]
public class SMSMappingMigration : BaseMigration
{
    public override void Up()
    {
        var pathSMSMappingResponseData = "Migrations/DataJson/SMSMappingResponse/SMSMappingResponseData.json";
        var listSMSMappingResponseData = FileUtils.ReadJson<SMSMappingResponse>(pathSMSMappingResponseData).GetAwaiter().GetResult();
        SeedListData(listSMSMappingResponseData, NCHConditionField.SMSMappingResponseCondition).Wait();
    }
}
