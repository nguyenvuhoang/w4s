using LinqToDB;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Services.Interfaces;

namespace O24OpenAPI.O24NCH.Services.Services;

public class UserNotificationsService(
    IRepository<UserNotifications> userNotificationsRepository,
    IRepository<NotificationDetail> notificationDetailRepository,
    IRepository<GroupUserNotification> groupUserNotificationRepository,
    ICTHGrpcClientService cthGrpcClientService
) : IUserNotificationsService
{
    private readonly IRepository<UserNotifications> _userNotificationsRepository =
        userNotificationsRepository;
    private readonly IRepository<NotificationDetail> _notificationDetailRepository =
        notificationDetailRepository;
    private readonly IRepository<GroupUserNotification> _groupUserNotificationRepository =
        groupUserNotificationRepository;
    private readonly ICTHGrpcClientService _cthGrpcClientService = cthGrpcClientService;

    public async Task<bool> CreateUserNotifications(string userCode, string category)
    {
        var unreadNotifications = await _notificationDetailRepository
            .Table.Where(n =>
                n.Status == true
                && n.NotificationCategory == category
                && (
                    n.TargetType == "All"
                    || (
                        n.TargetType == "Group"
                        && _groupUserNotificationRepository.Table.Any(g =>
                            g.GroupID == n.GroupID && g.UserCode == userCode
                        )
                    )
                    || (n.TargetType == "User" && n.UserCode == userCode)
                )
                && !_userNotificationsRepository.Table.Any(u =>
                    u.NotificationID == n.Id && u.UserCode == userCode
                )
            )
            .Select(n => n.Id)
            .ToListAsync();

        if (unreadNotifications.Count == 0)
        {
            return false;
        }

        foreach (var id in unreadNotifications)
        {
            var entity = new UserNotifications
            {
                NotificationID = id,
                UserCode = userCode,
                DateTime = DateTime.Now,
            };

            await _userNotificationsRepository.InsertAsync(entity);
        }
        return true;
    }

    /// <summary>
    /// Get Push ID by phone number
    /// </summary>
    /// <param name="phonenumber"></param>
    /// <returns></returns>
    public Task<UserNotifications> GetPushAsync(string phonenumber)
    {
        return _userNotificationsRepository
            .Table.Where(x => x.PhoneNumber == phonenumber)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Scan user notifications and send push notifications if there are any new notifications.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> ScanUserNotification(CancellationToken cancellationToken)
    {
        try
        {
            var notifications = await _cthGrpcClientService.GetUserNotificationAsync();
            foreach (var notification in notifications)
            {
                var existing = await _userNotificationsRepository
                    .Table.Where(x => x.UserCode == notification.UserCode)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existing != null)
                {
                    existing.PushId = notification.PushId;
                    existing.DateTime = DateTime.Now;
                    existing.PhoneNumber = notification.PhoneNumber;
                    existing.UserDevice = notification.UserDevice;

                    await _userNotificationsRepository.Update(existing);
                }
                else
                {
                    var maxId =
                        await _userNotificationsRepository.Table.MaxAsync(
                            x => (int?)x.NotificationID,
                            cancellationToken
                        ) ?? 0;
                    var newEntity = new UserNotifications
                    {
                        NotificationID = maxId + 1,
                        PushId = notification.PushId,
                        UserCode = notification.UserCode,
                        DateTime = DateTime.Now,
                        PhoneNumber = notification.PhoneNumber,
                        UserDevice = notification.UserDevice,
                    };

                    await _userNotificationsRepository.InsertAsync(newEntity);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            _ = ex.LogErrorAsync();
            return false;
        }
    }
}
