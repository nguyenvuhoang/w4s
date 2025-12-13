using FluentMigrator;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Migrations;

namespace O24OpenAPI.ControlHub.Migrations;

/// <summary>
/// Migration to populate Calendar table with dates and flags
/// </summary>
/// <param name="dataProvider"></param>
[O24OpenAPIMigration("2025/09/26 13:00:00:0000000",
    "CalendarMigrations - 2024/02/25 03:00:00:0000000",
    MigrationProcessType.Installation)]
[Environment(EnvironmentType.All)]
public class CalendarMigrations(IO24OpenAPIDataProvider dataProvider) : AutoReversingMigration
{
    private readonly IO24OpenAPIDataProvider _dataProvider = dataProvider;

    /// <summary>
    /// Up migration
    /// </summary>
    public override void Up()
    {
        DateTime startDate = DateTime.Parse("2025-01-01 00:00:00");

        List<Calendar> listCalendar = [];

        for (int i = 0; i <= 50 * 365; i++)
        {
            var calendar = new Calendar
            {
                SqnDate = startDate.AddDays(i),
                IsCurrentDate = startDate.AddDays(i).Date == DateTime.Now.Date ? 1 : 0,
                IsHoliday = startDate.AddDays(i).DayOfWeek == DayOfWeek.Sunday || startDate.AddDays(i).DayOfWeek == DayOfWeek.Saturday ? 1 : 0,
                IsEndOfWeek = startDate.AddDays(i).DayOfWeek == DayOfWeek.Sunday ? 1 : 0,
                IsEndOfMonth = startDate.AddDays(i).Month < startDate.AddDays(i + 1).Month ? 1 : 0,
                IsEndOfQuater = "3|6|9|12".Equals(startDate.AddDays(i).Month.ToString()) && startDate.AddDays(i).Month < startDate.AddDays(i + 1).Month ? 1 : 0,
                IsEndOfHalfYear = startDate.AddDays(i).Month == 6 && startDate.AddDays(i).Month < startDate.AddDays(i + 1).Month ? 1 : 0,
                IsEndOfYear = startDate.AddDays(i).Year < startDate.AddDays(i + 1).Year ? 1 : 0,
                IsBeginOfWeek = startDate.AddDays(i).DayOfWeek == DayOfWeek.Monday ? 1 : 0,
                IsBeginOfMonth = startDate.AddDays(i).Day == 1 ? 1 : 0,
                IsBeginOfQuater = "3|6|9|12".Equals(startDate.AddDays(i).Month.ToString()) && startDate.AddDays(i).Day == 1 ? 1 : 0,
                IsBeginOfHalfYear = startDate.AddDays(i).Month == 6 && startDate.AddDays(i).Day == 1 ? 1 : 0,
                IsBeginOfYear = startDate.AddDays(i).Month == 1 && startDate.AddDays(i).Day == 1 ? 1 : 0,
                CurrencyCode = "LAK"
            };
            listCalendar.Add(calendar);
        }
        if (listCalendar.Count > 0)
        {
            _dataProvider.BulkInsertEntities(listCalendar).Wait();
        }

    }
}
