using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.O24ACT.Domain;

namespace O24OpenAPI.O24ACT.Migrations.Builders;

/// <summary>
/// AccountClearingBuilder
/// </summary>
public partial class AccountClearingBuilder : O24OpenAPIEntityBuilder<AccountClearing>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AccountClearing.BranchCode)).AsString(5).NotNullable()
            .WithColumn(nameof(AccountClearing.CurrencyId)).AsString(3).NotNullable()
            .WithColumn(nameof(AccountClearing.ClearingBranchCode)).AsString(5).NotNullable()
            .WithColumn(nameof(AccountClearing.ClearingType)).AsString(1).NotNullable()
            .WithColumn(nameof(AccountClearing.AccountName)).AsString(50).Nullable()
            .WithColumn(nameof(AccountClearing.AccountNumber)).AsString(25).NotNullable()
            .WithColumn(nameof(AccountClearing.CreatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountClearing.UpdatedOnUtc)).AsDateTime2().Nullable();
    }
}
