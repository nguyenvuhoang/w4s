namespace O24OpenAPI.Web.CMS.Domain;

public partial class D_SECURITY_QUESTION : BaseEntity
{
    /// <summary>D_SECURITY_QUESTION
    ///
    /// </summary>
    public D_SECURITY_QUESTION() { }

    /// <summary>
    ///
    /// </summary>
    public string Question { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string LangID { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Status { get; set; }

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
}
