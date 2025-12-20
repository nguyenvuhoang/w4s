using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Models.Request;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class MailWorkflowService : BaseQueueService
{
    private readonly IMailConfigService _contextMailConfig =
        EngineContext.Current.Resolve<IMailConfigService>();
    private readonly IMailTemplateService _contextMailTemplate =
        EngineContext.Current.Resolve<IMailTemplateService>();
    private readonly WebApiSettings _setting = EngineContext.Current.Resolve<WebApiSettings>();

    public async Task<WorkflowScheme> SendMail(WorkflowScheme workflow)
    {
        Console.WriteLine("workflow===");
        var model = await workflow.ToModel<SendMailRequestModel>();
        Console.WriteLine("model===" + JsonConvert.SerializeObject(model));
        return await Invoke<SendMailRequestModel>(
            workflow,
            async () =>
            {
                await SendMailToClient(model);
                return model;
            }
        );
    }

    private async Task SendMailToClient(SendMailRequestModel mailRequest)
    {
        try
        {
            var getMailConfig = await _contextMailConfig.GetByConfigId(mailRequest.ConfigId);
            var getMailTemplate = await _contextMailTemplate.GetByTemplateId(
                mailRequest.TemplateId
            );
            if (getMailConfig == null || getMailTemplate == null)
            {
                throw new O24OpenAPIException("Mail Config or Mail Template not found");
            }

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(getMailConfig.Sender);

            foreach (
                var address in mailRequest.Receiver.Split(
                    new[] { ";" },
                    StringSplitOptions.RemoveEmptyEntries
                )
            )
            {
                email.To.Add(MailboxAddress.Parse(address));
            }

            email.Subject = ReplaceData(getMailTemplate.Subject, mailRequest.DataTemplate);

            var builder = new BodyBuilder();
            if (
                mailRequest.AttachmentBase64Strings != null
                && mailRequest.AttachmentBase64Strings.Count > 0
            )
            {
                List<byte[]> attachmentByteArrays = mailRequest
                    .AttachmentBase64Strings.Select(base64String =>
                        Convert.FromBase64String(base64String)
                    )
                    .ToList();
                for (int i = 0; i < attachmentByteArrays.Count; i++)
                {
                    byte[] byteArray = attachmentByteArrays[i];
                    string filename = mailRequest.AttachmentFilenames[i];

                    builder.Attachments.Add(filename, byteArray);
                }
            }

            if (mailRequest.MimeEntities != null && mailRequest.MimeEntities.Count > 0)
            {
                foreach (var entity in mailRequest.MimeEntities)
                {
                    builder.LinkedResources.Add(
                        ConvertBase64ToMimeEntity(
                            entity.Base64,
                            entity.ContentType,
                            entity.ContentId
                        )
                    );
                }
            }

            if (mailRequest.IncludeLogo)
            {
                var imagePartFooter = new MimePart(new ContentType("image", "png"))
                {
                    ContentId = "logo_footer",
                    Content = new MimeContent(
                        new MemoryStream(Convert.FromBase64String(_setting.LogoBankFooter))
                    ),
                };
                var imagePartHeader = new MimePart(new ContentType("image", "png"))
                {
                    ContentId = "logo_header",
                    Content = new MimeContent(
                        new MemoryStream(Convert.FromBase64String(_setting.LogoBankHeader))
                    ),
                };
                builder.LinkedResources.Add(imagePartFooter);
                builder.LinkedResources.Add(imagePartHeader);
            }

            builder.HtmlBody = ReplaceData(getMailTemplate.Body, mailRequest.DataTemplate);
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(
                getMailConfig.Host,
                getMailConfig.Port,
                getMailConfig.EnableTLS
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.StartTlsWhenAvailable
            );
            smtp.Authenticate(getMailConfig.Sender, getMailConfig.Password);

            //

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        catch (System.Exception ex)
        {
            throw new O24OpenAPIException(ex.ToString());
        }
    }

    public static MimePart ConvertBase64ToMimeEntity(
        string base64String,
        string contentType,
        string contentId
    )
    {
        byte[] fileBytes = Convert.FromBase64String(base64String);
        var memoryStream = new MemoryStream(fileBytes);
        var mimePart = new MimePart(contentType)
        {
            Content = new MimeContent(memoryStream),
            ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
            ContentTransferEncoding = ContentEncoding.Base64,
            ContentId = contentId,
        };

        return mimePart;
    }

    private static string ReplaceData(string para, Dictionary<string, object> data)
    {
        if (para == null || data == null)
        {
            return para;
        }

        string pattern = "@\\{([^\\{]*)\\}";

        foreach (Match match in Regex.Matches(para, pattern))
        {
            if (match.Success && match.Groups.Count > 0)
            {
                var text = match.Groups[1].Value;
                try
                {
                    var value = JObject.Parse(System.Text.Json.JsonSerializer.Serialize(data));
                    if (value.SelectToken(text) != null)
                    {
                        para = para.Replace(
                            match.Groups[0].Value,
                            value.SelectToken(text).ToString()
                        );
                    }
                }
                catch (System.Exception ex)
                {
                    // TODO
                    System.Console.WriteLine(
                        "CANT FIND "
                            + text
                            + " IN DATA SAMPLE "
                            + System.Text.Json.JsonSerializer.Serialize(data)
                            + ex.StackTrace
                    );
                }
            }
        }

        return para;
    }
}
