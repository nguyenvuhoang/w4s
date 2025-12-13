using Newtonsoft.Json;
using System.Text.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;


public class CardServiceSearchResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardServiceSearchResponseModel()
    {
        this.PageSize = int.MaxValue;
    }

    public int Id { get; set; }

     [JsonProperty("cardservicecode")]
    public string CardServiceCode { get; set; }

    [JsonProperty("cardservicename")]
    public string CardServiceName { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>Page size</summary>
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;

}

public class CardServiceAdvancedSearchRequestModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardServiceAdvancedSearchRequestModel()
    {
        this.PageSize = int.MaxValue;
    }

    public int Id { get; set; }

    [JsonProperty("cardservicecode")]
    public string CardServiceCode { get; set; }

    [JsonProperty("cardservicename")]
    public string CardServiceName { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>Page size</summary>
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;

}



public class CardServiceInsertModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardServiceInsertModel(){}

    [JsonProperty("cardservicecode")]
    public string CardServiceCode { get; set; }

    [JsonProperty("cardservicename")]
    public string CardServiceName { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

}


public class CardServiceUpdateModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardServiceUpdateModel() { }

    public int Id { get; set; }

    [JsonProperty("cardservicename")]
    public string CardServiceName { get; set; }

}


public class CardServiceViewResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardServiceViewResponseModel() {}

    public int Id { get; set; }

     [JsonProperty("cardservicecode")]
    public string CardServiceCode { get; set; }

    [JsonProperty("cardservicename")]
    public string CardServiceName { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }
}
