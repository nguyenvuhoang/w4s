using System.Text.Json;
using LinKit.Core.Cqrs;
using MailKit.Security;
using MimeKit;
using O24OpenAPI.Framework;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Constants;
using O24OpenAPI.NCH.API.Application.Models.Request;
using O24OpenAPI.NCH.API.Application.Utils;
using O24OpenAPI.NCH.Config;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace O24OpenAPI.NCH.API.Application.Features.Mail;

public class SendEmailCommand : BaseTransactionModel, ICommand<bool>
{
    public string TemplateId { get; set; } = string.Empty;
    public string ConfigId { get; set; } = string.Empty;
    public string Receiver { get; set; } = string.Empty;
    public Dictionary<string, object> DataTemplate { get; set; } = [];
    public List<string> AttachmentBase64Strings { get; set; } = [];
    public List<string> AttachmentFilenames { get; set; } = [];
    public List<O24MimeEntity> MimeEntities { get; set; } = [];
    public bool IncludeLogo { get; set; } = false;
    public List<int> FileIds { get; set; } = [];
}

[CqrsHandler]
public class SendEmailHandle(
    IMailConfigRepository mailConfigRepository,
    IMailTemplateRepository mailTemplateRepository,
    WebApiSettings webApiSettings,
    O24NCHSetting o24NCHSetting,
    IEmailSendOutRepository emailSendOutRepository
) : ICommandHandler<SendEmailCommand, bool>
{
    public async Task<bool> HandleAsync(
        SendEmailCommand request,
        CancellationToken cancellationToken = default
    )
    {
        EmailSendOut emailLog = new()
        {
            ConfigId = request.ConfigId,
            TemplateId = request.TemplateId,
            Receiver = !string.IsNullOrWhiteSpace(request.Receiver)
                ? request.Receiver
                : (
                    request.DataTemplate != null
                    && request.DataTemplate.TryGetValue("email", out object? emailVal)
                        ? emailVal?.ToString()
                        : null
                ),
            Status = MailSendOutStatus.PENDING,
            CreatedAt = DateTime.UtcNow,
        };
        try
        {
            MailConfig? getMailConfig = await mailConfigRepository.GetByConfigId(request.ConfigId);
            MailTemplate? getMailTemplate = await mailTemplateRepository.GetByTemplateId(
                request.TemplateId
            );

            if (getMailConfig == null)
            {
                throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Validation.MailConfigNotExist,
                    request.Language
                );
            }

            if (getMailTemplate == null)
            {
                throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Validation.MailTemplateNotExist,
                    request.Language
                );
            }

            MimeMessage email = new() { Sender = MailboxAddress.Parse(getMailConfig.Sender) };

            if (string.IsNullOrWhiteSpace(emailLog.Receiver))
            {
                throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Validation.MailReceiverNotFound,
                    request.Language
                );
            }

            foreach (
                string address in emailLog.Receiver.Split(
                    separator,
                    StringSplitOptions.RemoveEmptyEntries
                )
            )
            {
                email.To.Add(MailboxAddress.Parse(address));
            }

            email.Subject = Utility.ReplaceData(getMailTemplate.Subject, request.DataTemplate);
            emailLog.Subject = email.Subject;
            BodyBuilder builder = new();

            // Handle attachments
            if (
                request.AttachmentBase64Strings != null
                && request.AttachmentBase64Strings.Count > 0
            )
            {
                for (int i = 0; i < request.AttachmentBase64Strings.Count; i++)
                {
                    string base64 = request.AttachmentBase64Strings[i];
                    string filename = request.AttachmentFilenames[i];
                    try
                    {
                        byte[] bytes = Convert.FromBase64String(base64);
                        builder.Attachments.Add(filename, bytes);
                    }
                    catch
                    {
                        // Ignore invalid attachment
                        continue;
                    }
                }
                emailLog.Attachments = JsonSerializer.Serialize(request.AttachmentFilenames);
            }

            // Linked resources (images or inline files)
            if (request.MimeEntities != null)
            {
                foreach (O24MimeEntity entity in request.MimeEntities)
                {
                    try
                    {
                        builder.LinkedResources.Add(
                            Utility.ConvertBase64ToMimeEntity(
                                entity.Base64,
                                entity.ContentType,
                                entity.ContentId
                            )
                        );
                    }
                    catch
                    {
                        // Ignore invalid base64 images
                        continue;
                    }
                }
            }

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

            builder.HtmlBody = Utility.ReplaceData(getMailTemplate.Body, request.DataTemplate);
            email.Body = builder.ToMessageBody();
            emailLog.Body = builder.HtmlBody;
            // SMTP send
            using SmtpClient smtp = new();
            await smtp.ConnectAsync(
                getMailConfig.Host,
                getMailConfig.Port,
                getMailConfig.EnableTLS
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.StartTlsWhenAvailable
            );
            await smtp.AuthenticateAsync(getMailConfig.Sender, getMailConfig.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            emailLog.Status = MailSendOutStatus.SUCCESS;
            emailLog.SentAt = DateTime.UtcNow;
            await emailSendOutRepository.Create(emailLog);

            return true;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            emailLog.Status = MailSendOutStatus.FAILED;
            emailLog.ErrorMessage = ex.Message;
            emailLog.SentAt = DateTime.UtcNow;

            try
            {
                await emailSendOutRepository.Create(emailLog);
            }
            catch { }
            return false;
        }
    }

    private static readonly string[] separator = [";"];
}
