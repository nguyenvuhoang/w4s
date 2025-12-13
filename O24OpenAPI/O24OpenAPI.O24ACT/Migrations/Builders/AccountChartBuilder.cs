using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.O24ACT.Domain;

namespace O24OpenAPI.O24ACT.Migrations.Builders;

/// <summary>
/// AccountChartBuilder
/// </summary>
public partial class AccountChartBuilder : O24OpenAPIEntityBuilder<AccountChart>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AccountChart.AccountNumber)).AsString(25).NotNullable()
            .WithColumn(nameof(AccountChart.CurrencyCode)).AsString(3).Nullable()
            .WithColumn(nameof(AccountChart.BranchCode)).AsString(5).Nullable()
            .WithColumn(nameof(AccountChart.ParentAccountId)).AsString(25).Nullable()
            .WithColumn(nameof(AccountChart.AccountLevel)).AsInt16().NotNullable().WithDefaultValue(1)
            .WithColumn(nameof(AccountChart.IsAccountLeave)).AsBoolean().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountChart.BalanceSide)).AsString(1).NotNullable().WithDefaultValue("B")
            .WithColumn(nameof(AccountChart.ReverseBalance)).AsString(1).NotNullable().WithDefaultValue("N")
            .WithColumn(nameof(AccountChart.PostingSide)).AsString(1).NotNullable().WithDefaultValue("A")
            .WithColumn(nameof(AccountChart.AccountName)).AsString(500).NotNullable()
            .WithColumn(nameof(AccountChart.ShortAccountName)).AsString(500).NotNullable()
            .WithColumn(nameof(AccountChart.MultiValueName)).AsString(2000).Nullable()
            .WithColumn(nameof(AccountChart.AccountClassification)).AsString(1).NotNullable().WithDefaultValue("A")
            .WithColumn(nameof(AccountChart.AccountCategories)).AsString(1).NotNullable().WithDefaultValue("N")
            .WithColumn(nameof(AccountChart.AccountGroup)).AsString(1).NotNullable().WithDefaultValue("N")
            .WithColumn(nameof(AccountChart.DirectPosting)).AsString(1).NotNullable().WithDefaultValue("N")
            .WithColumn(nameof(AccountChart.IsVisible)).AsString(1).NotNullable().WithDefaultValue("N")
            .WithColumn(nameof(AccountChart.IsMultiCurrency)).AsString(1).NotNullable().WithDefaultValue("N")
            .WithColumn(nameof(AccountChart.JobProcessOption)).AsString(2).NotNullable().WithDefaultValue("N")
            .WithColumn(nameof(AccountChart.RefAccountNumber)).AsString(100).Nullable()
            .WithColumn(nameof(AccountChart.ReferencesNumber)).AsString(300).Nullable()
            .WithColumn(nameof(AccountChart.CreatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountChart.UpdatedOnUtc)).AsDateTime2().Nullable();
    }
}
