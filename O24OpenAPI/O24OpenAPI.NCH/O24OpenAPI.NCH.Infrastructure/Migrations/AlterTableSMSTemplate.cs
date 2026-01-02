using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Migrations;

[O24OpenAPIMigration(
    "2025/10/07 14:31:01:0000000",
    "Add column CreatedOnUtc/UpdatedOnUtc for SMSTemplate",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
[DatabaseType(DataProviderType.SqlServer)]
public class AlterTableSMSTemplate : AutoReversingMigration
{
    public override void Up()
    {
        if (
        Schema.Table(nameof(SMSTemplate)).Exists()
        && !Schema
            .Table(nameof(SMSTemplate))
            .Column(nameof(SMSTemplate.CreatedOnUtc))
            .Exists()
        )
        {
            Alter
                .Table(nameof(SMSTemplate))
                .AddColumn(nameof(SMSTemplate.CreatedOnUtc))
                .AsDateTime2()
                .Nullable();

        }

        if (
        Schema.Table(nameof(SMSTemplate)).Exists()
        && !Schema
            .Table(nameof(SMSTemplate))
            .Column(nameof(SMSTemplate.UpdatedOnUtc))
            .Exists()
        )
        {
            Alter
                .Table(nameof(SMSTemplate))
                .AddColumn(nameof(SMSTemplate.UpdatedOnUtc))
                .AsDateTime2()
                .Nullable();

        }
    }
}
