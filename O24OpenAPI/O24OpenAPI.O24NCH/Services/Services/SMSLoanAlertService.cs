using System.Globalization;
using LinqToDB;
using O24OpenAPI.APIContracts.Models.NCH;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Utils;
using O24OpenAPI.GrpcContracts.GrpcClientServices.DTS;
using O24OpenAPI.Logging.Helpers;
using O24OpenAPI.O24NCH.Constant;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.O24NCH.Utils;

namespace O24OpenAPI.O24NCH.Services.Services;

public class SMSLoanAlertService(
    IDTSGrpcClientService grpcClientService,
    ISMSService smsService,
    IRepository<RepaymentRemind> repaymentRemind,
    INotificationTemplateService notificationTemplateService,
    INotificationService notificationService,
    IUserNotificationsService userNotificationsService
) : ISMSLoanAlertService
{
    private readonly IDTSGrpcClientService _grpcClientService = grpcClientService;
    private readonly ISMSService _smsService = smsService;
    private readonly IRepository<RepaymentRemind> _repaymentRemind = repaymentRemind;
    private readonly INotificationTemplateService _notificationTemplateService =
        notificationTemplateService;
    private readonly INotificationService _notificationService = notificationService;
    private readonly IUserNotificationsService _userNotificationsService = userNotificationsService;

    /// <summary>
    /// Fetches all alerts async.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pageSize"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async Task<List<SMSLoanAlertModel>> FetchAllAlertsAsync(string type)
    {
        List<SMSLoanAlertModel> page;

        page = await _grpcClientService.GetSMSLoanAlertAsync(type);
        BusinessLogHelper.Info($"Fetched {page.Count} alerts for type={type}");

        if (page == null || page.Count == 0)
            return [];

        var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
        var accountno = page.Select(x => x.AccountNumber).ToList();
        var dueDates = page.Select(x => x.DueDate.Date).ToList();

        var history = await _repaymentRemind
            .Table.Where(x =>
                accountno.Contains(x.AccountNumber)
                && dueDates.Contains(x.DueDate)
                && x.MessageType == type
            )
            .ToListAsync();

        var result = new List<SMSLoanAlertModel>();

        foreach (var item in page)
        {
            var record = history.FirstOrDefault(x =>
                x.AccountNumber == item.AccountNumber
                && x.DueDate == item.DueDate
                && x.MessageType == item.NotificationType
            );

            // Never sent → send
            if (record == null)
            {
                result.Add(item);
                continue;
            }

            // Sent > 1M → Resend
            if (record.LastSentOn <= oneMonthAgo)
            {
                result.Add(item);
            }
        }

        return result;
    }

    /// <summary>
    /// Send SMS loan alerts async.
    /// </summary>
    /// <param name="smsAlerts"></param>
    /// <param name="maxParallel"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async Task<(int SentOk, int SentFail)> SendSmsLoanAlertsAsync(
        IList<SMSLoanAlertModel> smsAlerts,
        int maxParallel,
        CancellationToken ct
    )
    {
        int totalOk = 0,
            totalFail = 0;

        foreach (var batch in smsAlerts.Chunk(maxParallel))
        {
            ct.ThrowIfCancellationRequested();

            var batchAlerts = batch.ToList();
            var batchRequests = batchAlerts.Select(ToSmsRequest).ToList();

            try
            {
                var ok = await _smsService.BulkSendSMS(batchRequests, maxParallel, ct);
                if (ok)
                {
                    totalOk += batchRequests.Count;

                    foreach (var alert in batchAlerts)
                    {
                        await UpdateRepaymentRemindAsync(alert, "SMS", true, ct);
                    }
                }
                else
                {
                    var (indOk, indFail) = await SendSmsIndividuallyWithFallbackAsync(
                        batchAlerts,
                        ct
                    );
                    totalOk += indOk;
                    totalFail += indFail;
                }
            }
            catch (Exception ex)
            {
                await ex.LogErrorAsync();

                var (indOk, indFail) = await SendSmsIndividuallyWithFallbackAsync(batchAlerts, ct);
                totalOk += indOk;
                totalFail += indFail;
            }
        }

        return (totalOk, totalFail);
    }

    /// <summary>
    /// Send SMS individually with fallback async.
    /// </summary>
    /// <param name="batchList"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async Task<(int Ok, int Fail)> SendSmsIndividuallyWithFallbackAsync(
        IList<SMSLoanAlertModel> alerts,
        CancellationToken ct
    )
    {
        int ok = 0,
            fail = 0;

        var tasks = alerts.Select(async alert =>
        {
            var req = ToSmsRequest(alert);

            try
            {
                ct.ThrowIfCancellationRequested();

                var singleOk = await _smsService.BulkSendSMS([req]);

                if (singleOk)
                {
                    Interlocked.Increment(ref ok);
                    await UpdateRepaymentRemindAsync(alert, "SMS", true, ct);
                }
                else
                {
                    Interlocked.Increment(ref fail);
                    await UpdateRepaymentRemindAsync(alert, "SMS", false, ct);
                }
            }
            catch (Exception ex)
            {
                await ex.LogErrorAsync();
                Interlocked.Increment(ref fail);
                await UpdateRepaymentRemindAsync(alert, "SMS", false, ct);
            }
        });

        await Task.WhenAll(tasks);

        return (ok, fail);
    }

    /// <summary>
    /// Prepares the firebase loan alerts.
    /// </summary>
    /// <param name="fbAlerts"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async Task<(int Ok, int Fail)> SendFirebaseLoanAlertsAsync(
        IList<SMSLoanAlertModel> fbAlerts,
        CancellationToken ct
    )
    {
        int ok = 0,
            fail = 0;
        var tasks = fbAlerts.Select(async a =>
        {
            try
            {
                ct.ThrowIfCancellationRequested();

                var dueDate = NormalizeUtcDateOnly(a.DueDate)
                    .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                var amount = Utility.FormatAmount(a.TotalPayment, a.CurrencyCode);

                var userNotifications = await _userNotificationsService.GetPushAsync(a.PhoneNumber);
                var template = await _notificationTemplateService.GetByTemplateIdAsync(
                    Code.NotificationTemplateCode.REMIND_LOAN.ToString()
                );
                if (template == null)
                {
                    template.Title = "EMI Remind Loan";
                    template.Body =
                        "EMI ຮຽນ: ທ່ານ {CustomerName}, ການຊຳລະຄືນເງິນກູ້ຂອງທ່ານ ຈຳນວນ {Currency} {Amount} ຈະຄົບກຳນົດໃນວັນທີ {DueDate}. ກະລຸນາຊຳລະກ່ອນວັນທີດັ່ງກ່າວ ເພື່ອຫຼີກລ້ຽງຄ່າປັບໄໝ. ຫາກທ່ານໄດ້ຊຳລະແລ້ວ,  ຂໍຂອບໃຈມາຍັງທ່ານຢ່າງສູງ.";
                }

                var tokenData = new
                {
                    CustomerName = a.AccountName,
                    Amount = amount,
                    Currency = a.CurrencyCode,
                    DueDate = dueDate,
                };

                var senderBody = Utility.ReplaceTokens(template.Body, tokenData);

                var model = new SendMobileDeviceRequestModel
                {
                    UserCode = userNotifications.UserCode,
                    PushId = userNotifications.PushId,
                    Title = template.Title,
                    Message = senderBody,
                };

                var issuccess = await _notificationService.SendMobileDeviceAsync(model);
                if (issuccess)
                {
                    await UpdateRepaymentRemindAsync(a, "SMS", true, ct);
                    Interlocked.Increment(ref ok);
                }
                else
                {
                    await UpdateRepaymentRemindAsync(a, "SMS", false, ct);
                    Interlocked.Increment(ref fail);
                }
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref fail);
                await ex.LogErrorAsync($"Firebase send failed for account={a.AccountNumber}");
            }
        });

        await Task.WhenAll(tasks);

        return (ok, fail);
    }

    /// <summary>
    /// Submits the SMS loan alert.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task SubmitSMSLoanAlert(CancellationToken ct = default)
    {
        var smsType = Code.NotificationTypeCode.SMS.ToString();
        var fbType = Code.NotificationTypeCode.FIREBASE.ToString();

        // Có thể chạy song song 2 query này
        var smsAlertsTask = FetchAllAlertsAsync(smsType);
        var fbAlertsTask = FetchAllAlertsAsync(fbType);

        await Task.WhenAll(smsAlertsTask, fbAlertsTask);

        var smsAlerts = smsAlertsTask.Result;
        var fbAlerts = fbAlertsTask.Result;

        const int maxParallel = 10;

        // 1. Send SMS
        var (sentOk, sentFail) = await SendSmsLoanAlertsAsync(smsAlerts, maxParallel, ct);

        // 2. Prepare (and/or send) Firebase
        var (firebaseSentOk, firebaseSentFail) = await SendFirebaseLoanAlertsAsync(fbAlerts, ct);

        // Log tổng kết
        ConsoleUtil.WriteInfo(
            $"Firebase push sent Ok: {firebaseSentOk}, Failed: {firebaseSentFail}"
        );
        ConsoleUtil.WriteInfo($"SMS sent OK: {sentOk}, failed: {sentFail}");
    }

    /// <summary>
    /// TO SMS request.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    private static SMSRequestModel ToSmsRequest(SMSLoanAlertModel model)
    {
        var amountStr = Utils.Utility.FormatAmount(model.TotalPayment, model.CurrencyCode);
        var dueDateStr = NormalizeUtcDateOnly(model.DueDate)
            .ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

        var data = new Dictionary<string, object>
        {
            ["CustomerName"] = model.AccountName ?? string.Empty,
            ["Currency"] = model.CurrencyCode ?? string.Empty,
            ["Amount"] = amountStr.Replace(model.CurrencyCode ?? "", "").Trim(),
            ["DueDate"] = dueDateStr,
        };

        return new SMSRequestModel
        {
            Purpose = "REMINDLOANREPAYMENT",
            PhoneNumber = model.PhoneNumber,
            SenderData = data,
        };
    }

    /// <summary>
    /// Generates the and send SMS async.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<bool> GenerateAndSendSMSAsync(SMSLoanAlertModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (string.IsNullOrWhiteSpace(model.PhoneNumber))
        {
            throw new ArgumentException("PhoneNumber is required.", nameof(model.PhoneNumber));
        }

        var amountStr = Utils.Utility.FormatAmount(model.TotalPayment, model.CurrencyCode);
        var dueDateStr = NormalizeUtcDateOnly(model.DueDate)
            .ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

        var data = new Dictionary<string, object>
        {
            ["CustomerName"] = model.AccountName ?? string.Empty,
            ["Currency"] = model.CurrencyCode ?? string.Empty, // template: {Currency}{Amount}
            ["Amount"] = amountStr.Replace(model.CurrencyCode ?? "", "").Trim(),
            ["DueDate"] = dueDateStr,
        };

        var smsRequest = new SMSRequestModel
        {
            Purpose = "REMINDLOANREPAYMENT",
            PhoneNumber = model.PhoneNumber,
            SenderData = data,
        };

        try
        {
            var ok = await _smsService.BulkSendSMS([smsRequest]);
            return ok;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            return false;
        }
    }

    private static DateTime NormalizeUtcDateOnly(DateTime dt)
    {
        var u =
            dt.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(dt, DateTimeKind.Utc)
                : dt.ToUniversalTime();
        return new DateTime(u.Year, u.Month, u.Day, 0, 0, 0, DateTimeKind.Utc);
    }

    /// <summary>
    /// Update repayment remind async.
    /// </summary>
    /// <param name="alert"></param>
    /// <param name="notificationType"></param>
    /// <param name="isSuccess"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async Task UpdateRepaymentRemindAsync(
        SMSLoanAlertModel alert,
        string notificationType,
        bool isSuccess,
        CancellationToken ct
    )
    {
        var dueDate = alert.DueDate.Date;

        var entity = await _repaymentRemind
            .Table.Where(x =>
                x.AccountNumber == alert.AccountNumber
                && x.DueDate == dueDate
                && x.MessageType == notificationType
            )
            .FirstOrDefaultAsync(ct);

        var now = DateTime.UtcNow;
        var status = isSuccess ? "SENT" : "FAILED";

        if (entity == null)
        {
            entity = new RepaymentRemind
            {
                AccountNumber = alert.AccountNumber,
                CustomerName = alert.AccountName,
                DueDate = dueDate,
                DueAmount = alert.TotalPayment,
                MessageType = notificationType,
                Status = status,
                LastSentOn = now,
                CreatedOn = now,
                UpdatedOn = now,
                ErrorMessage = isSuccess ? null : "Failed to send SMS",
            };

            await _repaymentRemind.Insert(entity);
            return;
        }

        entity.Status = status;
        entity.LastSentOn = now;
        entity.UpdatedOn = now;

        await _repaymentRemind.Update(entity);
    }
}
