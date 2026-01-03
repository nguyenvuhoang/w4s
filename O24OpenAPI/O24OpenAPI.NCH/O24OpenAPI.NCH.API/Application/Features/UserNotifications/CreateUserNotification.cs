using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.UserNotifications;

public class CreateUserNotificationCommand : BaseTransactionModel, ICommand<bool>
{
    public string UserCode { get; set; }
    public string Category { get; set; }
}

[CqrsHandler]
public class CreateUserNotificationHandler(
    INotificationDetailRepository notificationDetailRepository,
    IGroupUserNotificationRepository groupUserNotificationRepository,
    IUserNotificationsRepository userNotificationsRepository
) : ICommandHandler<CreateUserNotificationCommand, bool>
{
    [WorkflowStep(WorkflowStep.NCH.WF_STEP_NCH_CREATE_USER_NOTIFICATIONS)]
    public async Task<bool> HandleAsync(
        CreateUserNotificationCommand request,
        CancellationToken cancellationToken = default
    )
    {
        string userCode = request.UserCode.Trim();
        List<int> unreadNotifications = await notificationDetailRepository
            .Table.Where(n =>
                n.Status == true
                && n.NotificationCategory == request.Category
                && (
                    n.TargetType == "All"
                    || (
                        n.TargetType == "Group"
                        && groupUserNotificationRepository.Table.Any(g =>
                            g.GroupID == n.GroupID && g.UserCode == userCode
                        )
                    )
                    || (n.TargetType == "User" && n.UserCode == userCode)
                )
                && !userNotificationsRepository.Table.Any(u =>
                    u.NotificationID == n.Id && u.UserCode == userCode
                )
            )
            .Select(n => n.Id)
            .ToListAsync();

        if (unreadNotifications.Count == 0)
        {
            return false;
        }

        foreach (int id in unreadNotifications)
        {
            var entity = new UserNotification
            {
                NotificationID = id,
                UserCode = userCode,
                DateTime = DateTime.Now,
            };

            await userNotificationsRepository.InsertAsync(entity);
        }
        return true;
    }
}
