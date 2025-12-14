using LinqToDB;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Services.Interfaces;

namespace O24OpenAPI.ControlHub.Services;

public class BankService(IRepository<BankWorkingCalendar> bankWorkingCalendarRepository) : IBankService
{
    private readonly IRepository<BankWorkingCalendar> _bankWorkingCalendarRepository = bankWorkingCalendarRepository;

    /// <summary>
    /// inBatch = true  => trả BatchDate (nếu null thì dùng WorkingDate)
    /// inBatch = false => trả WorkingDate
    /// </summary>
    public virtual DateTime GetWorkingDate(bool inBatch = false)
    {
        var calendar = _bankWorkingCalendarRepository.Table
            .Where(x => x.Status)
            .OrderByDescending(x => x.WorkingDate)
            .FirstOrDefault() ?? throw new InvalidOperationException("No active BankWorkingCalendar record found.");
        if (inBatch)
        {
            var effectiveBatch = calendar.BatchDate ?? calendar.WorkingDate;
            return effectiveBatch.Date;
        }

        return calendar.WorkingDate.Date;
    }

    public async Task UpdateWorkingDateTripletAsync(DateTime newWorkingDate)
    {
        newWorkingDate = newWorkingDate.Date;

        var currentRow = _bankWorkingCalendarRepository.Table
            .Where(x => x.Status)
            .OrderByDescending(x => x.WorkingDate)
            .FirstOrDefault() ?? throw new InvalidOperationException("No active BankWorkingCalendar record found for update.");

        var previousWorkingDate = await _bankWorkingCalendarRepository.Table
            .Where(x => x.Status && x.IsWorkingDay)
            .Where(x => x.WorkingDate < newWorkingDate)
            .OrderByDescending(x => x.WorkingDate)
            .Select(x => (DateTime?)x.WorkingDate)
            .FirstOrDefaultAsync();

        var nextWorkingDate = await _bankWorkingCalendarRepository.Table
            .Where(x => x.Status && x.IsWorkingDay)
            .Where(x => x.WorkingDate > newWorkingDate)
            .OrderBy(x => x.WorkingDate)
            .Select(x => (DateTime?)x.WorkingDate)
            .FirstOrDefaultAsync();

        await _bankWorkingCalendarRepository.Table
            .Where(x => x.Id == currentRow.Id)
            .Set(x => x.WorkingDate, newWorkingDate)
            .Set(x => x.PreviousWorkingDate, previousWorkingDate)
            .Set(x => x.NextWorkingDate, nextWorkingDate)
            .Set(x => x.UpdatedOnUtc, DateTime.UtcNow)
            .UpdateAsync();
    }

}
