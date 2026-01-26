using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Utils;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Models.Response;
using O24OpenAPI.NCH.API.Application.Utils;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.Notifications;

public class AdvanceSearchNotificationCommand
    : BaseSearch,
        ICommand<NotificationSearchResponseModel>
{
    public string UserCode { get; set; }
    public string AppType { get; set; }
    public string NotificationType { get; set; }
}

public class NotificationSearchResponseModel : BaseO24OpenAPIModel
{
    public int TotalCount { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }
    public List<NotificationSearchResponse> Items { get; set; }

    public NotificationSearchResponseModel() { }

    public NotificationSearchResponseModel(
        List<NotificationSearchResponse> list,
        IPagedList<Notification> pageList
    )
    {
        TotalCount = pageList.TotalCount;
        TotalPages = pageList.TotalPages;
        HasPreviousPage = pageList.HasPreviousPage;
        HasNextPage = pageList.HasNextPage;
        Items = list;
    }
}

[CqrsHandler]
public class AdvanceSearchNotificationHandler(
    INotificationRepository notificationRepository,
    INotificationTemplateRepository notificationTemplateRepository
) : ICommandHandler<AdvanceSearchNotificationCommand, NotificationSearchResponseModel>
{
    [WorkflowStep(WorkflowStepCode.NCH.WF_STEP_NCH_GET_NOTIFICATIONS)]
    public async Task<NotificationSearchResponseModel> HandleAsync(
        AdvanceSearchNotificationCommand request,
        CancellationToken cancellationToken = default
    )
    {
        IPagedList<Notification> paged = await Search(request);
        if (paged == null || paged.Count == 0)
        {
            return new NotificationSearchResponseModel([], paged);
        }

        List<string> templateIds = paged.Select(x => x.TemplateID).Distinct().ToList();
        IReadOnlyList<NotificationTemplate> templates = await notificationTemplateRepository.GetByTemplateIdsAsync(templateIds);
        Dictionary<string, NotificationTemplate> templateMap = templates.ToDictionary(t => t.TemplateID);

        var language = string.IsNullOrWhiteSpace(request.Language)
            ? "en"
            : request.Language.Trim().ToLowerInvariant();
        List<NotificationSearchResponse> list = new(paged.Count);

        foreach (Notification item in paged)
        {
            NotificationSearchResponse obj = new(item);

            if (
                templateMap.TryGetValue(item.TemplateID, out NotificationTemplate template)
                && template?.Body != null
            )
            {
                obj.IsShowButton = template.IsShowButton;
                obj.Message =
                    template.Body.GetMessageNotification(obj.Data, language)
                    ?? template.Body.GetMessageNotification(obj.Data, "en")
                    ?? string.Empty;
            }
            else
            {
                obj.IsShowButton = false;
                obj.Message = string.Empty;
            }

            obj.TemplateID = item.TemplateID;
            list.Add(obj);
            item.IsRead = true;
            await notificationRepository.Update(item);
        }
        NotificationSearchResponseModel response = new(list, paged);
        return response;
    }

    private async Task<IPagedList<Notification>> Search(AdvanceSearchNotificationCommand model)
    {
        if (string.IsNullOrWhiteSpace(model.UserCode))
        {
            return new PagedList<Notification>([], model.PageIndex, model.PageSize);
        }

        var pageSize = Math.Clamp(model.PageSize, 1, 100);
        var targetChannel = StringUtils.Coalesce(model.AppType, model.ChannelId);

        IQueryable<Notification> query = notificationRepository.Table;

        query = query.Where(x => x.UserCode == model.UserCode);

        if (!string.IsNullOrWhiteSpace(targetChannel))
        {
            query = query.Where(x => x.AppType == targetChannel);
        }

        if (!string.IsNullOrWhiteSpace(model.NotificationType))
        {
            query = query.Where(x => x.NotificationType == model.NotificationType);
        }

        query = query.OrderByDescending(x => x.DateTime).ThenByDescending(x => x.Id);

        IPagedList<Notification> data = await query.ToPagedList(model.PageIndex, pageSize);
        return data;
    }
}
