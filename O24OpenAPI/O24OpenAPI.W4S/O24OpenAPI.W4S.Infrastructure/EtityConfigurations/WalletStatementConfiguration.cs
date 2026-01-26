using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;
using O24OpenAPI.W4S.Domain.Constants;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletStatementConfiguration : O24OpenAPIEntityBuilder<WalletStatement>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            // Scope
            .WithColumn(nameof(WalletStatement.WalletId)).AsInt32().NotNullable()
            .WithColumn(nameof(WalletStatement.AccountNumber)).AsString(50).Nullable()

            // Time
            .WithColumn(nameof(WalletStatement.StatementOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(WalletStatement.TransactionOnUtc)).AsDateTime2().Nullable()

            // Ledger
            .WithColumn(nameof(WalletStatement.EntryType)).AsString(20).Nullable() // Debit/Credit/Adjustment (string)
            .WithColumn(nameof(WalletStatement.Amount)).AsDecimal(19, 6).NotNullable()
            .WithColumn(nameof(WalletStatement.CurrencyCode)).AsString(3).NotNullable().WithDefaultValue("VND")
            .WithColumn(nameof(WalletStatement.RunningBalance)).AsDecimal(19, 6).NotNullable()

            .WithColumn(nameof(WalletStatement.OpeningBalance)).AsDecimal(19, 6).Nullable()
            .WithColumn(nameof(WalletStatement.ClosingBalance)).AsDecimal(19, 6).Nullable()

            // Display
            .WithColumn(nameof(WalletStatement.Description)).AsString(500).NotNullable().WithDefaultValue(string.Empty)

            // Classification
            .WithColumn(nameof(WalletStatement.CategoryId)).AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(WalletStatement.EventId)).AsInt32().NotNullable().WithDefaultValue(0)

            // Source & refs
            .WithColumn(nameof(WalletStatement.Source)).AsString(50).Nullable()
            .WithColumn(nameof(WalletStatement.ReferenceType)).AsString(50).Nullable()
            .WithColumn(nameof(WalletStatement.ReferenceId)).AsString(100).Nullable()
            .WithColumn(nameof(WalletStatement.ExternalRef)).AsString(100).Nullable()

            // Reconcile
            .WithColumn(nameof(WalletStatement.IsReconciled)).AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn(nameof(WalletStatement.ReconciledOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(WalletStatement.ReconciledBy)).AsString(50).Nullable()

            // Status
            .WithColumn(nameof(WalletStatement.Status)).AsString(20).NotNullable()
            .WithDefaultValue(WalletStatementStatus.POSTED)

            // Integrity
            .WithColumn(nameof(WalletStatement.EntryHash)).AsString(64).Nullable()
            .WithColumn(nameof(WalletStatement.BlockchainTxHash)).AsString(128).Nullable()
            .WithColumn(nameof(WalletStatement.AnchoredOnUtc)).AsDateTime2().Nullable();
    }
}
