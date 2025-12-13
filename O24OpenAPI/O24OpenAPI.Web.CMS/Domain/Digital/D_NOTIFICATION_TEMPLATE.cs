namespace O24OpenAPI.Web.CMS.Domain;

public class D_NOTIFICATION_TEMPLATE : BaseEntity
{
    public string TemplateID { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Data { get; set; }
    public string LearnApiSending { get; set; }
    public bool IsShowButton { get; set; } = false;
}
