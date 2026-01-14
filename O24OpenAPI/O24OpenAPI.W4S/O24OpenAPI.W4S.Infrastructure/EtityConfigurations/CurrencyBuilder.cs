using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.CommonAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class CurrencyBuilder : O24OpenAPIEntityBuilder<Currency>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            // ===== Currency code =====
            .WithColumn(nameof(Currency.CurrencyId))
                .AsString(20)
                .NotNullable()

            .WithColumn(nameof(Currency.ShortCurrencyId))
                .AsString(10)
                .NotNullable()

            // ===== Display names =====
            .WithColumn(nameof(Currency.CurrencyName))
                .AsString(200)
                .NotNullable()

            .WithColumn(nameof(Currency.MasterName))
                .AsString(200)
                .Nullable()

            .WithColumn(nameof(Currency.DecimalName))
                .AsString(200)
                .Nullable()

            // ===== ISO / numeric info =====
            .WithColumn(nameof(Currency.CurrencyNumber))
                .AsInt64()
                .Nullable()

            // ===== Decimal & rounding =====
            .WithColumn(nameof(Currency.DecimalDigits))
                .AsInt32()
                .NotNullable()
                .WithDefaultValue(0)

            .WithColumn(nameof(Currency.RoundingDigits))
                .AsInt32()
                .NotNullable()
                .WithDefaultValue(0)

            // ===== Status & UI =====
            .WithColumn(nameof(Currency.StatusOfCurrency))
                .AsString(50)
                .NotNullable()

            .WithColumn(nameof(Currency.DisplayOrder))
                .AsInt32()
                .Nullable()
            .WithColumn(nameof(Currency.Symbol))
                .AsString(50)
                .Nullable();
    }
}
