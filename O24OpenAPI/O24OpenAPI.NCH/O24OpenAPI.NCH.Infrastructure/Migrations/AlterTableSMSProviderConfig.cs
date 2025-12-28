using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.NCH.Domain;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Migrations;

[O24OpenAPIMigration(
    "2025/10/18 14:32:01:0000000",
    "Add column IsMainKey for SMSProviderConfig",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
[DatabaseType(DataProviderType.SqlServer)]
public class AlterTableSMSProviderConfig : AutoReversingMigration
{
    public override void Up()
    {
        if (
        Schema.Table(nameof(SMSProviderConfig)).Exists()
        && !Schema
            .Table(nameof(SMSProviderConfig))
            .Column(nameof(SMSProviderConfig.IsMainKey))
            .Exists()
        )
        {
            Alter
                .Table(nameof(SMSProviderConfig))
                .AddColumn(nameof(SMSProviderConfig.IsMainKey))
                .AsBoolean()
                .Nullable();

        }
    }
}
