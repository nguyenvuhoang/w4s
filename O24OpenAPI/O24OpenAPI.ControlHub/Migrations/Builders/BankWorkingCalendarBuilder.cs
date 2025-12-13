using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

/// <summary>
/// The builder for <see cref="BankWorkingCalendar"/>
/// </summary>
public class BankWorkingCalendarBuilder : O24OpenAPIEntityBuilder<BankWorkingCalendar>
{
    /// <summary>
    /// Maps the entity to its table definition.
    /// </summary>
    /// <param name="table">The FluentMigrator table builder.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BankWorkingCalendar.BankCode)).AsString(50).NotNullable()
            .WithColumn(nameof(BankWorkingCalendar.BranchCode)).AsString(50).Nullable()
            .WithColumn(nameof(BankWorkingCalendar.WorkingDate)).AsDate().NotNullable()
            .WithColumn(nameof(BankWorkingCalendar.BatchDate)).AsDate().Nullable()
            .WithColumn(nameof(BankWorkingCalendar.IsWorkingDay)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(BankWorkingCalendar.IsHoliday)).AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn(nameof(BankWorkingCalendar.HolidayCode)).AsString(50).Nullable()
            .WithColumn(nameof(BankWorkingCalendar.HolidayName)).AsString(200).Nullable()
            .WithColumn(nameof(BankWorkingCalendar.OpenTime)).AsTime().Nullable()
            .WithColumn(nameof(BankWorkingCalendar.CloseTime)).AsTime().Nullable()
            .WithColumn(nameof(BankWorkingCalendar.LunchStartTime)).AsTime().Nullable()
            .WithColumn(nameof(BankWorkingCalendar.LunchEndTime)).AsTime().Nullable()
            .WithColumn(nameof(BankWorkingCalendar.CutoffPaymentTime)).AsTime().Nullable()
            .WithColumn(nameof(BankWorkingCalendar.CutoffTransferTime)).AsTime().Nullable()
            .WithColumn(nameof(BankWorkingCalendar.Timezone)).AsString(100).NotNullable().WithDefaultValue("Asia/Ho_Chi_Minh")
            .WithColumn(nameof(BankWorkingCalendar.CalendarType)).AsString(30).NotNullable().WithDefaultValue("Bank")
            .WithColumn(nameof(BankWorkingCalendar.WeekMask)).AsString(14).Nullable()
            .WithColumn(nameof(BankWorkingCalendar.PreviousWorkingDate)).AsDate().Nullable()
            .WithColumn(nameof(BankWorkingCalendar.NextWorkingDate)).AsDate().Nullable()
            .WithColumn(nameof(BankWorkingCalendar.Status)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(BankWorkingCalendar.Notes)).AsString(500).Nullable()
            .WithColumn(nameof(BankWorkingCalendar.CreatedOnUtc)).AsDateTime2().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn(nameof(BankWorkingCalendar.UpdatedOnUtc)).AsDateTime2().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
    }
}
