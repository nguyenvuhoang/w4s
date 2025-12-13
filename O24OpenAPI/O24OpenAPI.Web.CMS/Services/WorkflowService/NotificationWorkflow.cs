using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Utils;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class NotificationWorkflow : BaseQueueService
{
    private readonly INotificationService _contextService =
        EngineContext.Current.Resolve<INotificationService>();

    private readonly IDeviceService _deviceService =
        EngineContext.Current.Resolve<IDeviceService>();

    private readonly INotificationTemplateService _notificationTemplateService =
        EngineContext.Current.Resolve<INotificationTemplateService>();

    private readonly IMappingService _mappingService =
        EngineContext.Current.Resolve<IMappingService>();

    public async Task<WorkflowScheme> GetListByAppAndType(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetListByAppAndTypeModel>();
        return await Invoke<GetListByAppAndTypeModel>(
            workflow,
            async () =>
            {
                var response = new List<JObject>();
                List<D_NOTIFICATION> list = await _contextService.GetListByAppAndType(
                    model.AppType,
                    model.NotificationType
                );
                foreach (var item in list)
                {
                    var obj = item.ToJObject();
                    var dataValue = item.DataValue.ToDictionary();
                    obj["message"] = obj["Template"].ToString().GetMessage(dataValue);
                    obj["DataValue"] = dataValue.ToJToken();
                    response.Add(obj);
                }
                return response;
            }
        );
    }

    public async Task<WorkflowScheme> GetListByApp(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetListByAppModel>();
        return await Invoke<GetListByAppModel>(
            workflow,
            async () =>
            {
                var response = new List<JObject>();
                List<D_NOTIFICATION> list = await _contextService.GetListByApp(model.AppType);
                foreach (var item in list)
                {
                    var obj = item.ToJObject();
                    var dataValue = item.DataValue.ToDictionary();
                    obj["message"] = obj["Template"].ToString().GetMessage(dataValue);
                    obj["DataValue"] = dataValue.ToJToken();
                    response.Add(obj);
                }
                return response;
            }
        );
    }

    public async Task<WorkflowScheme> Search(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<NotificationSearchModel>();
        return await Invoke<NotificationSearchModel>(
            workflow,
            async () =>
            {
                var data = await _contextService.Search(model);
                var list = new List<NotificationSearchResponse>();

                foreach (var item in data)
                {
                    var obj = new NotificationSearchResponse(item);

                    var template = await _notificationTemplateService.GetTemplate(
                        item.TemplateID
                    );
                    obj.IsShowButton = template.IsShowButton;

                    string message = template.Body.GetMessage(obj.Data, model.Language);
                    obj.Message = message;
                    obj.TemplateID = item.TemplateID;

                    list.Add(obj);
                    item.IsRead = true;
                    await _contextService.Update(item);
                }
                var response = new NotificationSearchResponseModel(list, data);
                return response;
            }
        );
    }

    public async Task<WorkflowScheme> GetUnreadCount(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<NotificationSearchModel>();
        return await Invoke<NotificationSearchModel>(
            workflow,
            async () =>
            {
                var userCode = StringUtils.Coalesce(model.UserCode, SessionUtils.GetUserCode());
                var count = await _contextService.GetUnreadCount(userCode, model.ChannelId);
                return new JObject { { "count", count } };
            }
        );
    }

    public async Task<WorkflowScheme> CompleteNotification(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ModelWithId>();
        return await Invoke<ModelWithId>(
            workflow,
            async () =>
            {
                await _contextService.CompleteNotification(model.Id);
                return null;
            }
        );
    }

    public async Task<WorkflowScheme> PushNotification(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<PushNotificationModel>();
        return await Invoke<PushNotificationModel>(
            workflow,
            async () =>
            {
                var device = await _deviceService.GetByUserIdAndAppType(
                    model.UserCode,
                    model.AppType
                );

                if (device == null)
                {
                    return null;
                }

                var template = await _notificationTemplateService.GetTemplate(model.TemplateID);
                Dictionary<string, string> dataValue = [];
                JToken dataToken = null;
                if (model.Data != null)
                {
                    dataValue["datasending"] = model.Data.ToString();
                }
                dataValue["learnapisending"] = template.LearnApiSending;
                var notification = new D_NOTIFICATION
                {
                    UserCode = model.UserCode,
                    AppType = model.AppType,
                    NotificationType = model.NotificationType,
                    DataValue = dataValue.ToSerialize(),
                    TemplateID = model.TemplateID,
                    Redirect = model.Redirect,
                    IsPushed = true,
                    IsRead = false,
                    DateTime = DateTime.Now.ToLocalTime(),
                };
                await _contextService.Insert(notification);
                await _contextService.SendNotificationAsync(
                    device.PushID,
                    model.Title.HasValue() ? model.Title : template.Title,
                    model.Body.HasValue()
                        ? model.Body
                        : template.Body.GetMessage(dataToken.ToDictionary(), model.Language),
                    dataValue
                );
                return null;
            }
        );
    }
}
