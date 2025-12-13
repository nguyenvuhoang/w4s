namespace O24OpenAPI.Web.CMS.Domain;

public class TranslationEntry : BaseEntity
{
    public string Key { get; set; }
    public string English { get; set; }
    public string Vietnamese { get; set; }
    public string Lao { get; set; }
    public string Chinese { get; set; }
}
