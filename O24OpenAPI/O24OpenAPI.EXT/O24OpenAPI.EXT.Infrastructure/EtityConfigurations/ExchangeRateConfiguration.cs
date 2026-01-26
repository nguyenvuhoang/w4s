using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.EXT.Domain.AggregatesModel.ExchangeRateAggregate;

namespace O24OpenAPI.EXT.Infrastructure.EtityConfigurations;

public class ExchangeRateConfiguration : O24OpenAPIEntityBuilder<ExchangeRate>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            // ===== Rate info =====
            .WithColumn(nameof(ExchangeRate.RateDateUtc))
                .AsDateTime()
                .NotNullable()

            // ===== Currency info =====
            .WithColumn(nameof(ExchangeRate.CurrencyCode))
                .AsString(10)
                .NotNullable()

            .WithColumn(nameof(ExchangeRate.CurrencyName))
                .AsString(100)
                .NotNullable()

            // ===== Exchange values =====
            .WithColumn(nameof(ExchangeRate.Buy))
                .AsDecimal(18, 4)
                .Nullable()

            .WithColumn(nameof(ExchangeRate.Transfer))
                .AsDecimal(18, 4)
                .Nullable()

            .WithColumn(nameof(ExchangeRate.Sell))
                .AsDecimal(18, 4)
                .Nullable()

            // ===== Metadata =====
            .WithColumn(nameof(ExchangeRate.Source))
                .AsString(200)
                .Nullable();
    }
}
