using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.Data.Migrations;

namespace O24OpenAPI.CTH.Infrastructure.Migrations;

[O24OpenAPIMigration(
    "2026/01/19 12:02:02:0000000",
    "2. Alter Table Channel Add Column AppICon",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AlterTableChannel : AutoReversingMigration
{
    public override void Up()
    {
        if (
            Schema.Table(nameof(Channel)).Exists()
            && !Schema
                .Table(nameof(Channel))
                .Column(nameof(Channel.IsMaster))
                .Exists()
        )
        {
            Alter
                .Table(nameof(Channel))
                .AddColumn(nameof(Channel.IsMaster))
                .AsBoolean()
                .Nullable()
                .WithDefaultValue(false);
        }

        if (
            Schema.Table(nameof(Channel)).Exists()
            && !Schema
                .Table(nameof(Channel))
                .Column(nameof(Channel.AppICon))
                .Exists()
        )
        {
            Alter
                .Table(nameof(Channel))
                .AddColumn(nameof(Channel.AppICon))
                .AsString(500)
                .Nullable();
        }

    }
}
