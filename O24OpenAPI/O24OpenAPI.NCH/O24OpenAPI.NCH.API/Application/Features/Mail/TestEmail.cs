using LinKit.Core.Cqrs;
using MailKit.Security;
using MimeKit;
using O24OpenAPI.Framework;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.Config;
using O24OpenAPI.NCH.Constant;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace O24OpenAPI.NCH.API.Application.Features.Mail;

public class TestEmailCommand : BaseTransactionModel, ICommand<bool>
{
    public string TemplateId { get; set; }
    public bool IncludeLogo { get; set; }
    public string ConfigId { get; set; }

    /// <summary>
    /// Host
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Sender
    /// </summary>
    public string Sender { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// EnableTLS
    /// </summary>
    public bool EnableTLS { get; set; }

    /// <summary>
    /// EmailTest
    /// </summary>
    public string EmailTest { get; set; }

    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = int.MaxValue;
}

[CqrsHandler]
public class TestEmailHandle(
    IMailTemplateRepository mailTemplateRepository,
    WebApiSettings webApiSettings,
    O24NCHSetting o24NCHSetting
) : ICommandHandler<TestEmailCommand, bool>
{
    private static readonly string[] Separator = [";"];

    public async Task<bool> HandleAsync(
        TestEmailCommand request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            MailTemplate getMailTemplate =
                await mailTemplateRepository.GetByTemplateId(request.TemplateId)
                ?? throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Validation.MailTemplateNotExist,
                    request.Language
                );
            MimeMessage email = new() { Sender = MailboxAddress.Parse(request.Sender) };

            foreach (
                string address in request.EmailTest.Split(
                    separator,
                    StringSplitOptions.RemoveEmptyEntries
                )
            )
            {
                email.To.Add(MailboxAddress.Parse(address));
            }

            email.Subject = getMailTemplate.Subject;

            BodyBuilder builder = new();

            // Add logo if required
            if (request.IncludeLogo)
            {
                try
                {
                    MimePart footer = new("image", "png")
                    {
                        ContentId = "logo_footer",
                        Content = new MimeContent(
                            new MemoryStream(
                                Convert.FromBase64String(webApiSettings.LogoBankFooter)
                            )
                        ),
                    };
                    MimePart header = new("image", "png")
                    {
                        ContentId = "logo_header",
                        Content = new MimeContent(
                            new MemoryStream(
                                Convert.FromBase64String(webApiSettings.LogoBankHeader)
                            )
                        ),
                    };

                    MimePart iconphone = new("image", "png")
                    {
                        ContentId = "iconphone",
                        Content = new MimeContent(
                            new MemoryStream(Convert.FromBase64String(o24NCHSetting.IconPhone))
                        ),
                    };

                    MimePart iconwebsite = new("image", "png")
                    {
                        ContentId = "iconwebsite",
                        Content = new MimeContent(
                            new MemoryStream(Convert.FromBase64String(o24NCHSetting.IconWebsite))
                        ),
                    };

                    builder.LinkedResources.Add(footer);
                    builder.LinkedResources.Add(header);
                    builder.LinkedResources.Add(iconphone);
                    builder.LinkedResources.Add(iconwebsite);
                }
                catch
                {
                    // Ignore invalid logos
                }
            }

            builder.HtmlBody = getMailTemplate.Body;
            email.Body = builder.ToMessageBody();

            // SMTP send
            using SmtpClient smtp = new();
            await smtp.ConnectAsync(
                request.Host,
                request.Port,
                request.EnableTLS
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.StartTlsWhenAvailable
            );
            await smtp.AuthenticateAsync(request.Sender, request.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            return true;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            return false;
        }
    }

    private static readonly string[] separator = [";"];
}
