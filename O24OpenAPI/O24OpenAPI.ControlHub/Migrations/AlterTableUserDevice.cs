using FluentMigrator;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;

namespace O24OpenAPI.ControlHub.Migrations;

[O24OpenAPIMigration(
    "2025/10/30 15:02:02:0000000",
    "Add Column Network/Memory into Table UserDevice",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AlterTableUserDevice : AutoReversingMigration
{
    public override void Up()
    {
        if (
            Schema.Table(nameof(UserDevice)).Exists()
            && !Schema
                .Table(nameof(UserDevice))
                .Column(nameof(UserDevice.Network))
                .Exists()
        )
        {
            Alter
                .Table(nameof(UserDevice))
                .AddColumn(nameof(UserDevice.Network))
                .AsString(500)
                .Nullable();
        }

        if (
            Schema.Table(nameof(UserDevice)).Exists()
            && !Schema
                .Table(nameof(UserDevice))
                .Column(nameof(UserDevice.Memory))
                .Exists()
        )
        {
            Alter
                .Table(nameof(UserDevice))
                .AddColumn(nameof(UserDevice.Memory))
                .AsString(100)
                .Nullable();
        }
    }
}
