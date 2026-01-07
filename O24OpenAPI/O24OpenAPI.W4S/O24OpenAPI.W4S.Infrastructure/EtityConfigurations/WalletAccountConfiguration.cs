using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletAccountConfiguration : O24OpenAPIEntityBuilder<WalletAccount>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WalletAccount.AccountNumber))
                .AsString(50)
                .NotNullable()

            .WithColumn(nameof(WalletAccount.WalletId))
                .AsInt32()
                .NotNullable()

            .WithColumn(nameof(WalletAccount.AccountType))
                .AsString(20)
                .NotNullable()

            .WithColumn(nameof(WalletAccount.CurrencyCode))
                .AsString(10)
                .NotNullable()

            .WithColumn(nameof(WalletAccount.IsPrimary))
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(false)

            .WithColumn(nameof(WalletAccount.Status))
                .AsString(20)
                .NotNullable();
    }
}
