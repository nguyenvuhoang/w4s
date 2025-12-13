using O24OpenAPI.APIContracts.Models.DTS;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models.Roles;
using O24OpenAPI.ControlHub.Services.Interfaces;
using StringExtensions = O24OpenAPI.ControlHub.Utils.StringExtensions;

namespace O24OpenAPI.ControlHub.Services;

public class NotificationBuilder : INotificationBuilder
{
    public ResetPasswordResponseModel BuildResetPasswordNotification(
        string usercode,
        string phoneNumber,
        UserAccount userAccount,
        string newPassword)
    {
        var smsData = new Dictionary<string, object> { { "0", newPassword } };

        var nameParts = new[] { userAccount.FirstName, userAccount.MiddleName, userAccount.LastName };
        var fullname = string.Join(" ", nameParts.Where(part => !string.IsNullOrWhiteSpace(part)));

        var qrImageBytes = StringExtensions.GenerateQRCodeBytes(newPassword);
        var mimeEntities = new List<DTSMimeEntityModel>
    {
        new() {
            ContentType = "image/png",
            ContentId = "qr",
            Base64 = Convert.ToBase64String(qrImageBytes)
        }
    };

        var emailDataTemplate = new Dictionary<string, object>
    {
        { "usercode", usercode },
        { "fullname", fullname },
        { "user_name", userAccount.UserName },
        { "phone", userAccount.Phone },
        { "email", userAccount.Email }
    };

        return new ResetPasswordResponseModel
        {
            UserCode = usercode,
            PhoneNumber = phoneNumber,
            TemplateCode = "RESETPASSWORD",
            NotificationType = userAccount.NotificationType,
            Email = userAccount.Email,
            SmsData = smsData,
            EmailDataTemplate = emailDataTemplate,
            MimeEntities = mimeEntities
        };
    }
}
