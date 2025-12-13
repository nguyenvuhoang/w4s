using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.O24ACT.Domain;

namespace O24OpenAPI.O24ACT.Migrations;

[O24OpenAPIMigration(
    "2025/12/06 20:06:06:0000000",
    "Add TransId column + unique index on AccountStatementDone",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AlterTableAccountStatementDone : AutoReversingMigration
{
    public override void Up()
    {
        if (
            Schema.Table(nameof(AccountStatement)).Exists()
            && !Schema
                .Table(nameof(AccountStatement))
                .Column(nameof(AccountStatement.TransId))
                .Exists()
        )
        {
            // Add column
            Alter
                .Table(nameof(AccountStatement))
                .AddColumn(nameof(AccountStatement.TransId))
                .AsString(36)
                .Nullable();
        }

        // Add unique index if not exists
        if (!Schema.Table(nameof(AccountStatement))
            .Index("UQ_AccountStatement_TransId")
            .Exists())
        {
            Create.Index("UQ_AccountStatement_TransId")
                .OnTable(nameof(AccountStatement))
                .OnColumn(nameof(AccountStatement.TransId)).Ascending()
                .WithOptions().Unique();
        }

        if (
            Schema.Table(nameof(AccountStatementDone)).Exists()
            && !Schema
                .Table(nameof(AccountStatementDone))
                .Column(nameof(AccountStatementDone.TransId))
                .Exists()
        )
        {
            // Add column
            Alter
                .Table(nameof(AccountStatementDone))
                .AddColumn(nameof(AccountStatementDone.TransId))
                .AsString(36)
                .Nullable();
        }
        // Add unique index if not exists
        if (!Schema.Table(nameof(AccountStatementDone))
            .Index("UQ_AccountStatementDone_TransId")
            .Exists())
        {
            Create.Index("UQ_AccountStatementDone_TransId")
                .OnTable(nameof(AccountStatementDone))
                .OnColumn(nameof(AccountStatementDone.TransId)).Ascending()
                .WithOptions().Unique();
        }
    }
}
