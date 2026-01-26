using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletGoalConfiguration : O24OpenAPIEntityBuilder<WalletGoal>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WalletGoal.WalletId))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WalletGoal.GoalName))
            .AsString(200)
            .NotNullable()
            .WithColumn(nameof(WalletGoal.TargetAmount))
            .AsDecimal(18, 2)
            .Nullable()
            .WithColumn(nameof(WalletGoal.CurrentAmount))
            .AsDecimal(18, 2)
            .Nullable()
            .WithDefaultValue(0)
            .WithColumn(nameof(WalletGoal.TargetDate))
            .AsDateTime()
            .Nullable();
    }
}
