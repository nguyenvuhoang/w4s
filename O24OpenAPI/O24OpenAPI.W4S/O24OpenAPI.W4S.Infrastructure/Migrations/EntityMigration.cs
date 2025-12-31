using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Framework.Domain;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Migrations;

[O24OpenAPIMigration(
    "2025/12/31 06:01:00:0000000",
    "6. Create SchemeMigration (Business Table)",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]

public class EntityMigration : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(WalletAccount)).Exists())
        {
            Create.TableFor<WalletAccount>();

            Create.Index("IDX_WalletAccount_WalletId")
                .OnTable(nameof(WalletAccount))
                .OnColumn(nameof(WalletAccount.WalletId))
                .Ascending().WithOptions().NonClustered();

            Create.Index("IDX_WalletAccount_AccountNumber")
                .OnTable(nameof(WalletAccount))
                .OnColumn(nameof(WalletAccount.AccountNumber))
                .Ascending().WithOptions().NonClustered();

            Create.UniqueConstraint("UQ_WalletAccount_Wallet_Account")
                .OnTable(nameof(WalletAccount))
                .Columns(nameof(WalletAccount.WalletId), nameof(WalletAccount.AccountNumber));
        }

        if (!Schema.Table(nameof(WalletBudget)).Exists())
        {
            Create.TableFor<WalletBudget>();

            Create.Index("IDX_WalletBudget_WalletId")
                .OnTable(nameof(WalletBudget))
                .OnColumn(nameof(WalletBudget.WalletId))
                .Ascending().WithOptions().NonClustered();

            Create.Index("IDX_WalletBudget_CategoryId")
                .OnTable(nameof(WalletBudget))
                .OnColumn(nameof(WalletBudget.CategoryId))
                .Ascending().WithOptions().NonClustered();

            Create.Index("IDX_WalletBudget_Period")
                .OnTable(nameof(WalletBudget))
                .OnColumn(nameof(WalletBudget.StartDate)).Ascending()
                .OnColumn(nameof(WalletBudget.EndDate)).Ascending()
                .WithOptions().NonClustered();

            Create.UniqueConstraint("UQ_WalletBudget_Wallet_Category_Period")
                .OnTable(nameof(WalletBudget))
                .Columns(
                    nameof(WalletBudget.WalletId),
                    nameof(WalletBudget.CategoryId),
                    nameof(WalletBudget.StartDate),
                    nameof(WalletBudget.EndDate)
                );
        }

        if (!Schema.Table(nameof(WalletGoal)).Exists())
        {
            Create.TableFor<WalletGoal>();

            Create.Index("IDX_WalletGoal_WalletId")
                .OnTable(nameof(WalletGoal))
                .OnColumn(nameof(WalletGoal.WalletId))
                .Ascending().WithOptions().NonClustered();

            Create.Index("IDX_WalletGoal_TargetDate")
                .OnTable(nameof(WalletGoal))
                .OnColumn(nameof(WalletGoal.TargetDate))
                .Ascending().WithOptions().NonClustered();

            Create.UniqueConstraint("UQ_WalletGoal_Wallet_GoalName")
                .OnTable(nameof(WalletGoal))
                .Columns(
                    nameof(WalletGoal.WalletId),
                    nameof(WalletGoal.GoalName)
                );
        }

        if (!Schema.Table(nameof(WalletBalance)).Exists())
        {
            Create.TableFor<WalletBalance>();

            Create.UniqueConstraint("UQ_WalletBalance_AccountNumber")
                .OnTable(nameof(WalletBalance))
                .Columns(nameof(WalletBalance.AccountNumber));

            Create.Index("IDX_WalletBalance_AvailableBalance")
                .OnTable(nameof(WalletBalance))
                .OnColumn(nameof(WalletBalance.AvailableBalance))
                .Descending().WithOptions().NonClustered();
        }

        if (!Schema.Table(nameof(WalletCategory)).Exists())
        {
            Create.TableFor<WalletCategory>();

            Create.Index("IDX_WalletCategory_WalletId")
                .OnTable(nameof(WalletCategory))
                .OnColumn(nameof(WalletCategory.WalletId))
                .Ascending().WithOptions().NonClustered();

            Create.Index("IDX_WalletCategory_ParentCategoryId")
                .OnTable(nameof(WalletCategory))
                .OnColumn(nameof(WalletCategory.ParentCategoryId))
                .Ascending().WithOptions().NonClustered();

            Create.Index("IDX_WalletCategory_CategoryType")
                .OnTable(nameof(WalletCategory))
                .OnColumn(nameof(WalletCategory.CategoryType))
                .Ascending().WithOptions().NonClustered();

            Create.UniqueConstraint("UQ_WalletCategory_Wallet_Type_Name")
                .OnTable(nameof(WalletCategory))
                .Columns(
                    nameof(WalletCategory.WalletId),
                    nameof(WalletCategory.CategoryType),
                    nameof(WalletCategory.CategoryName)
                );
        }


        if (!Schema.Table(nameof(WalletTransaction)).Exists())
        {
            Create.TableFor<WalletTransaction>();

            // Indexes
            Create.Index("IDX_TransactionHistory_UserID")
                  .OnTable(nameof(WalletTransaction))
                  .OnColumn(nameof(WalletTransaction.UserId)).Ascending().WithOptions().NonClustered();

            Create.Index("WalletTransaction")
                  .OnTable(nameof(WalletTransaction))
                  .OnColumn(nameof(WalletTransaction.SourceId)).Ascending().WithOptions().NonClustered();

            Create.Index("IDX_TransactionHistory_TransactionCode")
                  .OnTable(nameof(WalletTransaction))
                  .OnColumn(nameof(WalletTransaction.TransactionCode)).Ascending().WithOptions().NonClustered();

            Create.Index("IDX_TransactionHistory_Status")
                  .OnTable(nameof(WalletTransaction))
                  .OnColumn(nameof(WalletTransaction.Status)).Ascending().WithOptions().NonClustered();

            Create.Index("IDX_TransactionHistory_WorkDate")
                  .OnTable(nameof(WalletTransaction))
                  .OnColumn(nameof(WalletTransaction.TransactionWorkDate)).Descending().WithOptions().NonClustered();
        }

        if (!Schema.Table(nameof(C_CODELIST)).Exists())
        {
            Create.TableFor<C_CODELIST>();
            Create
                .UniqueConstraint("UC_C_CODELIST_CodeId_CodeName_CodeGroup")
                .OnTable(nameof(C_CODELIST))
                .Columns(
                    nameof(C_CODELIST.CodeId),
                    nameof(C_CODELIST.CodeName),
                    nameof(C_CODELIST.CodeGroup)
                );
        }
    }
}
