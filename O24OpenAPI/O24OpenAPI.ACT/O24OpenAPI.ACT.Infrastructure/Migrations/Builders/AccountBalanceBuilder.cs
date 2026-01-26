using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

namespace O24OpenAPI.ACT.Migrations.Builders;

/// <summary>
/// AccountBalanceBuilder
/// </summary>
public partial class AccountBalanceBuilder : O24OpenAPIEntityBuilder<AccountBalance>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AccountBalance.AccountNumber)).AsString(25).NotNullable()
            .WithColumn(nameof(AccountBalance.Balance)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.DailyDebit)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.DailyCredit)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.WeeklyDebit)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.WeeklyCredit)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.MonthlyDebit)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.MonthlyCredit)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.QuarterlyDebit)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.QuarterlyCredit)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.HalfYearlyDebit)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.HalfYearlyCredit)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.YearlyDebit)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.YearlyCredit)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.WeekAverageBalance)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.MonthAverageBalance)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.QuarterAverageBalance)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.HalfYearAverageBalance)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.YearAverageBalance)).AsDecimal(38, 4).NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(AccountBalance.CreatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountBalance.UpdatedOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(AccountBalance.BranchCode)).AsString(10).Nullable()
            .WithColumn(nameof(AccountBalance.CurrencyCode)).AsString(3).Nullable();
    }
}
