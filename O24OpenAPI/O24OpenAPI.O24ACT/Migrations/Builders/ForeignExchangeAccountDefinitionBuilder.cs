using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.O24ACT.Domain;

namespace O24OpenAPI.O24ACT.Migrations.Builders;

/// <summary>
/// ForeignExchangeAccountDefinitionBuilder
/// </summary>
public partial class ForeignExchangeAccountDefinitionBuilder : O24OpenAPIEntityBuilder<ForeignExchangeAccountDefinition>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ForeignExchangeAccountDefinition.BranchCode)).AsString(5).NotNullable()
            .WithColumn(nameof(ForeignExchangeAccountDefinition.AccountCurrency)).AsString(3).NotNullable().WithDefaultValue("USD")
            .WithColumn(nameof(ForeignExchangeAccountDefinition.ClearingCurrency)).AsString(3).NotNullable()
            .WithColumn(nameof(ForeignExchangeAccountDefinition.ClearingType)).AsString(1).NotNullable().WithDefaultValue("I")
            .WithColumn(nameof(ForeignExchangeAccountDefinition.AccountName)).AsString(50).Nullable()
            .WithColumn(nameof(ForeignExchangeAccountDefinition.AccountNumber)).AsString(25).NotNullable()
            .WithColumn(nameof(ForeignExchangeAccountDefinition.CreatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(ForeignExchangeAccountDefinition.UpdatedOnUtc)).AsDateTime2().Nullable();
    }
}
