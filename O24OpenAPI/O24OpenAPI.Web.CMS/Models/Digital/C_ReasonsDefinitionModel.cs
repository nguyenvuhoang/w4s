using System.Text.Json;
using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;

public class ReasonsDefinitionSearchAdvanceModel : BaseTransactionModel
{
    /// <summary>
    /// Deposit Account Search advance model constructor
    /// </summary>
    public ReasonsDefinitionSearchAdvanceModel()
    {
        this.PageSize = int.MaxValue;
    }

    /// <summary>
    ///
    /// </summary>
    public decimal ReasonID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonAction { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonEvent { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Status { get; set; }
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;
}

public class ReasonsDefinitionSearchSimpleResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasonid")]
    public decimal ReasonID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasoncode")]
    public string ReasonCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasonname")]
    public string ReasonName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasonaction")]
    public string ReasonAction { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasontype")]
    public string ReasonType { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasonevent")]
    public string ReasonEvent { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }
}

public class ReasonsDefinitionSearchAdvanceResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasonid")]
    public decimal ReasonID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasoncode")]
    public string ReasonCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasonname")]
    public string ReasonName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasonaction")]
    public string ReasonAction { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasontype")]
    public string ReasonType { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasonevent")]
    public string ReasonEvent { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }
}

public class ReasonsDefinitionInsertModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public ReasonsDefinitionInsertModel() { }

    /// <summary>
    ///
    /// </summary>
    public decimal ReasonID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonAction { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonEvent { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Status { get; set; }
}

public class ReasonsDefinitionViewModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public ReasonsDefinitionViewModel() { }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasonid")]
    public decimal ReasonID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasoncode")]
    public string ReasonCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasonname")]
    public string ReasonName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasonaction")]
    public string ReasonAction { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasontype")]
    public string ReasonType { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("reasonevent")]
    public string ReasonEvent { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }
}

public class ReasonsDefinitionUpdateModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public decimal ReasonID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonAction { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonEvent { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Status { get; set; }
}
