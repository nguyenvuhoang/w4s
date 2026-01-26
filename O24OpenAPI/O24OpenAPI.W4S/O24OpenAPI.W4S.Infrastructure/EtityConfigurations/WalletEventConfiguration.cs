using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.Constants;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletEventConfiguration : O24OpenAPIEntityBuilder<WalletEvent>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WalletEvent.WalletId)).AsInt32().NotNullable()

            .WithColumn(nameof(WalletEvent.Title)).AsString(200).NotNullable().WithDefaultValue(string.Empty)
            .WithColumn(nameof(WalletEvent.Description)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(WalletEvent.Location)).AsString(250).Nullable()
            .WithColumn(nameof(WalletEvent.Color)).AsString(30).Nullable()
            .WithColumn(nameof(WalletEvent.Icon)).AsString(100).Nullable()

            .WithColumn(nameof(WalletEvent.StartOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(WalletEvent.EndOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(WalletEvent.IsAllDay)).AsBoolean().NotNullable().WithDefaultValue(false)

            .WithColumn(nameof(WalletEvent.EventType)).AsString(20).NotNullable()
            .WithColumn(nameof(WalletEvent.Status)).AsString(20).NotNullable().WithDefaultValue(WalletEventStatus.ACTIVE)

            .WithColumn(nameof(WalletEvent.PlannedAmount)).AsDecimal(19, 6).Nullable()
            .WithColumn(nameof(WalletEvent.CurrencyCode)).AsString(3).NotNullable().WithDefaultValue("VND")
            .WithColumn(nameof(WalletEvent.CategoryId)).AsInt32().Nullable()
            .WithColumn(nameof(WalletEvent.BudgetId)).AsInt32().Nullable()

            .WithColumn(nameof(WalletEvent.ReminderMinutes)).AsInt32().Nullable()
            .WithColumn(nameof(WalletEvent.ReminderOnUtc)).AsDateTime2().Nullable()

            .WithColumn(nameof(WalletEvent.IsRecurring)).AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn(nameof(WalletEvent.RecurrenceRule)).AsString(500).Nullable()
            .WithColumn(nameof(WalletEvent.RecurrenceGroupId)).AsString(50).Nullable()

            .WithColumn(nameof(WalletEvent.ReferenceType)).AsString(50).Nullable()
            .WithColumn(nameof(WalletEvent.ReferenceId)).AsString(100).Nullable();
    }
}
