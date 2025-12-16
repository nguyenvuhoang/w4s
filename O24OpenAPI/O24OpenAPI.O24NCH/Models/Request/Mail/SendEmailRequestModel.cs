using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24NCH.Models.Request;

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
    ///
    public string TemplateId { get; set; } = string.Empty;

    /// <summary>
    /// Sender
    /// </summary>
    ///
    public string ConfigId { get; set; } = string.Empty;

    /// <summary>
    /// Receiver
    /// </summary>
    public string Receiver { get; set; } = string.Empty;

    /// <summary>
    /// DataTemplate
    /// </summary>
    ///
    public Dictionary<string, object> DataTemplate { get; set; } = [];

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public List<string> AttachmentBase64Strings { get; set; } = [];

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public List<string> AttachmentFilenames { get; set; } = [];

    // /// <summary>
    // ///
    // /// </summary>
    // [IgnoreDataMember]
    // [System.Text.Json.Serialization.JsonIgnore]
    public List<MimeEntity> MimeEntities { get; set; } = [];

    /// <summary>
    /// IncludeLogo
    /// </summary>
    public bool IncludeLogo { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="int"></typeparam>
    /// <returns></returns>
    public List<int> FileIds { get; set; } = new List<int>();
}
