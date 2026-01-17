using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Migrations;

[O24OpenAPIMigration(
    "2026/01/16 21:20:08:0000000",
    "Add field IsPrimary",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AlterTableWalletProfile : AutoReversingMigration
{
    public override void Up()
    {
        if (
            Schema.Table(nameof(WalletProfile)).Exists()
            && !Schema.Table(nameof(WalletProfile)).Column(nameof(WalletProfile.Icon)).Exists()
        )
        {
            Alter
                .Table(nameof(WalletProfile))
                .AddColumn(nameof(WalletProfile.Icon))
                .AsString(100)
                .Nullable();
        }

        if (
            Schema.Table(nameof(WalletProfile)).Exists()
            && !Schema.Table(nameof(WalletProfile)).Column(nameof(WalletProfile.Color)).Exists()
        )
        {
            Alter
                .Table(nameof(WalletProfile))
                .AddColumn(nameof(WalletProfile.Color))
                .AsString(100)
                .Nullable();
        }
        if (
          Schema.Table(nameof(WalletProfile)).Exists()
          && !Schema.Table(nameof(WalletProfile)).Column(nameof(WalletProfile.IsPrimary)).Exists()
        )
        {
            Alter
                .Table(nameof(WalletProfile))
                .AddColumn(nameof(WalletProfile.IsPrimary))
                .AsBoolean()
                .Nullable();
        }
    }
}
