namespace O24OpenAPI.CMS.Domain;

public partial class MailTemplate : BaseEntity
{
    /// <summary>
    ///
    /// </summary>
    public MailTemplate() { }

    /// <summary>
    /// TemplateId
    /// </summary>
    public string TemplateId { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Subject
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Body
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// EmailTest
    /// </summary>
    public string DataSample { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool SendAsPDF { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Attachments { get; set; }
}
