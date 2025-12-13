namespace O24OpenAPI.Web.CMS.Domain;

public class TranslationLanguages : BaseEntity
{
    public string ChannelId { get; set; }
    public string Language { get; set; }
    public string JSONContent { get; set; }
    public string Version { get; set; }
    public string UserCreated { get; set; }
    public string UserModified { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
