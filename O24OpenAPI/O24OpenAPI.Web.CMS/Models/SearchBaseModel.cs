using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models;

/// <summary>
///
/// </summary>
public partial class SearchBaseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("page_size")]
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("page_index")]
    public int PageIndex { get; set; } = 0;
}

public class SearchPagedListModel : BaseTransactionModel
{
    /// <summary>
    /// ///
    /// </summary>
    [JsonProperty("entity_name")]
    public string EntityName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("page_size")]
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("page_index")]
    public int PageIndex { get; set; } = 0;
}
