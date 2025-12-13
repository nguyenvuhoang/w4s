namespace O24OpenAPI.O24NCH.Models.Request.Mail;

public class TestMailRequestModel : MailConfigSearchModel
{
    public string TemplateId { get; set; }
    public bool IncludeLogo { get; set; }
}
