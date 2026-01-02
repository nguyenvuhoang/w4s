using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.CalendarAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.Calendars;

public class CalendarSearchCommand
    : BaseTransactionModel,
        ICommand<PagedListModel<Calendar, CalendarSearchResponseModel>>
{
    public CalendarSearchCommand()
    {
        this.PageIndex = 0;
        this.PageSize = int.MaxValue;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
}

[CqrsHandler]
public class SearchHandle(ICalendarRepository calendarRepository)
    : ICommandHandler<CalendarSearchCommand, PagedListModel<Calendar, CalendarSearchResponseModel>>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_RETRIEVE_CALENDAR)]
    public async Task<PagedListModel<Calendar, CalendarSearchResponseModel>> HandleAsync(
        CalendarSearchCommand request,
        CancellationToken cancellationToken = default
    )
    {
        request.PageSize = request.PageSize == 0 ? int.MaxValue : request.PageSize;
        var calendars = await calendarRepository.GetAllPaged(
            query =>
            {
                if (request.Year != null)
                {
                    query = query.Where(a =>
                        a.SqnDate.Year.ToString().Contains(request.Year.ToString())
                    );
                }
                if (request.Month != null)
                {
                    query = query.Where(a =>
                        a.SqnDate.Month.ToString().Contains(request.Month.ToString())
                    );
                }
                query = query.OrderBy(a => a.Id);
                return query;
            },
            request.PageIndex,
            request.PageSize
        );
        return calendars.ToPagedListModel<Calendar, CalendarSearchResponseModel>();
    }
}
