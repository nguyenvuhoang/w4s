using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.DataWarehouse.Domain;

namespace O24OpenAPI.DataWarehouse.Migrations.Builders;

/// <summary>
/// AccountStatementBuilder
/// </summary>
public partial class AccountStatementBuilder : O24OpenAPIEntityBuilder<AccountStatement>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AccountStatement.AccountNumber)).AsString(25).NotNullable()
            .WithColumn(nameof(AccountStatement.StatementDate)).AsDateTime2().NotNullable()
            .WithColumn(nameof(AccountStatement.ReferenceId)).AsString(36).NotNullable()
            .WithColumn(nameof(AccountStatement.ValueDate)).AsDateTime2().NotNullable()
            .WithColumn(nameof(AccountStatement.Amount)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountStatement.CurrencyCode)).AsString(3).NotNullable()
            .WithColumn(nameof(AccountStatement.ConvertAmount)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountStatement.StatementCode)).AsString(5).NotNullable()
            .WithColumn(nameof(AccountStatement.StatementStatus)).AsString(1).NotNullable()
            .WithColumn(nameof(AccountStatement.RefNumber)).AsString(25).Nullable()
            .WithColumn(nameof(AccountStatement.TransCode)).AsString(20).Nullable()
            .WithColumn(nameof(AccountStatement.Description)).AsString(250).Nullable()
            .WithColumn(nameof(AccountStatement.CreatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountStatement.UpdatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountStatement.TransactionNumber)).AsString(100).Nullable();
    }
}
