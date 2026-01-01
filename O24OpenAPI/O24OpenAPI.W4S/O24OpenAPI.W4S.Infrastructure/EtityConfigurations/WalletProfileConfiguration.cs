using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletProfileConfiguration : O24OpenAPIEntityBuilder<WalletProfile>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            // ===== Business Fields =====
            .WithColumn(nameof(WalletProfile.WalletId))
                .AsGuid()
                .NotNullable()

            .WithColumn(nameof(WalletProfile.UserCode))
                .AsString(50)
                .NotNullable()

            .WithColumn(nameof(WalletProfile.WalletName))
                .AsString(200)
                .NotNullable()

            .WithColumn(nameof(WalletProfile.WalletType))
                .AsString(20)
                .NotNullable()

            .WithColumn(nameof(WalletProfile.DefaultCurrency))
                .AsString(10)
                .NotNullable()

            .WithColumn(nameof(WalletProfile.Status))
                .AsFixedLengthString(1)
                .NotNullable()
                .WithDefaultValue('A');
    }
}
