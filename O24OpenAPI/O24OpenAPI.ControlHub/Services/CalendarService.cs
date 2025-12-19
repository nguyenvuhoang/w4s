using O24OpenAPI.ControlHub.Config;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.ControlHub.Utils;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Services;

/// <summary>
/// Calendar services
/// </summary>
/// <remarks>
/// Calendar service constructor
/// </remarks>
/// <param name="calendarRepository"></param>
/// <param name="adminSetting"></param>
/// <param name="bankService"></param>
/// <param name="settingService"></param>
/// <param name="localizationService"></param>
/// <param name="logger"></param>
public partial class CalendarService(
    IRepository<Calendar> calendarRepository,
    ControlHubSetting adminSetting,
    IBankService bankService,
    ILocalizationService localizationService
) : ICalendarService
{
    #region  Fields
    private readonly IRepository<Calendar> _calendarRepository = calendarRepository;
    private readonly ControlHubSetting _adminSetting = adminSetting;
    private readonly ILocalizationService _localizationService = localizationService;
    private readonly IBankService _bankService = bankService;

    #endregion
    #region Ctor

    #endregion

    /// <summary>
    /// Get a calendar by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<Calendar> GetById(int id)
    {
        return await _calendarRepository.GetById(id);
    }

    /// <summary>
    /// Simple search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<Calendar>> Search(SimpleSearchModel model)
    {
        model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;
        var calendars = await _calendarRepository.GetAllPaged(
            query =>
            {
                query = query.OrderBy(a => a.Id);
                return query;
            },
            model.PageIndex,
            model.PageSize
        );
        return calendars;
    }

    /// <summary>
    /// Advanced search calendar
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<Calendar>> Search(CalendarSearchModel model)
    {
        model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;
        var calendars = await _calendarRepository.GetAllPaged(
            query =>
            {
                if (model.Year != null)
                {
                    query = query.Where(a =>
                        a.SqnDate.Year.ToString().Contains(model.Year.ToString())
                    );
                }
                if (model.Month != null)
                {
                    query = query.Where(a =>
                        a.SqnDate.Month.ToString().Contains(model.Month.ToString())
                    );
                }
                query = query.OrderBy(a => a.Id);
                return query;
            },
            model.PageIndex,
            model.PageSize
        );
        return calendars;
    }

    /// <summary>
    /// Create a calendar
    /// </summary>
    /// <param name="calendar"></param>
    /// <returns></returns>
    public virtual async Task<Calendar> Create(Calendar calendar)
    {
        await _calendarRepository.Insert(calendar);
        return calendar;
    }

    /// <summary>
    /// Update a calendar
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    public virtual async Task Update(Calendar calendar, string referenceId = "")
    {
        await _calendarRepository.Update(calendar, referenceId);
    }

    /// <summary>
    /// Insert
    /// </summary>
    /// <param name="calendars"></param>
    /// <returns></returns>
    public virtual async Task Insert(List<Calendar> calendars)
    {
        await _calendarRepository.BulkInsert(calendars);
    }

    /// <summary>
    /// Delete a calendar
    /// </summary>
    /// <param name="calendarId"></param>
    /// <returns></returns>
    public virtual async Task Delete(int calendarId)
    {
        var calendar = await GetById(calendarId);
        if (calendar == null)
        {
            throw new O24OpenAPIException(
                await _localizationService.GetResource("Admin.Calendar.Value.NotFound")
            );
        }

        await _calendarRepository.Delete(calendar);
    }

    /// <summary>
    /// CreditMaturityDate
    /// </summary>
    /// <param name="date"></param>
    /// <param name="holidayMoveOn"></param>
    /// <param name="toDate"></param>
    /// <returns></returns>
    public virtual async Task<DateTime> CreditMaturityDate(
        DateTime date,
        int holidayMoveOn,
        DateTime toDate
    )
    {
        if (holidayMoveOn == 0)
        {
            return date;
        }

        var returnDay = date;
        var query = await _calendarRepository.Table.Where(c => c.SqnDate == date).ToListAsync();
        var count = query.Count;
        if (count == 0)
        {
            if (returnDay == toDate)
            {
                if (returnDay == toDate)
                {
                    returnDay = returnDay.AddDays(1);
                    while (
                        returnDay.DayOfWeek == DayOfWeek.Saturday
                        || returnDay.DayOfWeek == DayOfWeek.Sunday
                    )
                    {
                        returnDay = returnDay.AddDays(-1);
                    }
                }
            }
            else
            {
                if (holidayMoveOn < 0)
                {
                    while (
                        returnDay.DayOfWeek == DayOfWeek.Saturday
                        || returnDay.DayOfWeek == DayOfWeek.Sunday
                    )
                    {
                        returnDay = returnDay.AddDays(-1);
                    }
                }
                else if (holidayMoveOn > 0)
                {
                    while (
                        returnDay.DayOfWeek == DayOfWeek.Saturday
                        || returnDay.DayOfWeek == DayOfWeek.Sunday
                    )
                    {
                        returnDay = returnDay.AddDays(1);
                    }
                    if (returnDay == toDate)
                    {
                        returnDay = returnDay.AddDays(-1);
                        while (
                            returnDay.DayOfWeek == DayOfWeek.Saturday
                            || returnDay.DayOfWeek == DayOfWeek.Sunday
                        )
                        {
                            returnDay = returnDay.AddDays(-1);
                        }
                    }
                }
            }
        }
        else
        {
            var query2 = await _calendarRepository
                .Table.Where(c => c.IsHoliday == 0 && c.SqnDate <= date)
                .ToListAsync();
            if (date == toDate)
            {
                returnDay = (
                    await _calendarRepository
                        .Table.Where(c => c.IsHoliday == 0 && c.SqnDate <= date)
                        .Select(c => c.SqnDate)
                        .ToListAsync()
                ).Max();
            }
            else
            {
                if (holidayMoveOn < 0)
                {
                    returnDay = (
                        await _calendarRepository
                            .Table.Where(c => c.IsHoliday == 0 && c.SqnDate <= date)
                            .Select(c => c.SqnDate)
                            .ToListAsync()
                    ).Max();
                }
                else if (holidayMoveOn > 0)
                {
                    returnDay = (
                        await _calendarRepository
                            .Table.Where(c => c.IsHoliday == 0 && c.SqnDate >= date)
                            .Select(c => c.SqnDate)
                            .ToListAsync()
                    ).Min();
                }
            }
        }
        return returnDay;
    }

    /// <summary>
    /// AddNewYear
    /// </summary>
    /// <returns></returns>
    public async Task<IPagedList<Calendar>> AddNewYear(int year)
    {
        var pageIndex = 0;
        var pageSize = Int32.MaxValue;
        var currencyCode = _adminSetting.BaseCurrency;

        if (year == 0)
        {
            year = DateTime
                .ParseExact(
                    _adminSetting.Currdate,
                    "d/M/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture
                )
                .Year;
        }

        var maxDate = new DateTime(year - 1, 12, 31);

        if (await _calendarRepository.Table.AnyAsync())
        {
            maxDate = _calendarRepository.Table.Max(c => c.SqnDate);
        }
        maxDate = maxDate.AddDays(1);

        var listCalendars = new List<Calendar>() { };
        do
        {
            var newCalendar = new Calendar();
            newCalendar.SqnDate = maxDate;
            newCalendar.CurrencyCode = currencyCode;
            if (maxDate.DayOfWeek == DayOfWeek.Saturday || maxDate.DayOfWeek == DayOfWeek.Sunday)
            {
                newCalendar.IsHoliday = 1;
            }

            if (maxDate.DayOfWeek == DayOfWeek.Sunday)
            {
                newCalendar.IsEndOfWeek = 1;
            }

            if (maxDate.AddDays(1).Day == 1)
            {
                newCalendar.IsEndOfMonth = 1;
            }

            if (
                maxDate.ToString("dd/MM") == "31/03"
                || maxDate.ToString("dd/MM") == "30/06"
                || maxDate.ToString("dd/MM") == "30/09"
                || maxDate.ToString("dd/MM") == "31/12"
            )
            {
                newCalendar.IsEndOfQuater = 1;
            }

            if (maxDate.ToString("dd/MM") == "30/06" || maxDate.ToString("dd/MM") == "31/12")
            {
                newCalendar.IsEndOfHalfYear = 1;
            }

            if (maxDate.ToString("dd/MM") == "31/12")
            {
                newCalendar.IsEndOfYear = 1;
            }

            if (maxDate.DayOfWeek == DayOfWeek.Monday)
            {
                newCalendar.IsBeginOfWeek = 1;
            }

            if (maxDate.Day == 1)
            {
                newCalendar.IsBeginOfMonth = 1;
            }

            if (
                maxDate.AddDays(-1).ToString("dd/MM") == "31/03"
                || maxDate.AddDays(-1).ToString("dd/MM") == "30/06"
                || maxDate.AddDays(-1).ToString("dd/MM") == "30/09"
                || maxDate.AddDays(-1).ToString("dd/MM") == "31/12"
            )
            {
                newCalendar.IsBeginOfQuater = 1;
            }

            if (
                maxDate.AddDays(-1).ToString("dd/MM") == "30/06"
                || maxDate.AddDays(-1).ToString("dd/MM") == "31/12"
            )
            {
                newCalendar.IsBeginOfHalfYear = 1;
            }

            if (maxDate.AddDays(-1).ToString("dd/MM") == "31/12")
            {
                newCalendar.IsBeginOfYear = 1;
            }

            listCalendars.Add(newCalendar);
            maxDate = maxDate.AddDays(1);
        } while (maxDate.ToString("dd/MM") != "01/01");

        await Insert(listCalendars);
        var pagedListCalendars = await listCalendars.AsQueryable().ToPagedList(pageIndex, pageSize);
        return pagedListCalendars;
    }

    /// <summary>
    /// GetByDate
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public virtual async Task<Calendar> GetByDate(DateTime date)
    {
        return await _calendarRepository
            .Table.Where(c => c.SqnDate.Date == date.Date)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="dt"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    private async Task<Calendar> CreateIfNotExists(
        Calendar calendar,
        DateTime dt,
        string currencyCode
    )
    {
        if (calendar is null)
        {
            calendar = new Calendar
            {
                SqnDate = dt,
                CurrencyCode = currencyCode,
                IsHoliday =
                    (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                        ? 1
                        : 0,
                IsEndOfWeek = dt.DayOfWeek == DayOfWeek.Sunday ? 1 : 0,
                IsEndOfMonth = dt.AddDays(1).Day == 1 ? 1 : 0,
                IsEndOfQuater =
                    dt.ToString("dd/MM") == "31/03"
                    || dt.ToString("dd/MM") == "30/06"
                    || dt.ToString("dd/MM") == "30/09"
                    || dt.ToString("dd/MM") == "31/12"
                        ? 1
                        : 0,
                IsEndOfHalfYear =
                    dt.ToString("dd/MM") == "30/06" || dt.ToString("dd/MM") == "31/12" ? 1 : 0,
                IsEndOfYear = dt.ToString("dd/MM") == "31/12" ? 1 : 0,
                IsBeginOfWeek = dt.DayOfWeek == DayOfWeek.Monday ? 1 : 0,
                IsBeginOfMonth = dt.Day == 1 ? 1 : 0,
                IsBeginOfQuater =
                    dt.AddDays(-1).ToString("dd/MM") == "31/03"
                    || dt.AddDays(-1).ToString("dd/MM") == "30/06"
                    || dt.AddDays(-1).ToString("dd/MM") == "30/09"
                    || dt.AddDays(-1).ToString("dd/MM") == "31/12"
                        ? 1
                        : 0,
                IsBeginOfHalfYear =
                    dt.AddDays(-1).ToString("dd/MM") == "30/06"
                    || dt.AddDays(-1).ToString("dd/MM") == "31/12"
                        ? 1
                        : 0,
                IsBeginOfYear = dt.AddDays(-1).ToString("dd/MM") == "31/12" ? 1 : 0,
            };
            calendar = await Create(calendar);
        }
        return calendar;
    }

    /// <summary>
    /// Get current calendar
    /// </summary>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    public virtual async Task<Calendar> GetCurrentCalendar(string currencyCode)
    {
        if (string.IsNullOrEmpty(currencyCode))
        {
            currencyCode = _adminSetting.BaseCurrency;
        }

        var query = _calendarRepository.Table;
        query = query.Where(c => c.IsCurrentDate == 1 && c.CurrencyCode == currencyCode);

        var calendar = await query.FirstOrDefaultAsync();
        return await CreateIfNotExists(calendar, _bankService.GetWorkingDate().Date, currencyCode);
    }

    /// <summary>
    /// Get the calendar by date
    /// </summary>
    /// <param name="date"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    public virtual async Task<Calendar> GetCalendar(DateTime date, string currencyCode)
    {
        if (string.IsNullOrEmpty(currencyCode))
        {
            currencyCode = _adminSetting.BaseCurrency;
        }

        var query = _calendarRepository.Table;
        query = query.Where(c => c.SqnDate == date && c.CurrencyCode == currencyCode);

        var calendar = await query.FirstOrDefaultAsync();
        return await CreateIfNotExists(calendar, date, currencyCode);
    }

    /// <summary>
    /// Get next date
    /// </summary>
    /// <param name="date"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    public virtual async Task<Calendar> GetNextCalendar(DateTime date, string currencyCode)
    {
        if (string.IsNullOrEmpty(currencyCode))
        {
            currencyCode = _adminSetting.BaseCurrency;
        }

        //ensure calendar exist
        var c = await GetCalendar(date.AddDays(1).Date, currencyCode);

        var query = _calendarRepository.Table;
        query = query.Where(c => c.SqnDate > date && c.CurrencyCode == currencyCode);
        query = query.OrderBy(c => c.SqnDate);

        var calendar = await query.FirstOrDefaultAsync();
        return await CreateIfNotExists(calendar, date.AddDays(1).Date, currencyCode);
    }

    /// <summary>
    /// Change working date
    /// </summary>
    public virtual async Task ChangeWorkingDate(DateTime workingDate)
    {
        try
        {
            await _bankService.UpdateWorkingDateTripletAsync(workingDate);
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync(ex.Message);
        }
    }

    /// <summary>
    /// Change working date
    /// </summary>
    public virtual async Task ChangeWorkingDate()
    {
        try
        {
            var baseCurrency = _adminSetting.BaseCurrency;
            var currentCalendar = await GetCurrentCalendar(baseCurrency);
            await ChangeWorkingDate(currentCalendar.SqnDate.AddDays(1));
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync(ex.Message);
        }
    }

    /// <summary>
    /// Get Distinct List Year
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<GetListYearResponseModel>> GetListYear(
        SimpleSearchModel model
    )
    {
        model.PageSize = (model.PageSize == 0) ? int.MaxValue : model.PageSize;
        var listYear = await (
            from calendar in _calendarRepository.Table
            select new GetListYearResponseModel() { Caption = calendar.SqnDate.Year }
        )
            .Distinct()
            .ToPagedList(model.PageIndex, model.PageSize);
        return listYear;
    }

    /// <summary>
    /// GetSequenceDateByDays
    /// </summary>
    /// <param name="days"></param>
    /// <returns></returns>
    public virtual async Task<Calendar> GetSequenceDateByDays(int days)
    {
        DateTime busDate = ParamAdmin.GetBusDate();
        var calendar = await _calendarRepository
            .Table.Where(c =>
                (c.SqnDate.Date >= busDate.Date.AddDays(-365))
                && (c.SqnDate.Date <= busDate.Date.AddDays(-days))
                && c.IsHoliday == 0
                && c.CurrencyCode == _adminSetting.BaseCurrency
            )
            .OrderByDescending(x => x.SqnDate)
            .FirstOrDefaultAsync();
        return calendar ?? new Calendar();
    }

    /// <summary>
    /// Next Due Date
    /// </summary>
    /// <param name="date"></param>
    /// <param name="tenor"></param>
    /// <param name="tenornun"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public DateTime NextDueDate(DateTime date, int tenor, int count, string tenornun)
    {
        DateTime? duedate;
        switch (tenornun)
        {
            case CreditTenorType.Day:
                duedate = date.AddDays(tenor);
                break;
            case CreditTenorType.Week:
                duedate = date.AddDays(7 * tenor);
                break;
            case CreditTenorType.Month:
                duedate = date.AddMonths(count * tenor);
                break;
            case CreditTenorType.Quarter:
                duedate = date.AddMonths(count * tenor * 3);
                break;
            case CreditTenorType.HalfYear:
                duedate = date.AddMonths(count * tenor * 6);
                break;
            case CreditTenorType.Year:
                duedate = date.AddMonths(count * tenor * 12);
                break;
            default:
                duedate = null;
                break;
        }
        return (DateTime)duedate;
    }

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
    public async Task<
        List<DueCountAndCreditMaturityDateModelResponse>
    > DueCountAndCreditMaturityDate(
        int tenor,
        string tenornun,
        DateTime fromDate,
        DateTime toDate,
        int holidayMoveOn,
        DateTime beginOfTennor
    )
    {
        int count = 0;
        DateTime date = fromDate;
        DateTime predate = date;
        DateTime creditMaturityDateOfPredate = date;
        DateTime creditMaturityDate = (await CreditMaturityDate(fromDate, holidayMoveOn, toDate));
        DateTime creditMaturityToDate =
            holidayMoveOn > 0
                ? (await CreditMaturityDate(toDate, -1, toDate))
                : (await CreditMaturityDate(toDate, holidayMoveOn, toDate));
        var listDate = new List<DateTime>();
        var response = new List<DueCountAndCreditMaturityDateModelResponse>();

        if (tenornun == CreditTenorType.Bullet || tenornun == CreditTenorType.Any)
        {
            listDate.Add(creditMaturityToDate);
        }
        else
        {
            if (tenor == 0)
            {
                throw new O24OpenAPIException(
                    await _localizationService.GetResource("Credit.SystemError")
                );
            }
            else
            {
                int countTemp = 0;
                if (creditMaturityDate.Date <= beginOfTennor.Date)
                {
                    predate = date;
                    date = NextDueDate(fromDate, tenor, 1, tenornun);
                    creditMaturityDate = (
                        await CreditMaturityDate(
                            (new DateTime(Math.Min(date.Ticks, toDate.Ticks))),
                            holidayMoveOn,
                            toDate
                        )
                    );
                    creditMaturityDateOfPredate = (
                        await CreditMaturityDate(predate, holidayMoveOn, toDate)
                    );
                    countTemp = 1;
                }
                while (date <= toDate)
                {
                    count++;
                    listDate.Add(creditMaturityDate);
                    predate = date;
                    date = NextDueDate(fromDate, tenor, count + countTemp, tenornun);
                    creditMaturityDate = (
                        await CreditMaturityDate(
                            (new DateTime(Math.Min(date.Ticks, toDate.Ticks))),
                            holidayMoveOn,
                            toDate
                        )
                    );
                    creditMaturityDateOfPredate = (
                        await CreditMaturityDate(predate, holidayMoveOn, toDate)
                    );
                }
                if (
                    date > toDate
                    && predate < toDate
                    && creditMaturityDate > creditMaturityDateOfPredate
                )
                {
                    count++;
                    listDate.Add(creditMaturityDate);
                }
            }
        }
        count = 0;
        foreach (var d in listDate.Distinct().OrderBy(d => d))
        {
            count++;
            response.Add(
                new DueCountAndCreditMaturityDateModelResponse()
                {
                    DueCount = count,
                    CreditMaturityDate = d,
                }
            );
        }
        // Check if Last due
        return response;
    }

    /// <summary>
    /// Is holiday
    /// </summary>
    /// <param name="date"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    public async Task<bool> IsHoliday(DateTime date, string currencyCode)
    {
        var cal = await _calendarRepository
            .Table.Where(x => x.SqnDate.Date == date.Date && x.CurrencyCode == currencyCode)
            .FirstOrDefaultAsync();
        return Convert.ToBoolean(cal?.IsHoliday);
    }
}
