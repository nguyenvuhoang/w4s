using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Framework.Domain;
using O24OpenAPI.ACT.Domain;
using O24OpenAPI.ACT.Domain.AggregatesModel.CommonAggregate;
using O24OpenAPI.ACT.Domain.AggregatesModel.MappingAggregate;
using O24OpenAPI.ACT.Domain.AggregatesModel.RulesAggregate;
using O24OpenAPI.ACT.Domain.AggregatesModel.TransactionAggregate;
using O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

namespace O24OpenAPI.ACT.Migrations;

[O24OpenAPIMigration(
    "2025/10/11 14:07:00:0000000",
    "2. Initial accounting table",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(AccountBalance)).Exists())
        {
            Create.TableFor<AccountBalance>();
            Create
                .Index("IX_AccountBalance_AccountNumber")
                .OnTable(nameof(AccountBalance))
                .OnColumn(nameof(AccountBalance.AccountNumber))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(AccountChart)).Exists())
        {
            Create.TableFor<AccountChart>();
            Create
                .Index("IX_AccountChart_AccountNumber")
                .OnTable(nameof(AccountChart))
                .OnColumn(nameof(AccountChart.AccountNumber))
                .Ascending()
                .WithOptions()
                .NonClustered();
            Create
                .Index("IX_AccountChart_Lookup")
                .OnTable(nameof(AccountChart))
                .OnColumn(nameof(AccountChart.AccountLevel))
                .Ascending()
                .OnColumn(nameof(AccountChart.AccountNumber))
                .Ascending()
                .OnColumn(nameof(AccountChart.CurrencyCode))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }
        if (!Schema.Table(nameof(AccountClearing)).Exists())
        {
            Create.TableFor<AccountClearing>();
            Create
                .UniqueConstraint("UC_AccountClearing")
                .OnTable(nameof(AccountClearing))
                .Columns(
                    nameof(AccountClearing.BranchCode),
                    nameof(AccountClearing.CurrencyId),
                    nameof(AccountClearing.ClearingBranchCode),
                    nameof(AccountClearing.ClearingType)
                );
        }
        if (!Schema.Table(nameof(AccountCommon)).Exists())
        {
            Create.TableFor<AccountCommon>();
            Create
                .Index("IX_AccountCommon_AccountName")
                .OnTable(nameof(AccountCommon))
                .OnColumn(nameof(AccountCommon.AccountName))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }
        if (!Schema.Table(nameof(AccountingTransaction)).Exists())
        {
            Create.TableFor<AccountingTransaction>();
            Create
                .UniqueConstraint("UC_AccountingTransaction")
                .OnTable(nameof(AccountingTransaction))
                .Columns(
                    nameof(AccountingTransaction.ReferenceId),
                    nameof(AccountingTransaction.TransactionDate)
                );
        }
        if (!Schema.Table(nameof(AccountMapping)).Exists())
        {
            Create.TableFor<AccountMapping>();
            Create
                .UniqueConstraint("UC_AccountMapping")
                .OnTable(nameof(AccountMapping))
                .Columns(
                    nameof(AccountMapping.MappingId),
                    nameof(AccountMapping.MappingTableName),
                    nameof(AccountMapping.MappingType)
                );
        }
        if (!Schema.Table(nameof(AccountMappingDetail)).Exists())
        {
            Create.TableFor<AccountMappingDetail>();
            Create
                .UniqueConstraint("UC_AccountMappingDetail")
                .OnTable(nameof(AccountMappingDetail))
                .Columns(
                    nameof(AccountMappingDetail.MappingId),
                    nameof(AccountMappingDetail.SystemAccountNumber)
                );
        }

        if (!Schema.Table(nameof(CheckingAccountRules)).Exists())
        {
            Create.TableFor<CheckingAccountRules>();
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
        if (!Schema.Table(nameof(Currency)).Exists())
        {
            Create.TableFor<Currency>();
            Create
                .Index("IX_Currency_CurrencyId")
                .OnTable(nameof(Currency))
                .OnColumn(nameof(Currency.CurrencyId))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }
        if (!Schema.Table(nameof(RuleDefinition)).Exists())
        {
            Create.TableFor<RuleDefinition>();
        }
        if (!Schema.Table(nameof(ForeignExchangeAccountDefinition)).Exists())
        {
            Create.TableFor<ForeignExchangeAccountDefinition>();
            Create
                .UniqueConstraint("UC_ForeignExchangeAccountDefinition")
                .OnTable(nameof(ForeignExchangeAccountDefinition))
                .Columns(
                    nameof(ForeignExchangeAccountDefinition.BranchCode),
                    nameof(ForeignExchangeAccountDefinition.AccountCurrency),
                    nameof(ForeignExchangeAccountDefinition.ClearingCurrency),
                    nameof(ForeignExchangeAccountDefinition.ClearingType)
                );
        }

        if (!Schema.Table(nameof(GLEntries)).Exists())
        {
            Create.TableFor<GLEntries>();
            Create
                .Index("IX_GLEntries_TransactionNumber")
                .OnTable(nameof(GLEntries))
                .OnColumn(nameof(GLEntries.TransactionNumber))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IX_GLEntries_GLAccount")
                .OnTable(nameof(GLEntries))
                .OnColumn(nameof(GLEntries.GLAccount))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(GLEntriesDone)).Exists())
        {
            Create.TableFor<GLEntriesDone>();
            Create
                .Index("IX_GLEntriesDone_TransactionNumber")
                .OnTable(nameof(GLEntriesDone))
                .OnColumn(nameof(GLEntriesDone.TransactionNumber))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IX_GLEntriesDone_GLAccount")
                .OnTable(nameof(GLEntriesDone))
                .OnColumn(nameof(GLEntriesDone.GLAccount))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        if (!Schema.Table(nameof(TransactionAction)).Exists())
        {
            Create.TableFor<TransactionAction>();
        }
        if (!Schema.Table(nameof(MasterMapping)).Exists())
        {
            Create.TableFor<MasterMapping>();

            Create
                .Index("IX_MasterMapping_GLConfigClass")
                .OnTable(nameof(MasterMapping))
                .OnColumn(nameof(MasterMapping.GLConfigClass))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IX_MasterMapping_MasterTransClass")
                .OnTable(nameof(MasterMapping))
                .OnColumn(nameof(MasterMapping.MasterTransClass))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IX_MasterMapping_GLEntriesClass")
                .OnTable(nameof(MasterMapping))
                .OnColumn(nameof(MasterMapping.GLEntriesClass))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }
        if (!Schema.Table(nameof(AccountStatement)).Exists())
        {
            Create.TableFor<AccountStatement>();
        }
        if (!Schema.Table(nameof(AccountStatementDone)).Exists())
        {
            Create.TableFor<AccountStatementDone>();
        }

        if (!Schema.Table(nameof(AccountingRuleDefinition)).Exists())
        {
            Create.TableFor<AccountingRuleDefinition>();
            Create
                .UniqueConstraint("UC_AccountingRuleDefinition")
                .OnTable(nameof(AccountingRuleDefinition))
                .Columns(
                    nameof(AccountingRuleDefinition.TransactionCode),
                    nameof(AccountingRuleDefinition.AccountingEntryGroup),
                    nameof(AccountingRuleDefinition.AccountingEntryIndex)
                );
        }
    }
}
