using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.NCH.API.Application.Models.SMSTemplate;

/// <summary>
/// The templatetransfermodel class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
public class SMSTemplateModel : BaseTransactionModel
{
    public string TemplateCode { get; set; } = string.Empty;
    public string MessageContent { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime? CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}
