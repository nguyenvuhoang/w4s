namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>
/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class GetCityByCityCodeModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public int CityID { get; set; }
}

/// <summary>
/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class GetCityByCityNameModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string CityName { get; set; }
}


public class CityModel : BaseTransactionModel
{
    public string CityCode { get; set; }
    public string CityName { get; set; }
    public string SearchCode { get; set; }

    public string Description { get; set; }
    public int Ord { get; set; }
    public int RegionId { get; set; }
    public string CountryId { get; set; }
    public string Status { get; set; }
    public string UserCreated { get; set; }
    public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
    public string UserModified { get; set; }
    public DateTime? LastModified { get; set; }
    public string UserApproved { get; set; }
    public DateTime? DateApproved { get; set; }
    public string CityNameMM { get; set; }

}

public class CityUpdateModel : BaseTransactionModel
{

    public int CityID { get; set; }
    public string CityCode { get; set; }
    public string CityName { get; set; }
    public string Description { get; set; }


}
public class SearchCityModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string CityCode { get; set; }
    public string CityName { get; set; }

    public string Description { get; set; }


}
