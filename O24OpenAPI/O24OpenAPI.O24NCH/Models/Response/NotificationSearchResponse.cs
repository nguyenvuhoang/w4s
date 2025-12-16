using Newtonsoft.Json;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Utils;

namespace O24OpenAPI.O24NCH.Models.Response;

public class NotificationSearchResponse : BaseO24OpenAPIModel
{
    public NotificationSearchResponse(Notification notification)
    {
        IsRead = notification.IsRead;
        DateTime = notification.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
        var a = notification.DataValue.SafeParseJson();
        _ = a.ToDictionary();
        Data = notification.DataValue.SafeParseJson().ToDictionary();
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
    public bool? IsShowButton { get; set; }

    [JsonProperty("templateid")]
    public string TemplateID { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("isprocessed")]
    public bool? IsProcessed { get; set; }
}

public class NotificationSearchResponseModel : BaseO24OpenAPIModel
{
    public int TotalCount { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }
    public List<NotificationSearchResponse> Items { get; set; }

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
