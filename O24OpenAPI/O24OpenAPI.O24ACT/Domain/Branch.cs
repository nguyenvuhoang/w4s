using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24ACT.Domain;

public class Branch : BaseEntity
{
    /// <summary>
    /// BranchCode
    /// </summary>
    [JsonProperty("branch_code")]
    public string BranchCode { get; set; }

    /// <summary>
    /// RefId
    /// </summary>
    [JsonProperty("old_branch_id")]
    public string OldBranchId { get; set; }

    /// <summary>
    /// BranchName
    /// </summary>
    [JsonProperty("branch_name")]
    public string BranchName { get; set; }

    /// <summary>
    /// BranchAddress
    /// </summary>
    [JsonProperty("branch_address")]
    public string BranchAddress { get; set; }

    /// <summary>
    /// BranchPhone
    /// </summary>
    [JsonProperty("branch_phone")]
    public string BranchPhone { get; set; }

    /// <summary>
    /// Phone Number (Home, Office, Cell, Facsimile, Telex)
    /// </summary>
    [JsonProperty("phone_number")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// tax code
    /// </summary>
    [JsonProperty("tax_code")]
    public string TaxCode { get; set; }

    /// <summary>
    /// bcycid
    /// </summary>
    [JsonProperty("base_currency_code")]
    public string BaseCurrencyCode { get; set; }

    /// <summary>
    /// bcycnm
    /// </summary>
    [JsonProperty("base_currency_name")]
    public string BaseCurrencyName { get; set; }

    /// <summary>
    /// lcycid
    /// </summary>
    [JsonProperty("local_currency_code")]
    public string LocalCurrencyCode { get; set; }

    /// <summary>
    /// lcynm
    /// </summary>
    [JsonProperty("local_currency_name")]
    public string LocalCurrencyName { get; set; }

    /// <summary>
    /// Reference code (Bic, domestic bank code, internal code)
    /// </summary>
    [JsonProperty("reference_code")]
    public string ReferenceCode { get; set; }

    /// <summary>
    /// country
    /// </summary>
    [JsonProperty("country")]
    public string Country { get; set; }

    /// <summary>
    /// Main anguage
    /// </summary>
    [JsonProperty("main_language")]
    public string MainLanguage { get; set; }

    /// <summary>
    /// TimeZoneOfBranch
    /// </summary>
    [JsonProperty("time_zone_of_branch")]
    public decimal TimeZoneOfBranch { get; set; }

    /// <summary>
    /// ThousandSeparateCharacter
    /// </summary>
    [JsonProperty("thousand_separate_character")]
    public string ThousandSeparateCharacter { get; set; }

    /// <summary>
    /// DecimalSeparateCharacter
    /// </summary>
    [JsonProperty("decimal_separate_character")]
    public string DecimalSeparateCharacter { get; set; }

    /// <summary>
    /// DateFormatForShort
    /// </summary>
    [JsonProperty("date_format_for_short")]
    public string DateFormatForShort { get; set; }

    /// <summary>
    /// LongDateFormat
    /// </summary>
    [JsonProperty("long_date_format")]
    public string LongDateFormat { get; set; }

    /// <summary>
    /// TimeFormat
    /// </summary>
    [JsonProperty("time_format")]
    public string TimeFormat { get; set; }

    /// <summary>
    /// Online
    /// </summary>
    [JsonProperty("is_online")]
    public string IsOnline { get; set; }

    /// <summary>
    /// UdField1
    /// </summary>
    [JsonProperty("long_branch_name")]
    public string LongBranchName { get; set; }

    /// <summary>
    /// Region
    /// </summary>
    [JsonProperty("region")]
    public string Region { get; set; }

    /// <summary>
    /// Region
    /// </summary>
    [JsonProperty("branch_type")]
    public string BranchType { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }
}
