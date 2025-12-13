using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.DataWarehouse.Domain;

namespace O24OpenAPI.DataWarehouse.Migrations.Builders;

/// <summary>
/// Stock Transaction builder
/// </summary>
public partial class AccountStatementDoneBuilder : O24OpenAPIEntityBuilder<AccountStatementDone>
{
    /// <summary>
    /// Map entity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AccountStatementDone.AccountNumber)).AsString(25).NotNullable()
            .WithColumn(nameof(AccountStatementDone.StatementDate)).AsDateTime2().NotNullable()
            .WithColumn(nameof(AccountStatementDone.ReferenceId)).AsString(36).NotNullable()
            .WithColumn(nameof(AccountStatementDone.ValueDate)).AsDateTime2().NotNullable()
            .WithColumn(nameof(AccountStatementDone.Amount)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountStatementDone.CurrencyCode)).AsString(3).NotNullable()
            .WithColumn(nameof(AccountStatementDone.ConvertAmount)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountStatementDone.StatementCode)).AsString(5).NotNullable()
            .WithColumn(nameof(AccountStatementDone.StatementStatus)).AsString(1).NotNullable()
            .WithColumn(nameof(AccountStatementDone.RefNumber)).AsString(25).Nullable()
            .WithColumn(nameof(AccountStatementDone.TransCode)).AsString(20).Nullable()
            .WithColumn(nameof(AccountStatementDone.Description)).AsString(250).Nullable()
            .WithColumn(nameof(AccountStatementDone.CreatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountStatementDone.UpdatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountStatementDone.TransactionNumber)).AsString(100).Nullable();
    }
}
