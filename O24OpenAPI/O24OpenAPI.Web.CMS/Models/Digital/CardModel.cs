using Newtonsoft.Json;
using System.Text.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;


public class CardSearchResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardSearchResponseModel()
    {
        this.PageSize = int.MaxValue;
    }

    public int Id { get; set; }

    [JsonProperty("cardcode")]
    public string CardCode { get; set; }

    [JsonProperty("cardlogo")]
    public string CardLogo { get; set; }

    [JsonProperty("cardname")]
    public string CardName { get; set; }

    [JsonProperty("cardtype")]
    public string CardType { get; set; }

    [JsonProperty("cardlimit")]
    public decimal CardLimit { get; set; }

    [JsonProperty("cardservicecode")]
    public string CardServiceCode { get; set; }

    /// <summary>Page size</summary>
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;

}

public class CardAdvancedSearchRequestModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardAdvancedSearchRequestModel()
    {
        this.PageSize = int.MaxValue;
    }

    [JsonProperty("cardcode")]
    public string CardCode { get; set; }

    [JsonProperty("cardlogo")]
    public string CardLogo { get; set; }

    [JsonProperty("cardname")]
    public string CardName { get; set; }

    [JsonProperty("cardtype")]
    public string CardType { get; set; }

    [JsonProperty("cardlimit")]
    public decimal CardLimit { get; set; }

    [JsonProperty("cardservicecode")]
    public string CardServiceCode { get; set; }

    /// <summary>Page size</summary>
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;

}



public class CardInsertModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardInsertModel(){}

    [JsonProperty("cardcode")]
    public string CardCode { get; set; }

    [JsonProperty("cardlogo")]
    public string CardLogo { get; set; }

    [JsonProperty("cardname")]
    public string CardName { get; set; }

    [JsonProperty("cardtype")]
    public string CardType { get; set; }

    [JsonProperty("cardlimit")]
    public decimal CardLimit { get; set; }

    [JsonProperty("cardservicecode")]
    public string CardServiceCode { get; set; }

}


public class CardUpdateModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardUpdateModel() { }

    public int Id { get; set; }

    [JsonProperty("cardlogo")]
    public string CardLogo { get; set; }

    [JsonProperty("cardname")]
    public string CardName { get; set; }

    [JsonProperty("cardtype")]
    public string CardType { get; set; }

    [JsonProperty("cardlimit")]
    public decimal CardLimit { get; set; }

    [JsonProperty("cardservicecode")]
    public string CardServiceCode { get; set; }

}



public class CardViewResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public CardViewResponseModel(){}

    public int Id { get; set; }

    [JsonProperty("cardcode")]
    public string CardCode { get; set; }

    [JsonProperty("cardlogo")]
    public string CardLogo { get; set; }

    [JsonProperty("cardname")]
    public string CardName { get; set; }

    [JsonProperty("cardtype")]
    public string CardType { get; set; }

    [JsonProperty("cardlimit")]
    public decimal CardLimit { get; set; }

    [JsonProperty("cardservicecode")]
    public string CardServiceCode { get; set; }

}
