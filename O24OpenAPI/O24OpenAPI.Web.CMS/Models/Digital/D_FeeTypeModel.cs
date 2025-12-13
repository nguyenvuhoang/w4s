using System.Text.Json;
using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class FeeTypeSearchAdvanceModel : BaseTransactionModel
{
    /// <summary>
    /// Deposit Account Search advance model constructor
    /// </summary>
    public FeeTypeSearchAdvanceModel()
    {
        this.PageSize = int.MaxValue;
    }

    /// <summary>
    ///
    /// </summary>
    public string FeeType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TypeName { get; set; }
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;
}

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class FeeTypeSearchSimpleResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("feetype")]
    public string FeeType { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("typename")]
    public string TypeName { get; set; }

}

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class FeeTypeSearchAdvanceResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("feetype")]
    public string FeeType { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("typename")]
    public string TypeName { get; set; }

}

public class FeeTypeInsertModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public FeeTypeInsertModel() { }

    /// <summary>
    ///
    /// </summary>
    ///
    public string FeeType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TypeName { get; set; }
}

public class FeeTypeViewModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public FeeTypeViewModel() { }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("feetype")]
    public string FeeType { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("typename")]
    public string TypeName { get; set; }
}

public class FeeTypeUpdateModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FeeType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TypeName { get; set; }
}
