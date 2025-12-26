using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.DataWarehouse.Domain.PORTAL;

public partial class D_COUNTRY : BaseEntity
{
    /// <summary>
    ///
    /// </summary>
    public D_COUNTRY() { }

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
    public string Language { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    public decimal Order { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Description { get; set; }

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
    public DateTime LastModified { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UserApproved { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime DateApproved { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TimeZone { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string PhoneCountryCode { get; set; }
}
