using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.ACT.Domain.AggregatesModel.TransactionAggregate;

namespace O24OpenAPI.ACT.Migrations.Builders;

/// <summary>
/// AccountingTransactionBuilder
/// </summary>
public partial class AccountingTransactionBuilder : O24OpenAPIEntityBuilder<AccountingTransaction>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AccountingTransaction.ReferenceId)).AsString(36).NotNullable()
            .WithColumn(nameof(AccountingTransaction.TransactionDate)).AsDateTime2().NotNullable()
            .WithColumn(nameof(AccountingTransaction.UserDefined1)).AsString(30).Nullable()
            .WithColumn(nameof(AccountingTransaction.UserDefined2)).AsString(30).Nullable()
            .WithColumn(nameof(AccountingTransaction.UserDefined3)).AsString(30).Nullable()
            .WithColumn(nameof(AccountingTransaction.UserDefined4)).AsString(30).Nullable()
            .WithColumn(nameof(AccountingTransaction.UserDefined5)).AsString(30).Nullable()
            .WithColumn(nameof(AccountingTransaction.CreatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountingTransaction.UpdatedOnUtc)).AsDateTime2().Nullable();
    }
}
