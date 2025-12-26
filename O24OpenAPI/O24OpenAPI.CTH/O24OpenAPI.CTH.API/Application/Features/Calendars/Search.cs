using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.CalendarAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.Calendars;

public class SearchCommand
    : BaseTransactionModel,
        ICommand<PagedListModel<Calendar, CalendarSearchResponseModel>>
{
    public CalendarSearchModel Model { get; set; } = default!;
}

[CqrsHandler]
public class SearchHandle(ICalendarRepository calendarRepository)
    : ICommandHandler<SearchCommand, PagedListModel<Calendar, CalendarSearchResponseModel>>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_RETRIEVE_CALENDAR)]
    public async Task<PagedListModel<Calendar, CalendarSearchResponseModel>> HandleAsync(
        SearchCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var calendars = await Search(request.Model);
        return calendars.ToPagedListModel<Calendar, CalendarSearchResponseModel>();
    }

    public virtual async Task<IPagedList<Calendar>> Search(CalendarSearchModel model)
    {
        model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;
        var calendars = await calendarRepository.GetAllPaged(
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
}
