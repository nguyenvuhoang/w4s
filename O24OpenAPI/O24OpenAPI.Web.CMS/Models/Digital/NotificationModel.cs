using Newtonsoft.Json;
using O24OpenAPI.Core.Extensions;

namespace O24OpenAPI.Web.CMS.Models.Digital;

public class GetListByAppAndTypeModel : BaseTransactionModel
{
    public string AppType { get; set; }
    public string NotificationType { get; set; }
}

public class GetListByAppModel : BaseTransactionModel
{
    public string AppType { get; set; }
}

public class NotificationSearchModel : BaseTransactionModel
{
    public NotificationSearchModel()
    {
        PageIndex = 0;
        PageSize = int.MaxValue;
    }

    public string UserCode { get; set; }
    public string AppType { get; set; }
    public string NotificationType { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class PushNotificationModel : BaseTransactionModel
{
    public string AppType { get; set; }
    public string NotificationType { get; set; }
    public string UserCode { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public object Data { get; set; }
    public string TemplateID { get; set; }
    public string Redirect { get; set; }
}

public class NotificationSearchResponse : BaseO24OpenAPIModel
{
    public NotificationSearchResponse(D_NOTIFICATION notification)
    {
        IsRead = notification.IsRead;
        DateTime = notification.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
        Data = notification.DataValue.ToJObject().ConvertToJObject().ToDictionary();
        Id = notification.Id;
        IsProcessed = notification.IsProcessed;
    }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("data")]
    public Dictionary<string, object> Data { get; set; }

    [JsonProperty("isread")]
    public bool IsRead { get; set; } = false;

    [JsonProperty("datetime")]
    public string DateTime { get; set; }

    [JsonProperty("isshowbutton")]
    public bool IsShowButton { get; set; } = true;

    [JsonProperty("templateid")]
    public string TemplateID { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("isprocessed")]
    public bool IsProcessed { get; set; } = false;
}

public class NotificationSearchResponseModel : BaseO24OpenAPIModel
{
    public NotificationSearchResponseModel(
        List<NotificationSearchResponse> list,
        IPagedList<D_NOTIFICATION> pageList
    )
    {
        TotalCount = pageList.TotalCount;
        TotalPages = pageList.TotalPages;
        HasPreviousPage = pageList.HasPreviousPage;
        HasNextPage = pageList.HasNextPage;
        Items = list;
    }

    [JsonProperty("totalcount")]
    public int TotalCount { get; }

    [JsonProperty("totalpages")]
    public int TotalPages { get; }

    [JsonProperty("haspreviouspage")]
    public bool HasPreviousPage { get; }

    [JsonProperty("hasnextpage")]
    public bool HasNextPage { get; }

    [JsonProperty("items")]
    public List<NotificationSearchResponse> Items { get; set; } =
        new List<NotificationSearchResponse>();
}
