namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>

/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class GetBranchByBranchCodeModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string BranchID { get; set; }
}

public class DeleteBranchByBranchCodeModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string BranchID { get; set; }
    public List<string> ListBranchID { get; set; } = new List<string>();
}

public class BranchModel : BaseTransactionModel
{

    public string BranchID { get; set; }
    public string BranchName { get; set; }
    public string Address { get; set; }
    public string CityCode { get; set; }
    public string DistCode { get; set; }
    public string RegionID { get; set; }
    public string Phone { get; set; }
    public string AdPaymentAcctLow { get; set; }
    public string AdPaymentAcctHight { get; set; }
    public string FeeAcct { get; set; }
    public string IsOpenFD { get; set; }
    public string PositionX { get; set; }
    public string PositionY { get; set; }
    public string MobilePhone { get; set; }
    public string TaxCode { get; set; }
    public string BicCode { get; set; }
    public string SwiftCode { get; set; }
    public string BCurrencyCode { get; set; }
    public string BCYNM { get; set; }
    public string LCurrencyCode { get; set; }
    public string LCYNM { get; set; }
    public string RefCode { get; set; }
    public string CountryID { get; set; }
    public string Language { get; set; }
    public TimeSpan? TimeOpen { get; set; }
    public TimeSpan? TimeClose { get; set; }
    public string ThousandNumFMT { get; set; }
    public string DecimalNumFMT { get; set; }
    public string DateFMT { get; set; }
    public string LDateFMT { get; set; }
    public string TimeFMT { get; set; }
    public string Status { get; set; }
    public string UDF { get; set; }
    public string Website { get; set; }
    public string Email { get; set; }
    public DateTime? OpenDate { get; set; }
    public string UserCreate { get; set; }
    public DateTime DateCreate { get; set; } = DateTime.UtcNow;
    public string UserModified { get; set; }
    public DateTime? LastModified { get; set; }
    public string UserApproved { get; set; }
    public DateTime? DateApproved { get; set; }

}

public class BranchUpdateModel : BaseTransactionModel
{

    public string BranchID { get; set; }
    public string BranchName { get; set; }
    public string Address { get; set; }
    public string CityCode { get; set; }
    public string DistCode { get; set; }
    public string RegionID { get; set; }
    public string Phone { get; set; }
    public string PositionX { get; set; }
    public string PositionY { get; set; }
    public string MobilePhone { get; set; }
    public string TaxCode { get; set; }
    public string BicCode { get; set; }
    public string SwiftCode { get; set; }
    public string CountryID { get; set; }
    public TimeSpan? TimeOpen { get; set; }
    public TimeSpan? TimeClose { get; set; }
    public string Email { get; set; }
    public string UserModified { get; set; }
    public DateTime? LastModified { get; set; } = DateTime.UtcNow;
    public string UserApproved { get; set; }
}
public class SearchBranchModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string BranchID { get; set; }

    public string BranchName { get; set; }

    public string Address { get; set; }

    public string Phone { get; set; }
}
