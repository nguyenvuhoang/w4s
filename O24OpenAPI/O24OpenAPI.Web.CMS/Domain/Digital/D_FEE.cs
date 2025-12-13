namespace O24OpenAPI.Web.CMS.Domain;

public class D_FEE : BaseEntity
{
    /// <summary>
    ///
    /// </summary>
    public D_FEE() { }
    /// <summary>
    ///
    /// </summary>
    public string FeeID { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string FeeName { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string FeeType { get; set; }
    /// <summary>
    ///
    /// </summary>
    public decimal FixAmount { get; set; }
    /// <summary>
    ///
    /// </summary>
    public bool IsLadder { get; set; }
    /// <summary>
    ///
    /// </summary>
    public bool IsRegionFee { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string ChargeLater { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string CCYID { get; set; }
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
    public DateTime DateModified { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string Branchid { get; set; }
    /// <summary>
    ///
    /// </summary>
    public bool IsBillPaymentFee { get; set; }


}
