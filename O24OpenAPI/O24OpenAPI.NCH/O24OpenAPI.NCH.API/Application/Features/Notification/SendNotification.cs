using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LinKit.Core.Cqrs;
using LinqToDB;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Utils;
using O24OpenAPI.NCH.Common;
using O24OpenAPI.NCH.Config;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;
using O24OpenAPI.NCH.Domain.Constants;
using O24OpenAPI.NCH.Infrastructure.Repositories;
using O24OpenAPI.NCH.Models.Request;
using O24OpenAPI.NCH.Models.Request.Mail;
using O24OpenAPI.NCH.Models.Request.Telegram;
using O24OpenAPI.NCH.Models.Response;

namespace O24OpenAPI.NCH.API.Application.Features.Notification
{
    public class SendNotificationCommand : BaseTransactionModel, ICommand<bool>
    {
        public string UserCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Purpose { get; set; }
        public string NotificationType { get; set; }
        public string Email { get; set; }
        public Dictionary<string, object> SenderData { get; set; }
        public Dictionary<string, object> DataTemplate { get; set; } = [];
        public List<string> AttachmentBase64Strings { get; set; } = [];
        public List<string> AttachmentFilenames { get; set; } = [];
        public List<Models.Request.MimeEntity> MimeEntities { get; set; } = [];
        public List<int> FileIds { get; set; } = [];
        public string Message { get; set; } = string.Empty;
    }

    [CqrsHandler]
    public class SendNotificationHandler(
        IEmailSendOutRepository emailSendOutRepository,
        IMailConfigRepository mailConfigRepository,
        IMailTemplateRepository mailTemplateRepository,
        WebApiSettings webApiSettings,
        O24NCHSetting o24NCHSetting,
        ISMSProviderRepository sMSProviderRepository,
        ISMSTemplateRepository sMSTemplateRepository,
        ISMSProviderConfigRepository sMSProviderConfigRepository,
        ISMSSendOutRepository sMSSendOutRepository,
        ISMSMappingRespository sMSMappingRespository
    ) : ICommandHandler<SendNotificationCommand, bool>
    {
        [WorkflowStep(WorkflowStep.NCH.WF_STEP_CTH_SEND_NOTIFICATION)]
        Task<bool> IHandler<SendNotificationCommand, bool>.HandleAsync(
            SendNotificationCommand request,
            CancellationToken cancellationToken
        )
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendNotification(NotificationRequestModel model)
        {
            var workContext = EngineContext.Current.Resolve<WorkContext>();
            model.RefId = workContext.ExecutionId ?? Guid.NewGuid().ToString();
            try
            {
                var type = (model?.NotificationType ?? "").Trim().ToUpperInvariant();

                switch (type)
                {
                    case "MAIL":
                        return await SendEmailAsync(MapToSendMailModel(model));

                    case "SMS":
                        var sms = model as SMSRequestModel ?? MapToSms(model);
                        return await SendSMS(sms);

                    case "TELE":
                        var tele = model as TelegramSendModel ?? MapToTelegram(model);
                        return await SendMessage(tele);

                    default:
                        throw new NotSupportedException(
                            $"Notification type '{model?.NotificationType}' is not supported."
                        );
                }
            }
            catch (Exception ex)
            {
                _ = ex.LogErrorAsync(
                    new Dictionary<string, object?>
                    {
                        ["RuntimeType"] = model?.GetType().FullName,
                        ["NotificationType"] = model?.NotificationType,
                    }
                );
                return false;
            }
        }

        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<bool> SendMessage(TelegramSendModel model)
        {
            if (string.IsNullOrEmpty(model.ChatId) || string.IsNullOrEmpty(model.Message))
            {
                return false;
            }

            var _botToken = "8197855911:AAHId-nVPso_dkkBTsE5WHO-Ae0dp9HzMpE";
            var requestUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage";

            var payload = new
            {
                chat_id = model.ChatId,
                text = model.Message,
                parse_mode = "Markdown",
            };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.PostAsync(requestUrl, content);

            return response.IsSuccessStatusCode;
        }

        private static TelegramSendModel MapToTelegram(NotificationRequestModel m)
        {
            return new TelegramSendModel
            {
                NotificationType = "TELE", /*...*/
            };
        }

        public async Task<bool> SendSMS(SMSRequestModel model)
        {
            try
            {
                var message = string.IsNullOrWhiteSpace(model.Message)
                    ? await sMSTemplateRepository.BuildSMSContentDynamicAsync(
                        model.Purpose,
                        model.SenderData
                    )
                    : model.Message;

                var transactionId = model.RefId;
                var sendResult = await SendSMSAsync(model.PhoneNumber, message, transactionId);
                return sendResult.IsSuccess;
            }
            catch (Exception ex)
            {
                _ = ex.LogErrorAsync();
                return false;
            }
        }

        public async Task<SendSOAPResponseModel> SendSMSAsync(
            string phoneNumber,
            string message,
            string transactionId,
            string providerName = "UNITEL",
            string endtoend = "",
            string messagetype = "SMS"
        )
        {
            string soapXml = null;

            var providerMain = await sMSProviderRepository.GetProviderByPhoneNumber(phoneNumber);

            if (providerMain != null)
            {
                providerName = providerMain.ProviderName;
            }

            var provider = await sMSProviderRepository.Table.FirstOrDefaultAsync(p =>
                p.ProviderName == providerName && p.IsActive
            );

            if (provider == null)
            {
                return new SendSOAPResponseModel
                {
                    TransactionId = transactionId,
                    MessageId = null,
                    ErrorCode = 8888,
                    ErrorMessage = "Provider is not exist",
                    ProviderKey = null,
                    RawResponseCode = null,
                    IsSuccess = false,
                };
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                if (phoneNumber.StartsWith('0'))
                {
                    if (providerName == "ETL")
                    {
                        phoneNumber = "856" + phoneNumber[1..];
                    }
                    else if (providerName == "LTC") { }
                    else
                    {
                        phoneNumber = phoneNumber[1..];
                    }
                }
            }

            try
            {
                var transactionIdFinal = endtoend ?? transactionId ?? Guid.NewGuid().ToString();
                soapXml = await sMSProviderConfigRepository.BuildSOAPRequestAsync(
                    provider,
                    new Dictionary<string, string>
                    {
                        { "RECEIVERPHONE", phoneNumber },
                        { "MSGCONTENT", message },
                        { "TRANSACTIONID", transactionIdFinal },
                    }
                );

                return await SendSOAPRequestAndLogAsync(
                    provider,
                    phoneNumber,
                    message,
                    soapXml,
                    transactionIdFinal,
                    endtoend
                );
            }
            catch (Exception ex)
            {
                await ex.LogErrorAsync();
                await sMSSendOutRepository.InsertAsync(
                    new SMSSendOut
                    {
                        PhoneNumber = phoneNumber,
                        MessageContent = message,
                        SentAt = DateTime.UtcNow,
                        SMSProviderId = provider.ProviderName,
                        Status = "Failed",
                        ResponseMessage = $"Exception: {ex.Message}",
                        OtpRequestId = transactionId ?? Guid.NewGuid().ToString(),
                        RequestMessage = soapXml,
                    }
                );

                return new SendSOAPResponseModel
                {
                    TransactionId = transactionId,
                    MessageId = null,
                    ErrorCode = 9999,
                    ErrorMessage = ex.Message,
                    ProviderKey = null,
                    RawResponseCode = null,
                    IsSuccess = false,
                };
            }
        }

        private static readonly SocketsHttpHandler _sharedHandler = new()
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
            MaxConnectionsPerServer = 100,
        };
        private static readonly HttpClient _sharedHttpClient = new(_sharedHandler)
        {
            Timeout = TimeSpan.FromSeconds(30),
        };

        public async Task<SendSOAPResponseModel> SendSOAPRequestAndLogAsync(
            SMSProvider provider,
            string phoneNumber,
            string messageContent,
            string soapXml,
            string transactionId,
            string endtoend = "",
            int retryCount = 0,
            bool isResend = false,
            HttpClient httpClient = null,
            CancellationToken cancellationToken = default
        )
        {
            httpClient ??= _sharedHttpClient;

            var wsdlUrl = await sMSProviderConfigRepository
                .Table.Where(x =>
                    x.SMSProviderId == provider.ProviderName
                    && x.ConfigKey == "WSDL_URL"
                    && x.IsActive
                )
                .Select(x => x.ConfigValue)
                .FirstOrDefaultAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(wsdlUrl))
            {
                throw new Exception("WSDL_URL not found.");
            }

            var sendOut = sMSSendOutRepository.CreateSendOut(
                provider.ProviderName,
                phoneNumber,
                messageContent,
                soapXml,
                transactionId,
                endtoend,
                0,
                isResend
            );

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var contentType =
                    (
                        await sMSProviderConfigRepository.GetConfigValueAsync(
                            provider.ProviderName,
                            "SOAP_CONTENT_TYPE"
                        )
                    )?.Trim() ?? "application/soap+xml; charset=utf-8";
                using var content = new StringContent(soapXml, Encoding.UTF8);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
                var requestHeader = await sMSProviderConfigRepository.AddDynamicSoapHeaders(
                    content,
                    provider.ProviderName
                );
                sendOut.RequestHeader = requestHeader;
                sendOut.FullRequest = BuildFullHttpRequest(wsdlUrl, "POST", content);

                var response = await httpClient.PostAsync(wsdlUrl, content, cancellationToken);
                var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

                stopwatch.Stop();

                var allMappings = await sMSMappingRespository.Table.ToListAsync(cancellationToken);
                var successMap = allMappings
                    .Where(x => x.IsSuccess)
                    .GroupBy(x => x.ProviderName.ToUpper())
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ResponseCode).ToHashSet());

                var (msgId, isSuccess, resultCode, resultDesc, providerKey, rawCode) =
                    O24SMSResponseParser.ExtractMsgIdAndStatusFromSOAP(responseString, successMap);

                sendOut.ElapsedMilliseconds = (int)stopwatch.ElapsedMilliseconds;
                sendOut.ResponseMessage = responseString;
                sendOut.ProviderMsgId = msgId;
                sendOut.Status = isSuccess ? SMSSendOutStatus.SUCCESS : SMSSendOutStatus.FAILED;

                await sMSSendOutRepository.InsertAsync(sendOut);

                return new SendSOAPResponseModel
                {
                    TransactionId = transactionId,
                    MessageId = msgId,
                    ErrorCode = resultCode,
                    ErrorMessage = resultDesc,
                    ProviderKey = providerKey,
                    RawResponseCode = rawCode,
                    IsSuccess = isSuccess,
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                sendOut.ElapsedMilliseconds = (int)stopwatch.ElapsedMilliseconds;
                sendOut.Status = SMSSendOutStatus.FAILED;
                sendOut.ResponseMessage = $"Exception: {ex.Message}";

                await sMSSendOutRepository.InsertAsync(sendOut);
                await ex.LogErrorAsync();

                return new SendSOAPResponseModel
                {
                    TransactionId = transactionId,
                    MessageId = null,
                    ErrorCode = 9999,
                    ErrorMessage = ex.Message,
                    ProviderKey = null,
                    RawResponseCode = null,
                    IsSuccess = false,
                };
            }
        }

        private static string BuildFullHttpRequest(string url, string method, HttpContent content)
        {
            var requestLog = new StringBuilder();

            requestLog.AppendLine($"{method} {url} HTTP/1.1");

            foreach (var header in content.Headers)
            {
                foreach (var value in header.Value)
                {
                    requestLog.AppendLine($"{header.Key}: {value}");
                }
            }

            requestLog.AppendLine();

            var body = content.ReadAsStringAsync().Result;
            requestLog.AppendLine(body);

            return requestLog.ToString();
        }

        private static SMSRequestModel MapToSms(NotificationRequestModel m)
        {
            return m is null
                ? throw new ArgumentNullException(nameof(m))
                : new SMSRequestModel
                {
                    NotificationType = "SMS",
                    Purpose = m.Purpose,
                    PhoneNumber = m.PhoneNumber,
                    SenderData = m.SenderData ?? [],
                    RefId = m.RefId,
                    Message = m.Message,
                };
        }

        private static SendMailRequestModel MapToSendMailModel(NotificationRequestModel model)
        {
            return new SendMailRequestModel
            {
                TemplateId = $"{model.ChannelId}_MAIL_RESET_PASSWORD",
                ConfigId = "main_mail",
                Receiver = model.Email,
                DataTemplate = model.DataTemplate ?? [],
                AttachmentBase64Strings = model.AttachmentBase64Strings ?? [],
                AttachmentFilenames = model.AttachmentFilenames ?? [],
                MimeEntities = model.MimeEntities ?? [],
                IncludeLogo = true,
                FileIds = model.FileIds ?? [],
            };
        }

        private static readonly string[] separator = [";"];

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
                var getMailConfig = await mailConfigRepository.GetByConfigId(model.ConfigId);
                var getMailTemplate = await mailTemplateRepository.GetByTemplateId(
                    model.TemplateId
                );

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
                if (
                    model.AttachmentBase64Strings != null
                    && model.AttachmentBase64Strings.Count > 0
                )
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
                                    Convert.FromBase64String(webApiSettings.LogoBankFooter)
                                )
                            ),
                        };
                        var header = new MimePart("image", "png")
                        {
                            ContentId = "logo_header",
                            Content = new MimeContent(
                                new MemoryStream(
                                    Convert.FromBase64String(webApiSettings.LogoBankHeader)
                                )
                            ),
                        };

                        var iconphone = new MimePart("image", "png")
                        {
                            ContentId = "iconphone",
                            Content = new MimeContent(
                                new MemoryStream(Convert.FromBase64String(o24NCHSetting.IconPhone))
                            ),
                        };

                        var iconwebsite = new MimePart("image", "png")
                        {
                            ContentId = "iconwebsite",
                            Content = new MimeContent(
                                new MemoryStream(
                                    Convert.FromBase64String(o24NCHSetting.IconWebsite)
                                )
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
    }
}
