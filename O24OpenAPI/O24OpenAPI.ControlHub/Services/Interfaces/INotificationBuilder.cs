using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models.Roles;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface INotificationBuilder
{
    ResetPasswordResponseModel BuildResetPasswordNotification(
    string usercode,
    string phoneNumber,
    UserAccount userAccount,
    string newPassword);
}
