using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24NCH.Domain;

public partial class TelegramChatMapping : BaseEntity
{
    public string UserCode { get; set; }
    public string PhoneNumber { get; set; }
    public string ChatId { get; set; }
    public string TelegramUsername { get; set; }
    public string Fullname { get; set; }
    public string LanguageCode { get; set; }
    public bool? IsBot { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}
