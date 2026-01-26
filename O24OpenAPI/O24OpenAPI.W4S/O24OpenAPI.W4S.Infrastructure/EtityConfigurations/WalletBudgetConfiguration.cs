using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletBudgetConfiguration : O24OpenAPIEntityBuilder<WalletBudget>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WalletBudget.BudgetCode))
                .AsString(50)
                .NotNullable()

            .WithColumn(nameof(WalletBudget.WalletId))
                .AsInt32()
                .NotNullable()

            .WithColumn(nameof(WalletBudget.CategoryId))
                .AsString(50)
                .NotNullable()

            .WithColumn(nameof(WalletBudget.Amount))
                .AsDecimal(18, 2)
                .NotNullable()
                .WithDefaultValue(0)

            .WithColumn(nameof(WalletBudget.SourceBudget))
                .AsString(200)
                .Nullable()

            .WithColumn(nameof(WalletBudget.SourceTracker))
                .AsInt32()
                .Nullable()

            .WithColumn(nameof(WalletBudget.PeriodType))
                .AsString(30)
                .Nullable()

            .WithColumn(nameof(WalletBudget.StartDate))
                .AsDateTime()
                .NotNullable()

            .WithColumn(nameof(WalletBudget.EndDate))
                .AsDateTime()
                .NotNullable()
            .WithColumn(nameof(WalletBudget.IncludeInReport))
                .AsBoolean()
                .Nullable()
                .WithDefaultValue(true)
            .WithColumn(nameof(WalletBudget.IsAutoRepeat))
                .AsBoolean()
                .Nullable()
                .WithDefaultValue(false)
            .WithColumn(nameof(WalletBudget.Note))
                .AsString(int.MaxValue)
                .Nullable();
    }
}
