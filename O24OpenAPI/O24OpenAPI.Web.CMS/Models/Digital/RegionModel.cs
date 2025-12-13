namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>
/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class GetRegionByRegionCodeModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public int RegionID { get; set; }
}

/// <summary>
/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class GetRegionByRegionNameModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string RegionName { get; set; }
}


public class RegionModel : BaseTransactionModel
{
    public string RegionName { get; set; }
    public string RegionSpecial { get; set; }
    public string Description { get; set; }
    public string UserCreate { get; set; }
    public DateTime? DateCreate { get; set; } = DateTime.UtcNow;
    public string UserModify { get; set; }
    public DateTime? DateModify { get; set; }
    public string Status { get; set; }

}

public class RegionUpdateModel : BaseTransactionModel
{

    public int RegionID { get; set; }
    public string RegionName { get; set; }
    public string RegionSpecial { get; set; }
    public string Description { get; set; }

}
public class SearchRegionModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string RegionName { get; set; }

    public string RegionSpecial { get; set; }

    public string Description { get; set; }


}

public class DeleteRegionModel : BaseTransactionModel
{
    public string RegionCode { get; set; }
    public List<string> ListRegionCode { get; set; } = new List<string>();
}
