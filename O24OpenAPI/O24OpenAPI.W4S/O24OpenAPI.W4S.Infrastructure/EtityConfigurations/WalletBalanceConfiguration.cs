using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletBalanceConfiguration : O24OpenAPIEntityBuilder<WalletBalance>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WalletBalance.AccountNumber))
                .AsString(50)
                .NotNullable()

            .WithColumn(nameof(WalletBalance.Balance))
                .AsDecimal(18, 2)
                .NotNullable()
                .WithDefaultValue(0)

            .WithColumn(nameof(WalletBalance.BonusBalance))
                .AsDecimal(18, 2)
                .NotNullable()
                .WithDefaultValue(0)

            .WithColumn(nameof(WalletBalance.LockedBalance))
                .AsDecimal(18, 2)
                .NotNullable()
                .WithDefaultValue(0)

            .WithColumn(nameof(WalletBalance.AvailableBalance))
                .AsDecimal(18, 2)
                .NotNullable()
                .WithDefaultValue(0);
    }
}
