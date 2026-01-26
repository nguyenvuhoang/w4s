using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data.Migrations;

namespace O24OpenAPI.CTH.Infrastructure.Migrations;

[O24OpenAPIMigration(
    "2026/01/26 12:02:02:0000000",
    "2. Alter Table UserAccount Add Column CurrencyCode",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AlterTableUserAccount : AutoReversingMigration
{
    public override void Up()
    {
        if (
            Schema.Table(nameof(UserAccount)).Exists()
            && !Schema
                .Table(nameof(UserAccount))
                .Column(nameof(UserAccount.CurrencyCode))
                .Exists()
        )
        {
            Alter
                .Table(nameof(UserAccount))
                .AddColumn(nameof(UserAccount.CurrencyCode))
                .AsString(3)
                .Nullable();
        }
    }
}
