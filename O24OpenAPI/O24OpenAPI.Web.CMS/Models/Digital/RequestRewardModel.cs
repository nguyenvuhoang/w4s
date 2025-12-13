using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class GetRequestRewardByIDModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public int Id { get; set; }
}

public class RequestRewardModel : BaseTransactionModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("ipctranid")]
    public string IPCTRANSID { get; set; }

    [JsonProperty("usercode")]
    public string UserCode { get; set; }

    [JsonProperty("amount")]
    public string Amount { get; set; }

    [JsonProperty("qrcode")]
    public string QRCode { get; set; }

    [JsonProperty("giftid")]
    public int GiftID { get; set; }

    [JsonProperty("giftname")]
    public string GiftName { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("branchid")]
    public string BranchID { get; set; }

    [JsonProperty("createdby")]
    public string CreatedBy { get; set; }

    [JsonProperty("createdat")]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("modifiedby")]
    public string ModifiedBy { get; set; }

    [JsonProperty("modifiedat")]
    public DateTime? ModifiedAt { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("reason")]
    public string Reason { get; set; }

}
public class SearchRequestRewardModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    [JsonProperty("giftname")]
    public string GiftName { get; set; }
    [JsonProperty("branchid")]
    public string BranchID { get; set; }
    [JsonProperty("status")]
    public string Status { get; set; }
}
