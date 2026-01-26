using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Migrations;

[O24OpenAPIMigration(
    "2026/01/23 21:20:08:0000000",
    "Add field IsPrimary",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AlterTableWallBalance : AutoReversingMigration
{
    public override void Up()
    {
        if (
            Schema.Table(nameof(WalletBalance)).Exists()
            && !Schema.Table(nameof(WalletBalance)).Column(nameof(WalletBalance.CurrencyCode)).Exists()
        )
        {
            Alter
                .Table(nameof(WalletBalance))
                .AddColumn(nameof(WalletBalance.CurrencyCode))
                .AsString(3)
                .Nullable();
        }
    }
}
