using System.Text.Json;
using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Domain;

public class D_BANK : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value of the code
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the value of the bin
    /// </summary>
    [JsonProperty("bin")]
    public string Bin { get; set; }

    /// <summary>
    /// Gets or sets the value of the short name
    /// </summary>
    [JsonProperty("shortname")]
    public string ShortName { get; set; }

    /// <summary>
    /// Gets or sets the value of the logo
    /// </summary>
    [JsonProperty("logo")]
    public string Logo { get; set; }

    /// <summary>
    /// Gets or sets the value of the transfer supported
    /// </summary>\
    [JsonProperty("transfersupported")]
    public int TransferSupported { get; set; }

    /// <summary>
    /// Gets or sets the value of the lookup supported
    /// </summary>

    [JsonProperty("lookupsupported")]
    public int LookupSupported { get; set; }

    /// <summary>
    /// Gets or sets the value of the support
    /// </summary>
    [JsonProperty("support")]
    public int Support { get; set; }

    /// <summary>
    /// Gets or sets the value of the is transfer
    /// </summary>
    [JsonProperty("istransfer")]
    public int IsTransfer { get; set; }

    public bool IsSender { get; set; }

    /// <summary>
    /// Gets or sets the value of the swift code
    /// </summary>
    [JsonProperty("swiftcode")]
    public string SwiftCode { get; set; }
}

public class D_BANKModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>

    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value of the code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the value of the bin
    /// </summary>
    public string Bin { get; set; }

    /// <summary>
    /// Gets or sets the value of the short name
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// Gets or sets the value of the logo
    /// </summary>
    public string Logo { get; set; }

    /// <summary>
    /// Gets or sets the value of the transfer supported
    /// </summary>
    public int TransferSupported { get; set; }

    /// <summary>
    /// Gets or sets the value of the lookup supported
    /// </summary>
    public int LookupSupported { get; set; }

    /// <summary>
    /// Gets or sets the value of the support
    /// </summary>
    public int Support { get; set; }

    /// <summary>
    /// Gets or sets the value of the is transfer
    /// </summary>
    public int IsTransfer { get; set; }

    /// <summary>
    /// Gets or sets the value of the swift code
    /// </summary>
    public string SwiftCode { get; set; }
}
