using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Data;
using O24OpenAPI.CTH.Domain.AggregatesModel.CalendarAggregate;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class CalendarRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<Calendar>(eventPublisher, dataProvider, staticCacheManager),
        ICalendarRepository
{

public async Task<bool> AnyAsync()
{
    return await Table.AnyAsync();
}

public async Task<DateTime?> GetMaxSqnDateAsync()
{
    return await Table.Select(c => (DateTime?)c.SqnDate).MaxAsync();
}

public async Task<Calendar?> GetCurrentCalendarAsync(string currencyCode)
{
    return await Table
        .Where(c => c.IsCurrentDate == 1 && c.CurrencyCode == currencyCode)
        .FirstOrDefaultAsync();
}

public async Task<Calendar?> GetCalendarAsync(DateTime date, string currencyCode)
{
    return await Table
        .Where(c => c.SqnDate == date && c.CurrencyCode == currencyCode)
        .FirstOrDefaultAsync();
}

public async Task<Calendar?> GetNextCalendarAfterAsync(DateTime date, string currencyCode)
{
    return await Table
        .Where(c => c.SqnDate > date && c.CurrencyCode == currencyCode)
        .OrderBy(c => c.SqnDate)
        .FirstOrDefaultAsync();
}

public async Task<List<int>> GetDistinctYearsAsync()
{
    return await Table.Select(c => c.SqnDate.Year).Distinct().ToListAsync();
}

public async Task<Calendar?> GetSequenceDateByDaysAsync(DateTime busDate, int days, string currencyCode)
{
    return await Table
        .Where(c =>
            c.SqnDate.Date >= busDate.Date.AddDays(-365)
            && c.SqnDate.Date <= busDate.Date.AddDays(-days)
            && c.IsHoliday == 0
            && c.CurrencyCode == currencyCode
        )
        .OrderByDescending(x => x.SqnDate)
        .FirstOrDefaultAsync();
}
}
