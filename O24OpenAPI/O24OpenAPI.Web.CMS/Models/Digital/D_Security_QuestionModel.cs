using System.Text.Json;
using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class SecurityQuestionSearchAdvanceModel : BaseTransactionModel
{
    /// <summary>
    /// Deposit Account Search advance model constructor
    /// </summary>
    public SecurityQuestionSearchAdvanceModel()
    {
        this.PageSize = int.MaxValue;
    }

    /// <summary>
    ///
    /// </summary>
    public string Question { get; set; }

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
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;
}

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class SecurityQuestionSearchSimpleResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("question")]
    public string Question { get; set; }

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

}

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class SecurityQuestionSearchAdvanceResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("question")]
    public string Question { get; set; }

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

}

public class SecurityQuestionInsertModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public SecurityQuestionInsertModel() { }

    /// <summary>
    ///
    /// </summary>
    public string Question { get; set; }

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
}

public class SecurityQuestionViewModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public SecurityQuestionViewModel() { }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("question")]
    public string Question { get; set; }

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
}

public class SecurityQuestionUpdateModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Question { get; set; }

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
}
