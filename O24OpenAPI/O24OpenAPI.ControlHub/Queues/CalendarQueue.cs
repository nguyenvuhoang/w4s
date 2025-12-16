using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Services.Queue;

namespace O24OpenAPI.ControlHub.Queues;

public class CalendarQueue : BaseQueue
{
    private readonly ICalendarService _calendarService =
        EngineContext.Current.Resolve<ICalendarService>();

    public async Task<WFScheme> Search(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<CalendarSearchModel>();
        return await Invoke<CalendarSearchModel>(
            wfScheme,
            async () =>
            {
                var calendars = await _calendarService.Search(model);
                var items = calendars.ToPagedListModel<Calendar, CalendarSearchResponseModel>();
                return items;
            }
        );
    }
}
