using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.CalendarAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class BankWorkingCalendarRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<BankWorkingCalendar>(dataProvider, staticCacheManager),
        IBankWorkingCalendarRepository
{
    public async Task<BankWorkingCalendar?> GetLatestActiveAsync()
    {
        return await Table
            .Where(x => x.Status)
            .OrderByDescending(x => x.WorkingDate)
            .FirstOrDefaultAsync();
    }

    public async Task<DateTime?> GetPreviousWorkingDateAsync(DateTime newWorkingDate)
    {
        newWorkingDate = newWorkingDate.Date;
        return await Table
            .Where(x => x.Status && x.IsWorkingDay && x.WorkingDate < newWorkingDate)
            .OrderByDescending(x => x.WorkingDate)
            .Select(x => (DateTime?)x.WorkingDate)
            .FirstOrDefaultAsync();
    }

    public async Task<DateTime?> GetNextWorkingDateAsync(DateTime newWorkingDate)
    {
        newWorkingDate = newWorkingDate.Date;
        return await Table
            .Where(x => x.Status && x.IsWorkingDay && x.WorkingDate > newWorkingDate)
            .OrderBy(x => x.WorkingDate)
            .Select(x => (DateTime?)x.WorkingDate)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateWorkingDateTripletAsync(
        long id,
        DateTime newWorkingDate,
        DateTime? previousWorkingDate,
        DateTime? nextWorkingDate
    )
    {
        newWorkingDate = newWorkingDate.Date;
        await Table
            .Where(x => x.Id == id)
            .Set(x => x.WorkingDate, newWorkingDate)
            .Set(x => x.PreviousWorkingDate, previousWorkingDate)
            .Set(x => x.NextWorkingDate, nextWorkingDate)
            .Set(x => x.UpdatedOnUtc, DateTime.UtcNow)
            .UpdateAsync();
    }
}
