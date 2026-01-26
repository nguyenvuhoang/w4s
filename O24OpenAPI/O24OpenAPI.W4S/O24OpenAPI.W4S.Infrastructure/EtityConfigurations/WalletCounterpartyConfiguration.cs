using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletCounterpartyConfiguration : O24OpenAPIEntityBuilder<WalletCounterparty>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            // ===== Scope =====
            .WithColumn(nameof(WalletCounterparty.UserCode))
                .AsString(100)
                .NotNullable()

            // ===== Identity / Display =====
            .WithColumn(nameof(WalletCounterparty.DisplayName))
                .AsString(200)
                .NotNullable()
                .WithDefaultValue(string.Empty)

            .WithColumn(nameof(WalletCounterparty.Phone))
                .AsString(30)
                .Nullable()

            .WithColumn(nameof(WalletCounterparty.Email))
                .AsString(150)
                .Nullable()

            .WithColumn(nameof(WalletCounterparty.AvatarUrl))
                .AsString(500)
                .Nullable()

            // ===== Type / Note =====
            .WithColumn(nameof(WalletCounterparty.CounterpartyType))
                .AsInt16()
                .NotNullable()
                .WithDefaultValue((short)WalletCounterpartyType.Person)

            .WithColumn(nameof(WalletCounterparty.Note))
                .AsString(500)
                .Nullable()

            // ===== UX / Suggestion =====
            .WithColumn(nameof(WalletCounterparty.IsFavorite))
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(false)

            .WithColumn(nameof(WalletCounterparty.UseCount))
                .AsInt32()
                .NotNullable()
                .WithDefaultValue(0)

            .WithColumn(nameof(WalletCounterparty.LastUsedOnUtc))
                .AsDateTime2()
                .Nullable()

            // ===== Soft flags =====
            .WithColumn(nameof(WalletCounterparty.IsActive))
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(true)

            // ===== Search / Matching =====
            .WithColumn(nameof(WalletCounterparty.SearchKey))
                .AsString(500)
                .Nullable();
    }
}
