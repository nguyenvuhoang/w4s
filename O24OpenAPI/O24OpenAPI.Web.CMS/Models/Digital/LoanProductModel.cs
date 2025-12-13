using Newtonsoft.Json;
using System.Text.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;


public class LoanProductSimpleSearchResponseModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public LoanProductSimpleSearchResponseModel()
    {
        this.PageSize = int.MaxValue;
    }

    public int Id { get; set; }

    [JsonProperty("productcode")]
    public string ProductCode { get; set; }

    [JsonProperty("productname")]
    public string ProductName { get; set; }

    [JsonProperty("productimage")]
    public string ProductImage { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
    [JsonProperty("usercode")]
    public string UserCode { get; set; }
    [JsonProperty("isallowregister")]
    public bool IsAllowRegister { get; set; }

    [JsonProperty("producturl")]
    public string ProductUrl { get; set; }

    /// <summary>Page size</summary>
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;

}

public class LoanProductAdvancedSearchResponseModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public LoanProductAdvancedSearchResponseModel()
    {
        this.PageSize = int.MaxValue;
    }

    public int Id { get; set; }

    [JsonProperty("productcode")]
    public string ProductCode { get; set; }

    [JsonProperty("productname")]
    public string ProductName { get; set; }

    [JsonProperty("productimage")]
    public string ProductImage { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
    [JsonProperty("usercode")]
    public string UserCode { get; set; }
    [JsonProperty("isallowregister")]
    public bool IsAllowRegister { get; set; }

    [JsonProperty("producturl")]
    public string ProductUrl { get; set; }

    /// <summary>Page size</summary>
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;

}


public class LoanProductAdvancedSearchRequestModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public LoanProductAdvancedSearchRequestModel()
    {
        this.PageSize = int.MaxValue;
    }

    [JsonProperty("productcode")]
    public string ProductCode { get; set; }

    [JsonProperty("productname")]
    public string ProductName { get; set; }

    [JsonProperty("productimage")]
    public string ProductImage { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
    [JsonProperty("usercode")]
    public string UserCode { get; set; }
    [JsonProperty("isallowregister")]
    public bool IsAllowRegister { get; set; }

    /// <summary>Page size</summary>
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;

}

public class LoanProductInsertModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public LoanProductInsertModel(){}

    [JsonProperty("productcode")]
    public string ProductCode { get; set; }

    [JsonProperty("productname")]
    public MultiProductName ProductName { get; set; }

    [JsonProperty("productimage")]
    public string ProductImage { get; set; }

    [JsonProperty("description")]
    public MultiDescription Description { get; set; }
    [JsonProperty("usercode")]
    public string UserCode { get; set; }
    [JsonProperty("isallowregister")]
    public bool IsAllowRegister { get; set; }

    [JsonProperty("producturl")]
    public string ProductUrl { get; set; }

}


public class LoanProductUpdateModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public LoanProductUpdateModel() { }

    public int Id { get; set; }

    [JsonProperty("productname")]
    public MultiProductName ProductName { get; set; }

    [JsonProperty("productimage")]
    public string ProductImage { get; set; }

    [JsonProperty("description")]
    public MultiDescription Description { get; set; }
    [JsonProperty("usercode")]
    public string UserCode { get; set; }
    [JsonProperty("isallowregister")]
    public bool IsAllowRegister { get; set; }

    [JsonProperty("producturl")]
    public string ProductUrl { get; set; }

}


/// <summary>
/// MultiProductName
/// </summary>
public class MultiProductName
{
    /// <summary>
    /// Vietnamese
    /// </summary>
    [JsonProperty("vi")]
    public string Vietnamese { get; set; } = null;

    /// <summary>
    /// English
    /// </summary>
    [JsonProperty("en")]
    public string English { get; set; } = null;

    /// <summary>
    /// ThaiLan
    /// </summary>
    [JsonProperty("thai")]
    public string ThaiLand { get; set; } = null;

    /// <summary>
    /// Lao
    /// </summary>
    [JsonProperty("lao")]
    public string Lao { get; set; } = null;

    /// <summary>
    /// Cambodia
    /// </summary>
    [JsonProperty("cam")]
    public string Cambodia { get; set; } = null;

    /// <summary>
    /// Myanmar
    /// </summary>
    [JsonProperty("mya")]
    public string Myanmar { get; set; } = null;
}

/// <summary>
/// MultiProductName
/// </summary>
public class MultiDescription
{
    /// <summary>
    /// Vietnamese
    /// </summary>
    [JsonProperty("vi")]
    public string Vietnamese { get; set; } = null;

    /// <summary>
    /// English
    /// </summary>
    [JsonProperty("en")]
    public string English { get; set; } = null;

    /// <summary>
    /// ThaiLan
    /// </summary>
    [JsonProperty("thai")]
    public string ThaiLand { get; set; } = null;

    /// <summary>
    /// Lao
    /// </summary>
    [JsonProperty("lao")]
    public string Lao { get; set; } = null;

    /// <summary>
    /// Cambodia
    /// </summary>
    [JsonProperty("cam")]
    public string Cambodia { get; set; } = null;

    /// <summary>
    /// Myanmar
    /// </summary>
    [JsonProperty("mya")]
    public string Myanmar { get; set; } = null;
}

public class LoanProductViewResponseModel :BaseO24OpenAPIModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public LoanProductViewResponseModel(){}

    [JsonProperty("productcode")]
    public string ProductCode { get; set; }

    [JsonProperty("productname")]
    public string ProductName { get; set; }

    [JsonProperty("productimage")]
    public string ProductImage { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
    [JsonProperty("usercode")]
    public string UserCode { get; set; }
    [JsonProperty("isallowregister")]
    public bool IsAllowRegister { get; set; }

    [JsonProperty("producturl")]
    public string ProductUrl { get; set; }

}
