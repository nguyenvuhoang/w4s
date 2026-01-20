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
    "2026/01/20 16:10:01:0000000",
    "10. Init W4S table (WalletCounterparty)",
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
            Create
                .Index("IDX_WalletProfile_UserId")
                .OnTable(nameof(WalletProfile))
                .OnColumn(nameof(WalletProfile.UserCode))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IDX_WalletProfile_WalletType")
                .OnTable(nameof(WalletProfile))
                .OnColumn(nameof(WalletProfile.WalletType))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IDX_WalletProfile_Status")
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
            Create
                .Index("UQ_WalletContract_ContractNumber")
                .OnTable(nameof(WalletContract))
                .OnColumn(nameof(WalletContract.ContractNumber))
                .Ascending()
                .WithOptions()
                .Unique();

            // ===== Query indexes =====
            Create
                .Index("IDX_WalletContract_CustomerCode")
                .OnTable(nameof(WalletContract))
                .OnColumn(nameof(WalletContract.CustomerCode))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IDX_WalletContract_Status")
                .OnTable(nameof(WalletContract))
                .OnColumn(nameof(WalletContract.Status))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IDX_WalletContract_PolicyCode")
                .OnTable(nameof(WalletContract))
                .OnColumn(nameof(WalletContract.PolicyCode))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IDX_WalletContract_OpenDateUtc")
                .OnTable(nameof(WalletContract))
                .OnColumn(nameof(WalletContract.OpenDateUtc))
                .Descending()
                .WithOptions()
                .NonClustered();
        }
        if (!Schema.Table(nameof(WalletCategoryDefault)).Exists())
        {
            Create.TableFor<WalletCategoryDefault>();
            Create
                .Index("IDX_WalletCategoryDefault_ParentCategoryCode")
                .OnTable(nameof(WalletCategoryDefault))
                .OnColumn(nameof(WalletCategoryDefault.ParentCategoryCode))
                .Ascending()
                .WithOptions()
                .NonClustered();
            Create
                .Index("IDX_WalletCategoryDefault_CategoryType")
                .OnTable(nameof(WalletCategoryDefault))
                .OnColumn(nameof(WalletCategoryDefault.CategoryType))
                .Ascending()
                .WithOptions()
                .NonClustered();
            Create
                .Index("IDX_WalletCategoryDefault_CategoryGroup")
                .OnTable(nameof(WalletCategoryDefault))
                .OnColumn(nameof(WalletCategoryDefault.CategoryGroup))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table("WalletAvatar").Exists())
        {
            Create.TableFor<WalletAvatar>();

            Create
                .Index("IX_WalletAvatar_WalletId_CreatedOnUtc")
                .OnTable("WalletAvatar")
                .OnColumn("WalletId")
                .Ascending()
                .OnColumn("CreatedOnUtc")
                .Descending();

            Create
                .Index("IX_WalletAvatar_UserCode_CreatedOnUtc")
                .OnTable("WalletAvatar")
                .OnColumn("UserCode")
                .Ascending()
                .OnColumn("CreatedOnUtc")
                .Descending();
        }

        if (!Schema.Table(nameof(WalletStatement)).Exists())
        {
            Create.TableFor<WalletStatement>();

            Create.Index("IX_WalletStatement_WalletId_StatementOnUtc")
                .OnTable(nameof(WalletStatement))
                .OnColumn(nameof(WalletStatement.WalletId)).Ascending()
                .OnColumn(nameof(WalletStatement.StatementOnUtc)).Descending();

            Create.Index("IX_WalletStatement_WalletId_AccountNumber_StatementOnUtc")
                .OnTable(nameof(WalletStatement))
                .OnColumn(nameof(WalletStatement.WalletId)).Ascending()
                .OnColumn(nameof(WalletStatement.AccountNumber)).Ascending()
                .OnColumn(nameof(WalletStatement.StatementOnUtc)).Descending();

            Create.Index("IX_WalletStatement_WalletId_IsReconciled_StatementOnUtc")
                .OnTable(nameof(WalletStatement))
                .OnColumn(nameof(WalletStatement.WalletId)).Ascending()
                .OnColumn(nameof(WalletStatement.IsReconciled)).Ascending()
                .OnColumn(nameof(WalletStatement.StatementOnUtc)).Descending();

            Create.Index("IX_WalletStatement_ReferenceType_ReferenceId")
                .OnTable(nameof(WalletStatement))
                .OnColumn(nameof(WalletStatement.ReferenceType)).Ascending()
                .OnColumn(nameof(WalletStatement.ReferenceId)).Ascending();

            Create.Index("IX_WalletStatement_ExternalRef")
                .OnTable(nameof(WalletStatement))
                .OnColumn(nameof(WalletStatement.ExternalRef)).Ascending();

            Create.Index("IX_WalletStatement_WalletId_CategoryId_StatementOnUtc")
                .OnTable(nameof(WalletStatement))
                .OnColumn(nameof(WalletStatement.WalletId)).Ascending()
                .OnColumn(nameof(WalletStatement.CategoryId)).Ascending()
                .OnColumn(nameof(WalletStatement.StatementOnUtc)).Descending();

            Create.Index("IX_WalletStatement_WalletId_EventId_StatementOnUtc")
                .OnTable(nameof(WalletStatement))
                .OnColumn(nameof(WalletStatement.WalletId)).Ascending()
                .OnColumn(nameof(WalletStatement.EventId)).Ascending()
                .OnColumn(nameof(WalletStatement.StatementOnUtc)).Descending();
        }

        if (!Schema.Table(nameof(WalletEvent)).Exists())
        {
            Create.TableFor<WalletEvent>();

            Create.Index("IX_WalletEvent_WalletId_StartOnUtc")
                .OnTable(nameof(WalletEvent))
                .OnColumn(nameof(WalletEvent.WalletId)).Ascending()
                .OnColumn(nameof(WalletEvent.StartOnUtc)).Descending();

            Create.Index("IX_WalletEvent_WalletId_Status_StartOnUtc")
                .OnTable(nameof(WalletEvent))
                .OnColumn(nameof(WalletEvent.WalletId)).Ascending()
                .OnColumn(nameof(WalletEvent.Status)).Ascending()
                .OnColumn(nameof(WalletEvent.StartOnUtc)).Descending();

            Create.Index("IX_WalletEvent_ReminderOnUtc")
                .OnTable(nameof(WalletEvent))
                .OnColumn(nameof(WalletEvent.ReminderOnUtc)).Ascending();

            Create.Index("IX_WalletEvent_RecurringGroupId")
                .OnTable(nameof(WalletEvent))
                .OnColumn(nameof(WalletEvent.RecurrenceGroupId)).Ascending();

            Create.Index("IX_WalletEvent_ReferenceType_ReferenceId")
                .OnTable(nameof(WalletEvent))
                .OnColumn(nameof(WalletEvent.ReferenceType)).Ascending()
                .OnColumn(nameof(WalletEvent.ReferenceId)).Ascending();
        }

        if (!Schema.Table(nameof(WalletLedgerEntry)).Exists())
        {
            Create.TableFor<WalletLedgerEntry>();
            Create.Index("IX_WalletLedgerEntry_AccountNumber_DrCr")
                .OnTable(nameof(WalletLedgerEntry))
                .OnColumn(nameof(WalletLedgerEntry.AccountNumber)).Ascending()
                .OnColumn(nameof(WalletLedgerEntry.DrCr)).Descending();

            Create.UniqueConstraint("UQ_WalletLedgerEntry_TransactionRef")
                .OnTable(nameof(WalletLedgerEntry))
                .Columns(nameof(WalletLedgerEntry.TRANSACTIONID),
                         nameof(WalletLedgerEntry.Group),
                         nameof(WalletLedgerEntry.Index),
                         nameof(WalletLedgerEntry.DrCr),
                         nameof(WalletLedgerEntry.AccountNumber));

        }


        if (!Schema.Table(nameof(WalletAccountGLs)).Exists())
        {
            Create.TableFor<WalletAccountGLs>();
        }
        if (!Schema.Table(nameof(WalletCatalogGLs)).Exists())
        {
            Create.TableFor<WalletCatalogGLs>();
            Create.UniqueConstraint("UC_WalletCatalogGLs").OnTable(nameof(WalletCatalogGLs)).Columns(nameof(WalletCatalogGLs.CatalogCode), nameof(WalletCatalogGLs.SysAccountName));
        }

        if (!Schema.Table(nameof(WalletCounterparty)).Exists())
        {
            Create.TableFor<WalletCounterparty>();

            // ===== Unique (anti-duplicate in 1 wallet) =====
            Create.UniqueConstraint("UC_WalletCounterparty_WalletId_Phone")
                .OnTable(nameof(WalletCounterparty))
                .Columns(
                    nameof(WalletCounterparty.UserCode),
                    nameof(WalletCounterparty.Phone)
                );

            Create.UniqueConstraint("UC_WalletCounterparty_WalletId_Email")
                .OnTable(nameof(WalletCounterparty))
                .Columns(
                    nameof(WalletCounterparty.UserCode),
                    nameof(WalletCounterparty.Email)
                );

            Create.Index("IX_WalletCounterparty_UserCode_Active_Fav_LastUsed")
                .OnTable(nameof(WalletCounterparty))
                .OnColumn(nameof(WalletCounterparty.UserCode)).Ascending()
                .OnColumn(nameof(WalletCounterparty.IsActive)).Ascending()
                .OnColumn(nameof(WalletCounterparty.IsFavorite)).Descending()
                .OnColumn(nameof(WalletCounterparty.LastUsedOnUtc)).Descending();

            Create.Index("IX_WalletCounterparty_UserCode_Active_UseCount")
                .OnTable(nameof(WalletCounterparty))
                .OnColumn(nameof(WalletCounterparty.UserCode)).Ascending()
                .OnColumn(nameof(WalletCounterparty.IsActive)).Ascending()
                .OnColumn(nameof(WalletCounterparty.UseCount)).Descending();

            Create.Index("IX_WalletCounterparty_UserCode_SearchKey")
                .OnTable(nameof(WalletCounterparty))
                .OnColumn(nameof(WalletCounterparty.UserCode)).Ascending()
                .OnColumn(nameof(WalletCounterparty.SearchKey)).Ascending();
        }
    }
}
