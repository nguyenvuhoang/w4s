namespace O24OpenAPI.Web.CMS.Domain;

public class LearnApiCache : BaseEntity
{
    public string LearnApiId { get; set; }
    public string LearnApiIdClear { get; set; }
    public int? CacheTime { get; set; }
}
