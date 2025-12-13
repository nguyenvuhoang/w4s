namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>
/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class GetTelcoByTelcoCodeModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public int TelcoID { get; set; }
}

/// <summary>
/// The bankmodel class
/// </summary>
/// <seealso cref="BaseEntity"/>

public class GetTelcoByTelcoNameModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string TelcoName { get; set; }
}


public class TelcoModel : BaseTransactionModel
{
    public string TelcoCode { get; set; }
    public string TelcoName { get; set; }
    public string ELoadBillerCode { get; set; }
    public string EPinBillerCode { get; set; }
    public string ShortName { get; set; }
    public string ELoad { get; set; }
    public string EPin { get; set; }
    public string Status { get; set; }
    public string SUNDRYACCTNOBANK { get; set; }
    public string INCOMEACCTNOBANK { get; set; }
    public string SUNDRYACCTNOWALLET { get; set; }
    public string INCOMEACCTNOWALLET { get; set; }



}

public class TelcoUpdateModel : BaseTransactionModel
{
    public int TelcoID { get; set; }
    public string TelcoCode { get; set; }

    public string TelcoName { get; set; }
    public string ELoadBillerCode { get; set; }
    public string EPinBillerCode { get; set; }
    public string ShortName { get; set; }
    public string ELoad { get; set; }
    public string EPin { get; set; }
    public string Status { get; set; }
    public string SUNDRYACCTNOBANK { get; set; }
    public string INCOMEACCTNOBANK { get; set; }
    public string SUNDRYACCTNOWALLET { get; set; }
    public string INCOMEACCTNOWALLET { get; set; }


}
public class SearchTelcoModel : BaseTransactionModel
{
    public string TelcoCode { get; set; }
    public string TelcoName { get; set; }
    public string ELoadBillerCode { get; set; }
    public string EPinBillerCode { get; set; }
    public string ShortName { get; set; }
    public string Status { get; set; }
    public string SUNDRYACCTNOBANK { get; set; }
    public string SUNDRYACCTNOWALLET { get; set; }


}
