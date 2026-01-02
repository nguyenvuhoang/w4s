using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.ACT.Domain.AggregatesModel.TransactionAggregate;

namespace O24OpenAPI.ACT.Migrations.Builders;

public class GLEntriesDoneBuilder : O24OpenAPIEntityBuilder<GLEntriesDone>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(GLEntriesDone.TransactionNumber)).AsString(50).NotNullable()
            .WithColumn(nameof(GLEntriesDone.TransTableName)).AsString(100).NotNullable()
            .WithColumn(nameof(GLEntriesDone.TransId)).AsString(50).NotNullable()
            .WithColumn(nameof(GLEntriesDone.SysAccountName)).AsString(100).Nullable()
            .WithColumn(nameof(GLEntriesDone.GLAccount)).AsString(50).NotNullable()
            .WithColumn(nameof(GLEntriesDone.DorC)).AsString(1).NotNullable()
            .WithColumn(nameof(GLEntriesDone.TransactionStatus)).AsString(20).Nullable()
            .WithColumn(nameof(GLEntriesDone.Amount)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(GLEntriesDone.BranchCode)).AsString(10).Nullable()
            .WithColumn(nameof(GLEntriesDone.CurrencyCode)).AsString(3).Nullable()
            .WithColumn(nameof(GLEntriesDone.ValueDate)).AsDateTime2().NotNullable()
            .WithColumn(nameof(GLEntriesDone.Posted)).AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn(nameof(GLEntriesDone.AccountingGroup)).AsInt32().NotNullable().WithDefaultValue(1)
            .WithColumn(nameof(GLEntriesDone.CrossBranchCode)).AsString(10).Nullable()
            .WithColumn(nameof(GLEntriesDone.CrossCurrencyCode)).AsString(3).Nullable()
            .WithColumn(nameof(GLEntriesDone.BaseCurrencyAmount)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0);
    }
}
