using System.Text.Json;
using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class ServiceSearchAdvanceModel : BaseTransactionModel
{
    /// <summary>
    /// Deposit Account Search advance model constructor
    /// </summary>
    public ServiceSearchAdvanceModel()
    {
        this.PageSize = int.MaxValue;
    }

    /// <summary>
    ///
    /// </summary>
    public string ServiceID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool BankService { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool CorpService { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool checkuseronline { get; set; }
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;
}

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class ServiceSearchSimpleResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("serviceid")]
    public string ServiceID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("servicename")]
    public string ServiceName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("bankservice")]
    public bool BankService { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("corpservice")]
    public bool CorpService { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("checkuseronline")]
    public bool checkuseronline { get; set; }
}

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class ServiceSearchAdvanceResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("serviceid")]
    public string ServiceID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("servicename")]
    public string ServiceName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("bankservice")]
    public bool BankService { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("corpservice")]
    public bool CorpService { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("checkuseronline")]
    public bool checkuseronline { get; set; }
}

public class ServiceInsertModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public ServiceInsertModel() { }

    /// <summary>
    ///
    /// </summary>
    public string ServiceID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool BankService { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool CorpService { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool checkuseronline { get; set; }
}

public class ServiceViewModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public ServiceViewModel() { }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("serviceid")]
    public string ServiceID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("servicename")]
    public string ServiceName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("bankservice")]
    public bool BankService { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("corpservice")]
    public bool CorpService { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("checkuseronline")]
    public bool checkuseronline { get; set; }
}

public class ServiceUpdateModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ServiceID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool BankService { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool CorpService { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool checkuseronline { get; set; }
}
