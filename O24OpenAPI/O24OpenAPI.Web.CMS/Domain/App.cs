namespace O24OpenAPI.Web.CMS.Domain;

public partial class App : BaseEntity
{
    public string AppCode { get; set; }
    public string AppName { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public bool Status { get; set; } = true;
    public DateTime? CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}
