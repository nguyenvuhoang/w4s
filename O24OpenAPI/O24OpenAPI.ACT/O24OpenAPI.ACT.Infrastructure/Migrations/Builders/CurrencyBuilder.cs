using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.ACT.Domain.AggregatesModel.CommonAggregate;

namespace O24OpenAPI.ACT.Migrations.Builders;


/// <summary>
/// Currency builder
/// </summary>
	public partial class CurrencyBuilder : O24OpenAPIEntityBuilder<Currency>
{

    /// <summary>
    /// mapping entity currency table
    /// </summary>
    /// <param name="table"></param>
		public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Currency.CurrencyId)).AsString(3).NotNullable()
            .WithColumn(nameof(Currency.ShortCurrencyId)).AsString(2).NotNullable()
            .WithColumn(nameof(Currency.CurrencyName)).AsString(500).Nullable()
            .WithColumn(nameof(Currency.CurrencyNumber)).AsInt64().Nullable()
            .WithColumn(nameof(Currency.MasterName)).AsString(500).Nullable()
            .WithColumn(nameof(Currency.DecimalName)).AsString(500).Nullable()
            .WithColumn(nameof(Currency.DecimalDigits)).AsInt32().NotNullable()
            .WithColumn(nameof(Currency.RoundingDigits)).AsInt32().NotNullable()
            .WithColumn(nameof(Currency.StatusOfCurrency)).AsString(1).NotNullable()
            .WithColumn(nameof(Currency.DisplayOrder)).AsInt32().Nullable()
            .WithColumn(nameof(Currency.UpdatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(Currency.CreatedOnUtc)).AsDateTime2().Nullable();
    }
}
