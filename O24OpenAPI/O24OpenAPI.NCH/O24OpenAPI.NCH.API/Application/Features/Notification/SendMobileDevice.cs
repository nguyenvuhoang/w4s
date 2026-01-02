//using LinKit.Core.Cqrs;
//using O24OpenAPI.Framework.Exceptions;
//using O24OpenAPI.Framework.Extensions;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.NCH.API.Application.Constants;
//using System.Text.RegularExpressions;

//namespace O24OpenAPI.NCH.API.Application.Features.Notification;

//public class SendMobileDeviceCommand : BaseTransactionModel, ICommand<bool>
//{
//    public string UserCode { get; set; }

//    /// <summary>
//    /// Gets or sets the PushId
//    /// </summary>
//    public string PushId { get; set; }

//    /// <summary>
//    /// Gets or sets the Message
//    /// </summary>
//    public string Message { get; set; }

//    /// <summary>
//    /// Gets or sets the Title
//    /// </summary>
//    public string Title { get; set; }

//    /// <summary>
//    /// Image Url
//    /// </summary>
//    public string ImageUrl { get; set; } = string.Empty;

//    /// <summary>
//    /// Template ID
//    /// </summary>
//    public string TemplateId { get; set; } = string.Empty;
//}

//public partial class SendMobileDeviceHandler : ICommandHandler<SendMobileDeviceCommand, bool>
//{
//    public async Task<bool> HandleAsync(
//        SendMobileDeviceCommand request,
//        CancellationToken cancellationToken = default
//    )
//    {
//        ArgumentNullException.ThrowIfNull(request);

//        var pushId = request.PushId?.Trim();

//        if (string.IsNullOrEmpty(pushId))
//        {
//            throw await O24Exception.CreateAsync(
//                O24NCHResourceCode.Validation.FCMTokenIsNotExist,
//                request.Language,
//                [pushId]
//            );
//        }

//        var title = string.IsNullOrWhiteSpace(request.Title) ? "Notification" : request.Title.Trim();

//        var body = string.IsNullOrWhiteSpace(request.Message)
//            ? "TEST"
//            : NormalizeNotificationMessage(request.Message);
//        body = Utils.StringExtensions.AutoFormatBullets(body);
//        var imageUrl = request.ImageUrl?.Trim() ?? string.Empty;
//        try
//        {
//            await SendNotificationAsync(pushId, title, body);

//            var senderLogId = await LogInformation(
//                request.UserCode,
//                request.AppCode ?? "MB",
//                Code.NotificationTypeCode.FIREBASE,
//                templateID: request.TemplateId ?? "UNKNOWN",
//                redirect: Code.Common.YES,
//                dataSending: string.Empty,
//                notificationCategory: Code.Common.GENERAL,
//                message: body,
//                title: title,
//                imageUrl: imageUrl
//            );

//            if (senderLogId <= 0)
//            {
//                Console.WriteLine("⚠️ Sender log failed: " + request.UserCode);
//                return false;
//            }
//            return true;
//        }
//        catch (O24Exception)
//        {
//            throw;
//        }
//        catch (Exception ex)
//        {
//            await ex.LogErrorAsync(
//                $"Failed to send mobile notification. PushId={pushId}, Title={title}"
//            );
//            throw await O24Exception.CreateAsync(
//                O24NCHResourceCode.Error.SendEmailFailed,
//                request.Language,
//                [pushId, ex.Message]
//            );
//        }
//    }

//    private static string NormalizeNotificationMessage(string raw)
//    {
//        if (string.IsNullOrWhiteSpace(raw))
//            return string.Empty;

//        var text = raw.Trim();

//        text = Regex.Replace(text, @"\s{2,}|(\n){3,}", m =>
//        {
//            if (m.Value.Contains("\n"))
//                return "\n\n";
//            return " ";
//        });

//        text = text.Replace("\r\n", "\n").Replace("\r", "\n");

//        if (!string.IsNullOrEmpty(text))
//            text = char.ToUpper(text[0]) + text.Substring(1);

//        return text;
//    }

//}
