namespace O24OpenAPI.Web.CMS.Models.Request;

/// <summary>

/// The send mail request model class

/// </summary>

/// <seealso cref="BaseTransactionModel"/>

public partial class SendMailRequestModel : BaseTransactionModel
{
    /// <summary>
    /// MailTemplateModel constructor
    /// </summary>
    public SendMailRequestModel() { }
    /// <summary>
    /// TemplateId
    /// </summary>
    public string TemplateId { get; set; } = string.Empty;
    /// <summary>
    /// Sender
    /// </summary>
    public string ConfigId { get; set; } = string.Empty;
    /// <summary>
    /// Receiver
    /// </summary>
    public string Receiver { get; set; } = string.Empty;
    /// <summary>
    /// DataTemplate
    /// </summary>
    public Dictionary<string, object> DataTemplate { get; set; } = new Dictionary<string, object>();
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public List<string> AttachmentBase64Strings { get; set; } = new List<string>();
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public List<string> AttachmentFilenames { get; set; } = new List<string>();
    // /// <summary>
    // ///
    // /// </summary>
    // [IgnoreDataMember]
    // [System.Text.Json.Serialization.JsonIgnore]
    public List<MimeEntity> MimeEntities { get; set; } = new List<MimeEntity>();


    /// <summary>
    /// The mime entity class
    /// </summary>
    public class MimeEntity
    {
        /// <summary>
        /// Gets or sets the value of the content type
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Gets or sets the value of the base 64
        /// </summary>
        public string Base64 { get; set; }
        /// <summary>
        /// Gets or sets the value of the content id
        /// </summary>
        public string ContentId { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public bool IncludeLogo { get; set; } = false;
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="int"></typeparam>
    /// <returns></returns>
    public List<int> FileIds { get; set; } = new List<int>();


}
