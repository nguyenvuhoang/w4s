using System.Text.Json;
using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class CountrySearchAdvanceModel : BaseTransactionModel
{
    /// <summary>
    /// Deposit Account Search advance model constructor
    /// </summary>
    public CountrySearchAdvanceModel()
    {
        this.PageSize = int.MaxValue;
    }

    /// <summary>
    ///
    /// </summary>
    public string CountryID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CountryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string PhoneCountryCode { get; set; }

    /// <summary>Page size</summary>
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>Page index</summary>
    public int PageIndex { get; set; } = 0;
}

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class SearchSimpleResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("countryid")]
    public string CountryID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("countrycode")]
    public string CountryCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("countryname")]
    public string CountryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("mcountryname")]
    public string MCountryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("capitalname")]
    public string CapitalName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("currencyid")]
    public string CurrencyID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("language")]
    public string Language { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("order")]
    public decimal Order { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

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
    [JsonProperty("lastmodified")]
    public DateTime LastModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("userapproved")]
    public string UserApproved { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("dateapproved")]
    public DateTime DateApproved { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("timezone")]
    public string TimeZone { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("phonecountrycode")]
    public string PhoneCountryCode { get; set; }
}

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class SearchAdvanceResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("countryid")]
    public string CountryID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("countrycode")]
    public string CountryCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("countryname")]
    public string CountryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("mcountryname")]
    public string MCountryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("capitalname")]
    public string CapitalName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("currencyid")]
    public string CurrencyID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("language")]
    public string Language { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("order")]
    public decimal Order { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

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
    [JsonProperty("lastmodified")]
    public DateTime LastModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("userapproved")]
    public string UserApproved { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("dateapproved")]
    public DateTime DateApproved { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("timezone")]
    public string TimeZone { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("phonecountrycode")]
    public string PhoneCountryCode { get; set; }
}

public class CountryInsertModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public CountryInsertModel() { }

    /// <summary>
    ///
    /// </summary>
    ///
    public string CountryID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CountryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string MCountryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CapitalName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CurrencyID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Language { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    public decimal Order { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? LastModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserApproved { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string DateApproved { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TimeZone { get; set; }

    /// <summary>
    ///
    /// </summary>
    ///
    public string PhoneCountryCode { get; set; }
}

public class CountryViewModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public CountryViewModel() { }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("countryid")]
    public string CountryID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("countrycode")]
    public string CountryCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("countryname")]
    public string CountryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("mcountryname")]
    public string MCountryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("capitalname")]
    public string CapitalName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("currencyid")]
    public string CurrencyID { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("language")]
    public string Language { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("order")]
    public decimal Order { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

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
    [JsonProperty("lastmodified")]
    public DateTime? LastModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("userapproved")]
    public string UserApproved { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("dateapproved")]
    public DateTime? DateApproved { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("timezone")]
    public string TimeZone { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("phonecountrycode")]
    public string PhoneCountryCode { get; set; }
}

public class CountryUpdateModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CountryID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CountryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string MCountryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CapitalName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string CurrencyID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Language { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    public decimal Order { get; set; }

    /// <summary>
    ///
    /// </summary>
    public new string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? DateCreated { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? LastModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserApproved { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string DateApproved { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TimeZone { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string PhoneCountryCode { get; set; }
}
public class DeleteCountryModel : BaseTransactionModel
{
    public string ID { get; set; }
    public List<string> ListID { get; set; } = new List<string>();
}
