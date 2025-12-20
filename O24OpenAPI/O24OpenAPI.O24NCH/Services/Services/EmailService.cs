using System.Text.Json;
using MailKit.Security;
using MimeKit;
using O24OpenAPI.Framework;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.O24NCH.Config;
using O24OpenAPI.O24NCH.Constant;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.Request.Mail;
using O24OpenAPI.O24NCH.Services.Interfaces;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Utility = O24OpenAPI.O24NCH.Utils.Utility;

namespace O24OpenAPI.O24NCH.Services.Services;

public class EmailService(
    IMailConfigService contextMailConfig,
    IMailTemplateService contextMailTemplate,
    IMailSendOutService contextMailSendOut,
    O24NCHSetting nchSetting,
    WebApiSettings webApiSettings
) : IEmailService
{
    private readonly IMailConfigService _contextMailConfig = contextMailConfig;
    private readonly IMailTemplateService _contextMailTemplate = contextMailTemplate;
    private static readonly string[] separator = [";"];
    private readonly O24NCHSetting _setting = nchSetting;
    private readonly WebApiSettings _webApiSettings = webApiSettings;

    public async Task<bool> SendEmailAsync(SendMailRequestModel model)
    {
        EmailSendOut emailLog = new()
        {
            ConfigId = model.ConfigId,
            TemplateId = model.TemplateId,
            Receiver = !string.IsNullOrWhiteSpace(model.Receiver)
                ? model.Receiver
                : (
                    model.DataTemplate != null
                    && model.DataTemplate.TryGetValue("email", out var emailVal)
                        ? emailVal?.ToString()
                        : null
                ),
            Status = MailSendOutStatus.PENDING,
            CreatedAt = DateTime.UtcNow,
        };
        try
        {
            var getMailConfig = await _contextMailConfig.GetByConfigId(model.ConfigId);
            var getMailTemplate = await _contextMailTemplate.GetByTemplateId(model.TemplateId);

            if (getMailConfig == null)
            {
                throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Validation.MailConfigNotExist,
                    model.Language
                );
            }

            if (getMailTemplate == null)
            {
                throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Validation.MailTemplateNotExist,
                    model.Language
                );
            }

            var email = new MimeMessage { Sender = MailboxAddress.Parse(getMailConfig.Sender) };

            if (string.IsNullOrWhiteSpace(emailLog.Receiver))
            {
                throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Validation.MailReceiverNotFound,
                    model.Language
                );
            }

            foreach (
                var address in emailLog.Receiver.Split(
                    separator,
                    StringSplitOptions.RemoveEmptyEntries
                )
            )
            {
                email.To.Add(MailboxAddress.Parse(address));
            }

            email.Subject = Utility.ReplaceData(getMailTemplate.Subject, model.DataTemplate);
            emailLog.Subject = email.Subject;
            var builder = new BodyBuilder();

            // Handle attachments
            if (model.AttachmentBase64Strings != null && model.AttachmentBase64Strings.Count > 0)
            {
                for (int i = 0; i < model.AttachmentBase64Strings.Count; i++)
                {
                    var base64 = model.AttachmentBase64Strings[i];
                    var filename = model.AttachmentFilenames[i];
                    try
                    {
                        var bytes = Convert.FromBase64String(base64);
                        builder.Attachments.Add(filename, bytes);
                    }
                    catch
                    {
                        // Ignore invalid attachment
                        continue;
                    }
                }
                emailLog.Attachments = JsonSerializer.Serialize(model.AttachmentFilenames);
            }

            // Linked resources (images or inline files)
            if (model.MimeEntities != null)
            {
                foreach (var entity in model.MimeEntities)
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
            if (model.IncludeLogo)
            {
                try
                {
                    var footer = new MimePart("image", "png")
                    {
                        ContentId = "logo_footer",
                        Content = new MimeContent(
                            new MemoryStream(
                                Convert.FromBase64String(_webApiSettings.LogoBankFooter)
                            )
                        ),
                    };
                    var header = new MimePart("image", "png")
                    {
                        ContentId = "logo_header",
                        Content = new MimeContent(
                            new MemoryStream(
                                Convert.FromBase64String(_webApiSettings.LogoBankHeader)
                            )
                        ),
                    };

                    var iconphone = new MimePart("image", "png")
                    {
                        ContentId = "iconphone",
                        Content = new MimeContent(
                            new MemoryStream(Convert.FromBase64String(_setting.IconPhone))
                        ),
                    };

                    var iconwebsite = new MimePart("image", "png")
                    {
                        ContentId = "iconwebsite",
                        Content = new MimeContent(
                            new MemoryStream(Convert.FromBase64String(_setting.IconWebsite))
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

            builder.HtmlBody = Utility.ReplaceData(getMailTemplate.Body, model.DataTemplate);
            email.Body = builder.ToMessageBody();
            emailLog.Body = builder.HtmlBody;
            // SMTP send
            using var smtp = new SmtpClient();
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
            await contextMailSendOut.Create(emailLog);

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
                await contextMailSendOut.Create(emailLog);
            }
            catch { }
            return false;
        }
    }

    public async Task<bool> TestEmailAsync(TestMailRequestModel model)
    {
        try
        {
            var getMailTemplate =
                await _contextMailTemplate.GetByTemplateId(model.TemplateId)
                ?? throw await O24Exception.CreateAsync(
                    O24NCHResourceCode.Validation.MailTemplateNotExist,
                    model.Language
                );
            var email = new MimeMessage { Sender = MailboxAddress.Parse(model.Sender) };

            foreach (
                var address in model.EmailTest.Split(
                    separator,
                    StringSplitOptions.RemoveEmptyEntries
                )
            )
            {
                email.To.Add(MailboxAddress.Parse(address));
            }

            email.Subject = getMailTemplate.Subject;

            var builder = new BodyBuilder();

            // Add logo if required
            if (model.IncludeLogo)
            {
                try
                {
                    var footer = new MimePart("image", "png")
                    {
                        ContentId = "logo_footer",
                        Content = new MimeContent(
                            new MemoryStream(
                                Convert.FromBase64String(_webApiSettings.LogoBankFooter)
                            )
                        ),
                    };
                    var header = new MimePart("image", "png")
                    {
                        ContentId = "logo_header",
                        Content = new MimeContent(
                            new MemoryStream(
                                Convert.FromBase64String(_webApiSettings.LogoBankHeader)
                            )
                        ),
                    };

                    var iconphone = new MimePart("image", "png")
                    {
                        ContentId = "iconphone",
                        Content = new MimeContent(
                            new MemoryStream(Convert.FromBase64String(_setting.IconPhone))
                        ),
                    };

                    var iconwebsite = new MimePart("image", "png")
                    {
                        ContentId = "iconwebsite",
                        Content = new MimeContent(
                            new MemoryStream(Convert.FromBase64String(_setting.IconWebsite))
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
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                model.Host,
                model.Port,
                model.EnableTLS
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.StartTlsWhenAvailable
            );
            await smtp.AuthenticateAsync(model.Sender, model.Password);
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
}
