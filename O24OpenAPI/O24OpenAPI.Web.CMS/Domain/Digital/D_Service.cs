namespace O24OpenAPI.Web.CMS.Domain;

public partial class D_SERVICE : BaseEntity
{
    /// <summary>
    ///
    /// </summary>
    public D_SERVICE() { }

    /// <summary>
    ///
    /// </summary>
    public string ServiceID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool BankService { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool CorpService { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool checkuseronline { get; set; }
}
