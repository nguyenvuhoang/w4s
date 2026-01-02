using LinKit.Core.Cqrs;
using LinKit.Json.Runtime;
using LinqToDB;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Common;
using O24OpenAPI.NCH.API.Application.Models.Request;
using O24OpenAPI.NCH.API.Application.Models.Request.Mail;
using O24OpenAPI.NCH.API.Application.Models.Response;
using O24OpenAPI.NCH.API.Application.Utils;
using O24OpenAPI.NCH.Config;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;
using O24OpenAPI.NCH.Domain.Constants;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace O24OpenAPI.NCH.API.Application.Features.Notification;

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
    public List<O24MimeEntity> MimeEntities { get; set; } = [];
    public List<int> FileIds { get; set; } = [];
    public string Message { get; set; } = string.Empty;
}

public class SMSRequestModel : SendNotificationCommand { }

public class TelegramSendModel : SendNotificationCommand
{
    [JsonProperty("chat_id")]
    public string ChatId { get; set; }

    [JsonProperty("message")]
    public new string Message { get; set; }
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
    ISMSMappingResponseRepository sMSMappingRespository
) : ICommandHandler<SendNotificationCommand, bool>
{
    private readonly HttpClient _httpClient = new();

    [WorkflowStep(WorkflowStep.NCH.WF_STEP_NCH_SEND_NOTIFICATION)]
    public async Task<bool> HandleAsync(
        SendNotificationCommand request,
        CancellationToken cancellationToken = default
    )
    {
        WorkContext workContext = EngineContext.Current.Resolve<WorkContext>();
        request.RefId = workContext.ExecutionId ?? Guid.NewGuid().ToString();
        try
        {
            string type = (request?.NotificationType ?? "").Trim().ToUpperInvariant();

            switch (type)
            {
                case "MAIL":
                    return await SendEmailAsync(MapToSendMailModel(request));

                case "SMS":
                    SMSRequestModel sms = request as SMSRequestModel ?? MapToSms(request);
                    return await SendSMS(sms);

                case "TELE":
                    TelegramSendModel tele = request as TelegramSendModel ?? MapToTelegram(request);
                    return await SendMessage(tele);

                default:
                    throw new NotSupportedException(
                        $"Notification type '{request?.NotificationType}' is not supported."
                    );
            }
        }
        catch (Exception ex)
        {
            _ = ex.LogErrorAsync(
                new Dictionary<string, object?>
                {
                    ["RuntimeType"] = request?.GetType().FullName,
                    ["NotificationType"] = request?.NotificationType,
                }
            );
            return false;
        }
    }

    private async Task<bool> SendMessage(TelegramSendModel request)
    {
        if (string.IsNullOrEmpty(request.ChatId) || string.IsNullOrEmpty(request.Message))
        {
            return false;
        }

        string _botToken = "8197855911:AAHId-nVPso_dkkBTsE5WHO-Ae0dp9HzMpE";
        string requestUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage";

        var payload = new
        {
            chat_id = request.ChatId,
            text = request.Message,
            parse_mode = "Markdown",
        };
        StringContent content = new(payload.ToJson(), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, content);

        return response.IsSuccessStatusCode;
    }

    private static TelegramSendModel MapToTelegram(SendNotificationCommand m)
    {
        return new TelegramSendModel
        {
            NotificationType = "TELE", /*...*/
        };
    }

    private async Task<bool> SendSMS(SMSRequestModel request)
    {
        try
        {
            string message = string.IsNullOrWhiteSpace(request.Message)
                ? await sMSTemplateRepository.BuildSMSContentDynamicAsync(
                    request.Purpose,
                    request.SenderData
                )
                : request.Message;

            string transactionId = request.RefId;
            SendSOAPResponseModel sendResult = await SendSMSAsync(
                request.PhoneNumber,
                message,
                transactionId
            );
            return sendResult.IsSuccess;
        }
        catch (Exception ex)
        {
            _ = ex.LogErrorAsync();
            return false;
        }
    }

    private async Task<SendSOAPResponseModel> SendSMSAsync(
        string phoneNumber,
        string message,
        string transactionId,
        string providerName = "UNITEL",
        string endtoend = "",
        string messagetype = "SMS"
    )
    {
        string soapXml = null;

        SMSProvider providerMain = await sMSProviderRepository.GetProviderByPhoneNumber(
            phoneNumber
        );

        if (providerMain != null)
        {
            providerName = providerMain.ProviderName;
        }

        SMSProvider provider = await sMSProviderRepository.Table.FirstOrDefaultAsync(p =>
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
            string transactionIdFinal = endtoend ?? transactionId ?? Guid.NewGuid().ToString();
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

        string wsdlUrl = await sMSProviderConfigRepository
            .Table.Where(x =>
                x.SMSProviderId == provider.ProviderName && x.ConfigKey == "WSDL_URL" && x.IsActive
            )
            .Select(x => x.ConfigValue)
            .FirstOrDefaultAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(wsdlUrl))
        {
            throw new Exception("WSDL_URL not found.");
        }

        SMSSendOut sendOut = sMSSendOutRepository.CreateSendOut(
            provider.ProviderName,
            phoneNumber,
            messageContent,
            soapXml,
            transactionId,
            endtoend,
            0,
            isResend
        );

        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            string contentType =
                (
                    await sMSProviderConfigRepository.GetConfigValueAsync(
                        provider.ProviderName,
                        "SOAP_CONTENT_TYPE"
                    )
                )?.Trim() ?? "application/soap+xml; charset=utf-8";
            using StringContent content = new(soapXml, Encoding.UTF8);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
            string requestHeader = await sMSProviderConfigRepository.AddDynamicSoapHeaders(
                content,
                provider.ProviderName
            );
            sendOut.RequestHeader = requestHeader;
            sendOut.FullRequest = BuildFullHttpRequest(wsdlUrl, "POST", content);

            HttpResponseMessage response = await httpClient.PostAsync(
                wsdlUrl,
                content,
                cancellationToken
            );
            string responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            stopwatch.Stop();

            List<SMSMappingResponse> allMappings = await sMSMappingRespository.Table.ToListAsync(
                cancellationToken
            );
            Dictionary<string, HashSet<string>> successMap = allMappings
                .Where(x => x.IsSuccess)
                .GroupBy(x => x.ProviderName.ToUpper())
                .ToDictionary(g => g.Key, g => g.Select(x => x.ResponseCode).ToHashSet());

            (
                string msgId,
                bool isSuccess,
                int resultCode,
                string resultDesc,
                string providerKey,
                string rawCode
            ) = O24SMSResponseParser.ExtractMsgIdAndStatusFromSOAP(responseString, successMap);

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
        StringBuilder requestLog = new();

        requestLog.AppendLine($"{method} {url} HTTP/1.1");

        foreach (KeyValuePair<string, IEnumerable<string>> header in content.Headers)
        {
            foreach (string value in header.Value)
            {
                requestLog.AppendLine($"{header.Key}: {value}");
            }
        }

        requestLog.AppendLine();

        string body = content.ReadAsStringAsync().Result;
        requestLog.AppendLine(body);

        return requestLog.ToString();
    }

    private static SMSRequestModel MapToSms(SendNotificationCommand m)
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

    private static SendMailRequestModel MapToSendMailModel(SendNotificationCommand request)
    {
        return new SendMailRequestModel
        {
            TemplateId = $"{request.ChannelId}_MAIL_RESET_PASSWORD",
            ConfigId = "main_mail",
            Receiver = request.Email,
            DataTemplate = request.DataTemplate ?? [],
            AttachmentBase64Strings = request.AttachmentBase64Strings ?? [],
            AttachmentFilenames = request.AttachmentFilenames ?? [],
            MimeEntities = request.MimeEntities ?? [],
            IncludeLogo = true,
            FileIds = request.FileIds ?? [],
        };
    }

    private static readonly string[] separator = [";"];

    public async Task<bool> SendEmailAsync(SendMailRequestModel request)
    {
        EmailSendOut emailLog = new()
        {
            ConfigId = request.ConfigId,
            TemplateId = request.TemplateId,
            Receiver = !string.IsNullOrWhiteSpace(request.Receiver)
                ? request.Receiver
                : (
                    request.DataTemplate != null
                    && request.DataTemplate.TryGetValue("email", out object emailVal)
                        ? emailVal?.ToString()
                        : null
                ),
            Status = MailSendOutStatus.PENDING,
            CreatedAt = DateTime.UtcNow,
        };
        try
        {
            MailConfig getMailConfig = await mailConfigRepository.GetByConfigId(request.ConfigId);
            MailTemplate getMailTemplate = await mailTemplateRepository.GetByTemplateId(
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
                emailLog.Attachments = request.AttachmentFilenames.ToJson();
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
}
