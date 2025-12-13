using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.Core;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

/// <summary>
/// calendar service interface
/// </summary>
public partial interface ICalendarService
{

    /// <summary>
    /// get a calendar by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Calendar> GetById(int id);

    /// <summary>
    /// Simple search model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<Calendar>> Search(SimpleSearchModel model);


    /// <summary>
    /// advanced search calendar
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<Calendar>> Search(CalendarSearchModel model);


    /// <summary>
    /// create a calendar
    /// </summary>
    /// <param name="calendar"></param>
    /// <returns></returns>
    Task<Calendar> Create(Calendar calendar);

    /// <summary>
    /// Insert
    /// </summary>
    /// <param name="calendars"></param>
    /// <returns></returns>
    Task Insert(List<Calendar> calendars);


    /// <summary>
    /// Update a calendar
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task Update(Calendar calendar, string referenceId = "");


    /// <summary>
    /// Delete a caledar
    /// </summary>
    /// <param name="calendarId"></param>
    /// <returns></returns>
    Task Delete(int calendarId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="holidayMoveOn"></param>
    /// <param name="toDate"></param>
    /// <returns></returns>
    Task<DateTime> CreditMaturityDate(DateTime date, int holidayMoveOn, DateTime toDate);

    /// <summary>
    /// AddNewYear
    /// </summary>
    /// <returns></returns>
    Task<IPagedList<Calendar>> AddNewYear(int year);

    /// <summary>
    /// GetByDate
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    Task<Calendar> GetByDate(DateTime date);

    /// <summary>
    /// Get current calendar
    /// </summary>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    Task<Calendar> GetCurrentCalendar(string currencyCode);

    /// <summary>
    /// Get the calendar by date
    /// </summary>
    /// <param name="date"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    Task<Calendar> GetCalendar(DateTime date, string currencyCode);

    /// <summary>
    /// Get next date
    /// </summary>
    /// <param name="date"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    Task<Calendar> GetNextCalendar(DateTime date, string currencyCode);

    /// <summary>
    /// Change working date
    /// </summary>
    Task ChangeWorkingDate(DateTime workingDate);

    /// <summary>
    /// Change working date
    /// </summary>
    Task ChangeWorkingDate();

    /// <summary>
    /// Get List Year
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<GetListYearResponseModel>> GetListYear(SimpleSearchModel model);

    /// <summary>
    /// GetSequenceDateByDays
    /// </summary>
    /// <param name="days"></param>
    /// <returns></returns>
    Task<Calendar> GetSequenceDateByDays(int days);

    /// <summary>
    /// NextDueDate
    /// </summary>
    /// <param name="date"></param>
    /// <param name="tenor"></param>
    /// <param name="count"></param>
    /// <param name="tenornun"></param>
    /// <returns></returns>
    DateTime NextDueDate(DateTime date, int tenor, int count, string tenornun);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tenor"></param>
    /// <param name="tenornun"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <param name="holidayMoveOn"></param>
    /// <param name="beginOfTennor"></param>
    /// <returns></returns>
    Task<List<DueCountAndCreditMaturityDateModelResponse>> DueCountAndCreditMaturityDate(int tenor, string tenornun, DateTime fromDate, DateTime toDate, int holidayMoveOn, DateTime beginOfTennor);


    /// <summary>
    /// Is holiday
    /// </summary>
    /// <param name="date"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    Task<bool> IsHoliday(DateTime date, string currencyCode);
}