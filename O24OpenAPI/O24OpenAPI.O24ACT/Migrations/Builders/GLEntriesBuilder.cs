using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Framework.Domain;

namespace O24OpenAPI.O24ACT.Migrations.Builders;

public class GLEntriesBuilder : O24OpenAPIEntityBuilder<GLEntries>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(GLEntries.TransactionNumber))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(GLEntries.TransTableName))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(GLEntries.TransId))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(GLEntries.SysAccountName))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(GLEntries.GLAccount))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(GLEntries.DorC))
            .AsString(1)
            .NotNullable()
            .WithColumn(nameof(GLEntries.TransactionStatus))
            .AsString(20)
            .Nullable()
            .WithColumn(nameof(GLEntries.Amount))
            .AsDecimal(38, 4)
            .NotNullable()
            .WithDefaultValue(0)
            .WithColumn(nameof(GLEntries.BranchCode))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(GLEntries.CurrencyCode))
            .AsString(3)
            .Nullable()
            .WithColumn(nameof(GLEntries.ValueDate))
            .AsDateTime2()
            .NotNullable()
            .WithColumn(nameof(GLEntries.Posted))
            .AsBoolean()
            .NotNullable()
            .WithDefaultValue(false)
            .WithColumn(nameof(GLEntries.AccountingGroup))
            .AsInt32()
            .NotNullable()
            .WithDefaultValue(1)
            .WithColumn(nameof(GLEntries.CrossBranchCode))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(GLEntries.CrossCurrencyCode))
            .AsString(3)
            .Nullable()
            .WithColumn(nameof(GLEntries.BaseCurrencyAmount))
            .AsDecimal(38, 4)
            .NotNullable()
            .WithDefaultValue(0);
    }
}
