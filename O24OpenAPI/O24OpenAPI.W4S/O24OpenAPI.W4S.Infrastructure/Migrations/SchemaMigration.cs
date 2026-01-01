using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Migrations;

/// <summary>
/// The schema migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2025/01/01 21:23:00:0000000",
    "Table For WalletProfile",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
[DatabaseType(DataProviderType.SqlServer)]
public class SchemaMigration : AutoReversingMigration
{
    /// <summary>
    /// Ups this instance
    /// </summary>
    public override void Up()
    {

        if (!Schema.Table(nameof(WalletProfile)).Exists())
        {
            Create.TableFor<WalletProfile>();
            Create.Index("IDX_WalletProfile_UserId")
                .OnTable(nameof(WalletProfile))
                .OnColumn(nameof(WalletProfile.UserCode))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create.Index("IDX_WalletProfile_WalletType")
                .OnTable(nameof(WalletProfile))
                .OnColumn(nameof(WalletProfile.WalletType))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create.Index("IDX_WalletProfile_Status")
                .OnTable(nameof(WalletProfile))
                .OnColumn(nameof(WalletProfile.Status))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(WalletContract)).Exists())
        {
            Create.TableFor<WalletContract>();

            // ===== Unique: ContractNumber (must-have) =====
            Create.Index("UQ_WalletContract_ContractNumber")
                .OnTable(nameof(WalletContract))
                .OnColumn(nameof(WalletContract.ContractNumber))
                .Ascending()
                .WithOptions()
                .Unique();

            // ===== Query indexes =====
            Create.Index("IDX_WalletContract_CustomerCode")
                .OnTable(nameof(WalletContract))
                .OnColumn(nameof(WalletContract.CustomerCode))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create.Index("IDX_WalletContract_Status")
                .OnTable(nameof(WalletContract))
                .OnColumn(nameof(WalletContract.Status))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create.Index("IDX_WalletContract_PolicyCode")
                .OnTable(nameof(WalletContract))
                .OnColumn(nameof(WalletContract.PolicyCode))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create.Index("IDX_WalletContract_OpenDateUtc")
                .OnTable(nameof(WalletContract))
                .OnColumn(nameof(WalletContract.OpenDateUtc))
                .Descending()
                .WithOptions()
                .NonClustered();
        }

    }
}
