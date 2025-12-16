namespace O24OpenAPI.O24NCH.Services.Services;

using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.O24NCH.Common;
using O24OpenAPI.O24NCH.Constant;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Models.Response;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.O24NCH.Utils;
using static QRCoder.PayloadGenerator;

/// <summary>
/// Defines the <see cref="SMSProviderService" />
/// </summary>
public class SMSProviderService(
    IRepository<SMSProviderConfig> smsProviderConfigRepository,
    IRepository<SMSProvider> smsProviderRepository,
    IRepository<SMSSendOut> smsSendOutRepository,
    IRepository<SMSProviderStatus> smsProviderStatusRepository,
    IRepository<SMSMappingResponse> smsMappingResponseRepository
) : ISMSProviderService
{
    /// <summary>
    /// Defines the _smsProviderConfigRepository
    /// </summary>
    private readonly IRepository<SMSProviderConfig> _smsProviderConfigRepository =
        smsProviderConfigRepository;

    /// <summary>
    /// Defines the _smsProviderRepository
    /// </summary>
    private readonly IRepository<SMSProvider> _smsProviderRepository = smsProviderRepository;

    /// <summary>
    /// Defines the _smsSendOutRepository
    /// </summary>
    private readonly IRepository<SMSSendOut> _smsSendOutRepository = smsSendOutRepository;

    /// <summary>
    /// Defines the _smsProviderStatusRepository
    /// </summary>
    private readonly IRepository<SMSProviderStatus> _smsProviderStatusRepository =
        smsProviderStatusRepository;

    /// <summary>
    /// Defines the _smsMappingResponseRepository
    /// </summary>
    private readonly IRepository<SMSMappingResponse> _smsMappingResponseRepository =
        smsMappingResponseRepository;

    /// <summary>
    /// Defines the _sharedHandler
    /// </summary>
    private static readonly SocketsHttpHandler _sharedHandler = new()
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(5),
        PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
        MaxConnectionsPerServer = 100,
    };

    /// <summary>
    /// Defines the _sharedHttpClient
    /// </summary>
    private static readonly HttpClient _sharedHttpClient = new(_sharedHandler)
    {
        Timeout = TimeSpan.FromSeconds(30),
    };

    /// <summary>
    /// The GetProviderByPhoneNumber
    /// </summary>
    /// <param name="phoneNumber">The phoneNumber<see cref="string"/></param>
    /// <returns>The <see cref="Task{SMSProvider}"/></returns>
    public async Task<SMSProvider> GetProviderByPhoneNumber(string phoneNumber)
    {
        var providers = await _smsProviderRepository.Table.Where(p => p.IsActive).ToListAsync();
        foreach (var provider in providers)
        {
            var allowedPrefixes =
                provider
                    .AllowedPrefix?.Split(
                        ',',
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                    )
                    .ToList() ?? [];
            foreach (var prefix in allowedPrefixes)
            {
                if (phoneNumber.StartsWith(prefix))
                {
                    return provider;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Gửi SMS đến khách hàng qua provider đã cấu hình
    /// </summary>
    /// <param name="phoneNumber">The phoneNumber<see cref="string"/></param>
    /// <param name="message">The message<see cref="string"/></param>
    /// <param name="transactionId">The transactionId<see cref="string"/></param>
    /// <param name="providerName">The providerName<see cref="string"/></param>
    /// <param name="endtoend">The endtoend<see cref="string"/></param>
    /// <param name="messagetype">The messagetype<see cref="string"/></param>
    /// <returns>The <see cref="Task{SendSOAPResponseModel}"/></returns>
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

        var providerMain = await GetProviderByPhoneNumber(phoneNumber);

        if (providerMain != null)
        {
            providerName = providerMain.ProviderName;
        }

        var provider = await _smsProviderRepository.Table.FirstOrDefaultAsync(p =>
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
            soapXml = await BuildSOAPRequestAsync(
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
            await _smsSendOutRepository.InsertAsync(
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

    /// <summary>
    /// Build SOAP request using template, auth info and dynamic values
    /// </summary>
    /// <param name="provider">The provider<see cref="SMSProvider"/></param>
    /// <param name="values">The values<see cref="Dictionary{string, string}"/></param>
    /// <returns>The <see cref="Task{string}"/></returns>
    public async Task<string> BuildSOAPRequestAsync(
        SMSProvider provider,
        Dictionary<string, string> values
    )
    {
        var configs = await _smsProviderConfigRepository
            .Table.Where(x => x.SMSProviderId == provider.ProviderName && x.IsActive)
            .ToListAsync();

        var configDict = configs.ToDictionary(x => x.ConfigKey, x => x.ConfigValue);

        if (!configDict.TryGetValue("SOAP_TEMPLATE", out var template))
        {
            throw new Exception("SOAP_TEMPLATE not found.");
        }

        switch (provider.ProviderName?.ToUpperInvariant())
        {
            case "UNITEL":
                template = template.Replace("{USERNAME}", provider.ApiUsername ?? string.Empty);
                template = template.Replace("{PASSWORD}", provider.ApiPassword ?? string.Empty);
                template = template.Replace("{BRAND}", provider.BrandName ?? string.Empty);
                break;

            case "LTC":
                values.TryGetValue("RECEIVERPHONE", out var receiverPhone);
                values.TryGetValue("TRANSACTIONID", out var transactionId);
                var dynamicKey = $"{provider.ApiUsername}{transactionId}{receiverPhone}";
                string genkey = LTC.Encrypt(dynamicKey, provider.ApiKey);
                template = template.Replace("{LTCKEY}", genkey ?? string.Empty);
                template = template.Replace("{LTCUSER}", provider.ApiUsername ?? string.Empty);
                break;
            case "ETL":
                template = template.Replace("{USERNAME}", provider.ApiUsername ?? string.Empty);

                values.TryGetValue("RECEIVERPHONE", out var eltReceiverPhone);
                values.TryGetValue("TRANSACTIONID", out var etlTransactionId);
                var sign = Etl.GenerateSign(
                    spId: provider.ApiUsername,
                    transactionId: etlTransactionId,
                    msisdn: eltReceiverPhone,
                    key: provider.ApiKey,
                    url: "https://manage.etllao.com"
                );
                template = template.Replace("{ETLSIGN}", sign?.ToUpperInvariant() ?? string.Empty);
                break;
            default:
                // fallback: replace common keys if present
                template = template.Replace("{USERNAME}", provider.ApiUsername ?? string.Empty);
                template = template.Replace("{PASSWORD}", provider.ApiPassword ?? string.Empty);
                template = template.Replace("{BRAND}", provider.BrandName ?? string.Empty);
                break;
        }

        foreach (var pair in values)
        {
            template = template.Replace($"{{{pair.Key}}}", pair.Value ?? string.Empty);
        }

        return template;
    }

    /// <summary>
    /// Gửi SOAP request bằng HttpClient (không lưu log)
    /// </summary>
    /// <param name="providerId">The providerId<see cref="string"/></param>
    /// <param name="soapXml">The soapXml<see cref="string"/></param>
    /// <returns>The <see cref="Task{string}"/></returns>
    public async Task<string> SendSOAPRequestAsync(string providerId, string soapXml)
    {
        var wsdlUrl = await _smsProviderConfigRepository
            .Table.Where(x =>
                x.SMSProviderId == providerId && x.ConfigKey == "WSDL_URL" && x.IsActive
            )
            .Select(x => x.ConfigValue)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(wsdlUrl))
        {
            throw new Exception("WSDL_URL not found.");
        }

        using var client = new HttpClient();
        using var content = new StringContent(soapXml, Encoding.UTF8, "text/xml");

        var response = await client.PostAsync(wsdlUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? $"Success: {response.StatusCode} - {responseString}"
            : $"Failed: {response.StatusCode} - {responseString}";
    }

    /// <summary>
    /// The SyncSMSProviderStatusAsync
    /// </summary>
    /// <returns>The <see cref="Task"/></returns>
    public async Task SyncSMSProviderStatusAsync()
    {
        var activeProviders = await _smsProviderRepository
            .Table.Where(p => p.IsActive)
            .ToListAsync();

        foreach (var provider in activeProviders)
        {
            var stopwatch = Stopwatch.StartNew();
            string responseString = null;
            string errorMessage = null;
            bool isOnline = false;

            try
            {
                var dummyConfig = await _smsProviderConfigRepository
                    .Table.Where(x =>
                        x.SMSProviderId == provider.ProviderName
                        && x.IsActive
                        && x.ConfigKey == "DUMMY_DATA"
                    )
                    .Select(x => x.ConfigValue)
                    .FirstOrDefaultAsync();

                var dummyData = string.IsNullOrWhiteSpace(dummyConfig)
                    ? []
                    : JsonSerializer.Deserialize<Dictionary<string, string>>(dummyConfig) ?? [];
                dummyData["TRANSACTIONID"] = Guid.NewGuid().ToString();

                // Build SOAP for health check
                string soapXml = await BuildSOAPRequestAsync(provider, dummyData);

                using var client = new HttpClient();
                using var content = new StringContent(soapXml, Encoding.UTF8, "text/xml");

                var response = await client.PostAsync(provider.ApiUrl, content);
                responseString = await response.Content.ReadAsStringAsync();

                stopwatch.Stop();
                var allMappings = await _smsMappingResponseRepository.Table.ToListAsync();

                var successMap = allMappings
                    .Where(x => x.IsSuccess)
                    .GroupBy(x => x.ProviderName.ToUpper())
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ResponseCode).ToHashSet());

                // ✅ Dùng parser phân tích response từ cả UNITEL và LTC
                var (msgId, isSuccess, resultCode, resultDesc, providerKey, rawCode) =
                    O24SMSResponseParser.ExtractMsgIdAndStatusFromSOAP(responseString, successMap);

                isOnline = isSuccess;

                var status = new SMSProviderStatus
                {
                    ProviderId = provider.ProviderName,
                    CheckTime = DateTime.UtcNow,
                    IsOnline = isOnline,
                    ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds,
                    ResponseMessage = responseString,
                    ErrorDetail = null,
                };

                await _smsProviderStatusRepository.InsertAsync(status);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                errorMessage = ex.Message;

                var status = new SMSProviderStatus
                {
                    ProviderId = provider.ProviderName,
                    CheckTime = DateTime.UtcNow,
                    IsOnline = false,
                    ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds,
                    ResponseMessage = responseString ?? "",
                    ErrorDetail = errorMessage,
                };

                await _smsProviderStatusRepository.InsertAsync(status);
                await ex.LogErrorAsync();
            }
        }
    }

    // --- Bulk send method ---
    /// <summary>
    /// Bulk send SMS concurrently with degree of parallelism limit.
    /// messages: list of (ProviderName, PhoneNumber, Message, TransactionId, EndToEnd)
    /// maxDegreeOfParallelism: số task chạy song song cùng lúc
    /// </summary>
    /// <param name="messages">The messages<see cref="List{(string ProviderName, string PhoneNumber, string Message, string TransactionId, string EndToEnd)}"/></param>
    /// <param name="maxDegreeOfParallelism">The maxDegreeOfParallelism<see cref="int"/></param>
    /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
    /// <returns>The <see cref="Task{List{SendSOAPResponseModel}}"/></returns>
    public async Task<List<SendSOAPResponseModel>> SendBulkSMSAsync(
        List<(
            string ProviderName,
            string PhoneNumber,
            string Message,
            string TransactionId,
            string EndToEnd
        )> messages,
        int maxDegreeOfParallelism = 20,
        CancellationToken cancellationToken = default
    )
    {
        if (messages == null || messages.Count == 0)
        {
            return [];
        }

        var results = new List<SendSOAPResponseModel>(capacity: messages.Count);
        var semaphore = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);
        var tasks = new List<Task>();

        foreach (var msg in messages)
        {
            await semaphore.WaitAsync(cancellationToken);

            var t = Task.Run(
                async () =>
                {
                    try
                    {
                        var provider = await _smsProviderRepository.Table.FirstOrDefaultAsync(
                            p => p.ProviderName == msg.ProviderName && p.IsActive,
                            cancellationToken
                        );

                        if (provider == null)
                        {
                            lock (results)
                            {
                                results.Add(
                                    new SendSOAPResponseModel
                                    {
                                        TransactionId = msg.TransactionId,
                                        MessageId = null,
                                        ErrorCode = 8888,
                                        ErrorMessage = "Provider is not exist",
                                        ProviderKey = null,
                                        RawResponseCode = null,
                                        IsSuccess = false,
                                    }
                                );
                            }
                            return;
                        }

                        var phoneNumber = msg.PhoneNumber;

                        if (!string.IsNullOrWhiteSpace(phoneNumber))
                        {
                            if (phoneNumber.StartsWith('0'))
                            {
                                if (msg.ProviderName == Code.ProviderName.ETL)
                                {
                                    phoneNumber = "856" + phoneNumber[1..];
                                }
                                else if (msg.ProviderName == Code.ProviderName.LTC) { }
                                else
                                {
                                    phoneNumber = phoneNumber[1..];
                                }
                            }
                        }

                        var values = new Dictionary<string, string>
                        {
                            { "RECEIVERPHONE", phoneNumber },
                            { "MSGCONTENT", msg.Message },
                            { "TRANSACTIONID", msg.EndToEnd ?? string.Empty },
                        };

                        string soapXml = await BuildSOAPRequestAsync(provider, values);

                        var resp = await SendSOAPRequestAndLogAsync(
                            provider,
                            phoneNumber,
                            msg.Message,
                            soapXml,
                            msg.TransactionId,
                            httpClient: _sharedHttpClient,
                            cancellationToken: cancellationToken
                        );

                        lock (results)
                        {
                            results.Add(resp);
                        }
                    }
                    catch (Exception ex)
                    {
                        await ex.LogErrorAsync();

                        var fallback = new SendSOAPResponseModel
                        {
                            TransactionId = msg.TransactionId,
                            MessageId = null,
                            ErrorCode = 9999,
                            ErrorMessage = ex.Message,
                            ProviderKey = null,
                            RawResponseCode = null,
                            IsSuccess = false,
                        };

                        lock (results)
                        {
                            results.Add(fallback);
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                },
                cancellationToken
            );

            tasks.Add(t);
        }

        await Task.WhenAll(tasks);
        return results;
    }

    /// <summary>
    /// Send SOAP request and log the request/response into SMSSendOut table
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="messageContent"></param>
    /// <param name="soapXml"></param>
    /// <param name="transactionId"></param>
    /// <param name="endtoend">The endtoend<see cref="string"/></param>
    /// <param name="retryCount"></param>
    /// <param name="isResend"></param>
    /// <param name="httpClient"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

        var wsdlUrl = await _smsProviderConfigRepository
            .Table.Where(x =>
                x.SMSProviderId == provider.ProviderName && x.ConfigKey == "WSDL_URL" && x.IsActive
            )
            .Select(x => x.ConfigValue)
            .FirstOrDefaultAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(wsdlUrl))
        {
            throw new Exception("WSDL_URL not found.");
        }

        var sendOut = CreateSendOut(
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
                (await GetConfigValueAsync(provider.ProviderName, "SOAP_CONTENT_TYPE"))?.Trim()
                ?? "application/soap+xml; charset=utf-8";
            using var content = new StringContent(soapXml, Encoding.UTF8);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
            var requestHeader = await AddDynamicSoapHeaders(content, provider.ProviderName);
            sendOut.RequestHeader = requestHeader;
            sendOut.FullRequest = BuildFullHttpRequest(wsdlUrl, "POST", content);

            var response = await httpClient.PostAsync(wsdlUrl, content, cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            stopwatch.Stop();

            var allMappings = await _smsMappingResponseRepository.Table.ToListAsync(
                cancellationToken
            );
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

            await _smsSendOutRepository.InsertAsync(sendOut);

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

            await _smsSendOutRepository.InsertAsync(sendOut);
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

    /// <summary>
    /// The Search
    /// </summary>
    /// <param name="model">The model<see cref="Web.Framework.Models.SimpleSearchModel"/></param>
    /// <returns>The <see cref="Task{IPagedList{SMSProvider}}"/></returns>
    public async Task<IPagedList<SMSProvider>> Search(Framework.Models.SimpleSearchModel model)
    {
        return await _smsProviderRepository.GetAllPaged(
            query =>
            {
                if (!string.IsNullOrEmpty(model.SearchText))
                {
                    query = query.Where(c => c.ProviderName.Contains(model.SearchText));
                }

                query = query.OrderBy(c => c.Id);
                return query;
            },
            0,
            0
        );
    }

    /// <summary>
    /// The GetById
    /// </summary>
    /// <param name="id">The id<see cref="int"/></param>
    /// <returns>The <see cref="Task{SMSProvider}"/></returns>
    public virtual async Task<SMSProvider> GetById(int id)
    {
        return await _smsProviderRepository.GetById(id);
    }

    /// <summary>
    /// Update SMS Provider
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<UpdateSMSProviderResponseModel> Update(SMSProviderUpdateModel model)
    {
        var entity =
            await GetById(model.Id)
            ?? throw await O24Exception.CreateAsync(ResourceCode.Common.NotExists, model.Language);

        var originalEntity = entity.Clone();

        model.ToEntityNullable(entity);

        await _smsProviderRepository.Update(entity);

        if (model.SMSProviderConfig?.Count > 0)
        {
            var providerId = entity.ProviderName;

            var existingConfigs = await _smsProviderConfigRepository
                .Table.Where(x => x.SMSProviderId == providerId)
                .ToListAsync();

            var toDeleteKeys = model
                .SMSProviderConfig.Where(x =>
                    (x.Action?.Equals("delete", StringComparison.OrdinalIgnoreCase) ?? false)
                )
                .Select(x => x.ConfigKey)
                .Where(k => !string.IsNullOrWhiteSpace(k))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (toDeleteKeys.Count > 0)
            {
                var toDeleteEntities = existingConfigs
                    .Where(x => toDeleteKeys.Contains(x.ConfigKey))
                    .ToList();

                foreach (var del in toDeleteEntities)
                {
                    await _smsProviderConfigRepository.Delete(del);
                }
            }

            var toUpsertRaw = model.SMSProviderConfig.Where(x =>
                !(x.Action?.Equals("delete", StringComparison.OrdinalIgnoreCase) ?? false)
            );

            var toUpsert = toUpsertRaw
                .GroupBy(x => x.ConfigKey, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.Last())
                .Select(x => new SMSProviderConfig
                {
                    SMSProviderId = providerId,
                    ConfigKey = x.ConfigKey,
                    ConfigValue = x.ConfigValue,
                    Description = x.Description,
                    IsActive = x.IsActive,
                })
                .ToList();

            foreach (var config in toUpsert)
            {
                var existing = existingConfigs.FirstOrDefault(x =>
                    x.ConfigKey.Equals(config.ConfigKey, StringComparison.OrdinalIgnoreCase)
                );

                if (existing != null)
                {
                    if (
                        !string.Equals(
                            existing.ConfigValue?.Trim(),
                            config.ConfigValue?.Trim(),
                            StringComparison.Ordinal
                        )
                        || !string.Equals(
                            existing.Description?.Trim(),
                            config.Description?.Trim(),
                            StringComparison.Ordinal
                        )
                        || existing.IsActive != config.IsActive
                    )
                    {
                        existing.ConfigValue = config.ConfigValue;
                        existing.Description = config.Description;
                        existing.IsActive = config.IsActive;

                        await _smsProviderConfigRepository.Update(existing);
                    }
                }
                else
                {
                    await _smsProviderConfigRepository.Insert(config);
                }
            }
        }

        return UpdateSMSProviderResponseModel.FromUpdatedEntity(entity, originalEntity);
    }

    /// <summary>
    /// Create SMS Provider
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> CreateAsync(SMSProviderCreateModel model)
    {
        var isExisting = await _smsProviderRepository.Table.AnyAsync(p =>
            p.ProviderName == model.ProviderName
        );
        if (isExisting)
        {
            throw await O24Exception.CreateAsync(
                O24NCHResourceCode.Validation.SMSProviderIsExisting,
                model.Language,
                [model.ProviderName]
            );
            ;
        }

        // Tạo entity từ model
        var entity = new SMSProvider
        {
            ProviderName = model.ProviderName,
            ApiUrl = "",
            CountryPrefix = model.CountryPrefix,
            AllowedPrefix = model.AllowedPrefix,
            ApiUsername = model.ApiUsername,
            ApiPassword = model.ApiPassword,
            ApiKey = model.ApiKey,
            BrandName = model.BrandName,
            IsActive = true,
        };

        await _smsProviderRepository.InsertAsync(entity);

        if (model.SMSProviderConfig?.Count > 0)
        {
            var configEntities = model
                .SMSProviderConfig.Select(x => new SMSProviderConfig
                {
                    SMSProviderId = model.ProviderName,
                    ConfigKey = x.ConfigKey,
                    ConfigValue = x.ConfigValue,
                    Description = x.Description,
                    IsActive = x.IsActive,
                })
                .ToList();

            foreach (var config in configEntities)
            {
                await _smsProviderConfigRepository.InsertAsync(config);
            }
        }

        return true;
    }

    /// <summary>
    /// Create SMSSendOut object
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="messageContent"></param>
    /// <param name="soapLogXml"></param>
    /// <param name="transactionId"></param>
    /// <param name="endtoend"></param>
    /// <param name="retryCount"></param>
    /// <param name="isResend"></param>
    /// <returns></returns>
    public static SMSSendOut CreateSendOut(
        string providerName,
        string phoneNumber,
        string messageContent,
        string soapLogXml,
        string transactionId,
        string endtoend,
        int retryCount,
        bool isResend
    )
    {
        return new SMSSendOut
        {
            SMSProviderId = providerName,
            PhoneNumber = phoneNumber,
            MessageContent = messageContent,
            SentAt = DateTime.UtcNow,
            Status = SMSSendOutStatus.PENDING,
            OtpRequestId = transactionId,
            RetryCount = retryCount,
            IsResend = isResend,
            RequestMessage = soapLogXml,
            EndToEnd = endtoend,
        };
    }

    /// <summary>
    /// Build full HTTP request log string
    /// </summary>
    /// <param name="url"></param>
    /// <param name="method"></param>
    /// <param name="content"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Add dynamic SOAP headers from SMSProviderConfig
    /// </summary>
    /// <param name="content"></param>
    /// <param name="providerName"></param>
    /// <returns></returns>
    private async Task<string> AddDynamicSoapHeaders(StringContent content, string providerName)
    {
        var headerConfigs = await _smsProviderConfigRepository
            .Table.Where(x =>
                x.SMSProviderId == providerName
                && x.ConfigKey.StartsWith("SOAP_HEADER_")
                && x.IsActive
            )
            .ToListAsync();

        var headerLog = new StringBuilder();
        foreach (var header in headerConfigs)
        {
            var headerKey = header.ConfigKey["SOAP_HEADER_".Length..];

            content.Headers.Add(headerKey, header.ConfigValue);
            headerLog.AppendLine($"{headerKey}: {header.ConfigValue}");
        }

        return headerLog.ToString().Trim();
    }

    /// <summary>
    /// Get config value by provider and key
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private async Task<string> GetConfigValueAsync(string providerName, string key)
    {
        return await _smsProviderConfigRepository
            .Table.Where(x => x.SMSProviderId == providerName && x.ConfigKey == key && x.IsActive)
            .Select(x => x.ConfigValue)
            .FirstOrDefaultAsync();
    }
}
