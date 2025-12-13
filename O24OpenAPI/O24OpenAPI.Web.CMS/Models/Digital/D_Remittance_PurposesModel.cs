using System.Text.Json;
using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class RemittancePurposesSearchAdvanceModel : BaseTransactionModel
{
    /// <summary>
    /// Deposit Account Search advance model constructor
    /// </summary>
    public RemittancePurposesSearchAdvanceModel()
    {
        this.PageSize = int.MaxValue;
    }

    /// <summary>
    /// ///
    /// </summary>
    public string RemittancePurposes { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string LangID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string IMGLINK { get; set; }
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;
}

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class RemittancePurposesSearchSimpleResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("remittancepurposes")]
    public string RemittancePurposes { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("langid")]
    public string LangID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usercreated")]
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("datecreated")]
    public DateTime DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usermodified")]
    public string UserModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("imglink")]
    public string IMGLINK { get; set; }

}

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class RemittancePurposesSearchAdvanceResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("remittancepurposes")]
    public string RemittancePurposes { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("langid")]
    public string LangID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usercreated")]
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("datecreated")]
    public DateTime? DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usermodified")]
    public string UserModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("imglink")]
    public string IMGLINK { get; set; }

}

public class RemittancePurposesInsertModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public RemittancePurposesInsertModel() { }

    /// <summary>
    ///
    /// </summary>
    public string RemittancePurposes { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string LangID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string IMGLINK { get; set; }
}

public class RemittancePurposesViewModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public RemittancePurposesViewModel() { }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("remittancepurposes")]
    public string RemittancePurposes { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("langid")]
    public string LangID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usercreated")]
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("datecreated")]
    public DateTime DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("usermodified")]
    public string UserModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("imglink")]
    public string IMGLINK { get; set; }
}

public class RemittancePurposesUpdateModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string RemittancePurposes { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string LangID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string IMGLINK { get; set; }
}
