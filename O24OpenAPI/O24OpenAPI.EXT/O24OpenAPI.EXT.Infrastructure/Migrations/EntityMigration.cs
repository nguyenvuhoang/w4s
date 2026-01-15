using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.EXT.Domain.AggregatesModel.ExchangeRateAggregate;

namespace O24OpenAPI.EXT.Infrastructure.Migrations;


/// <summary>
/// The schema migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2026/01/15 15:01:00:0000000",
    "Table For ExchangeRate",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
[DatabaseType(DataProviderType.SqlServer)]
public class EntityMigration : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(ExchangeRate)).Exists())
        {
            Create.TableFor<ExchangeRate>();

            Create
                .UniqueConstraint("UX_EXCHANGERATE_RATEDATE_CURRENCY")
                .OnTable(nameof(ExchangeRate))
                .Columns(
                    nameof(ExchangeRate.RateDateUtc),
                    nameof(ExchangeRate.CurrencyCode)
                );

            Create
                .Index("IX_EXCHANGERATE_RATEDATE")
                .OnTable(nameof(ExchangeRate))
                .OnColumn(nameof(ExchangeRate.RateDateUtc))
                .Descending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IX_EXCHANGERATE_CURRENCY")
                .OnTable(nameof(ExchangeRate))
                .OnColumn(nameof(ExchangeRate.CurrencyCode))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }
    }
}
