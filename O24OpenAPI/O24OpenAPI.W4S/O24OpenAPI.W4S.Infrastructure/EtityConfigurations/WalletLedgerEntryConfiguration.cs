using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletLedgerEntryConfiguration : O24OpenAPIEntityBuilder<WalletLedgerEntry>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            // ===== Business Fields =====
            .WithColumn(nameof(WalletLedgerEntry.TRANSACTIONID))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(WalletLedgerEntry.AccountNumber))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(WalletLedgerEntry.Group))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WalletLedgerEntry.Index))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WalletLedgerEntry.Amount))
            .AsDecimal(18, 6)
            .NotNullable()
            .WithColumn(nameof(WalletLedgerEntry.Currency))
            .AsString(10)
            .NotNullable()
            .WithColumn(nameof(WalletLedgerEntry.DrCr))
            .AsString(2)
            .NotNullable()
            .WithColumn(nameof(WalletLedgerEntry.PostingDateUtc))
            .AsDateTime2()
            .NotNullable()
            .WithColumn(nameof(WalletLedgerEntry.EntryType))
            .AsString(50)
            .NotNullable();
    }
}
