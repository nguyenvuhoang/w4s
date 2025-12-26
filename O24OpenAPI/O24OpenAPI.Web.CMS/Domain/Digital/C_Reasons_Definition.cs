namespace O24OpenAPI.Web.CMS.Domain;

public partial class C_REASONS_DEFINITION : BaseEntity
{
    /// <summary>
    ///
    /// </summary>
    public C_REASONS_DEFINITION() { }

    /// <summary>
    ///
    /// </summary>
    public decimal ReasonID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonAction { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonType { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ReasonEvent { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Status { get; set; }
}
