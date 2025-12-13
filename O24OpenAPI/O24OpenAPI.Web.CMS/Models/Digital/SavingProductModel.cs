using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;


public class SavingProductSimpleSearchResponseModel : BaseO24OpenAPIModel

{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public SavingProductSimpleSearchResponseModel()
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

public class SavingProductAdvancedSearchResponseModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public SavingProductAdvancedSearchResponseModel()
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


public class SavingProductAdvancedSearchRequestModel : BaseTransactionModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public SavingProductAdvancedSearchRequestModel()
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


public class SavingProductInsertModel : BaseTransactionModel
{
    /// <summary>
    /// SavingProductInsertModel
    /// </summary>
    public SavingProductInsertModel(){}

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



public class SavingProductUpdateModel : BaseTransactionModel
{
    /// <summary>
    /// SavingProductUpdateModel
    /// </summary>
    public SavingProductUpdateModel() { }

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


public class SavingProductViewResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// SimpleSearchResponseModel
    /// </summary>
    public SavingProductViewResponseModel() { }

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
