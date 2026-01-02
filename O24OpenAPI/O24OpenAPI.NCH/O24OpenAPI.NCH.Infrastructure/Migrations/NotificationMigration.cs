using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Migrations;

/// <summary>
/// The schema migration class
/// </summary>
/// <seealso cref="AutoReversingMigration"/>
[O24OpenAPIMigration(
    "2025/09/05 13:25:05:0000000",
    "Created NotificationDetail, UserNotifications, GroupUserNotification Table",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]

public class NotificationMigration : AutoReversingMigration
{
    /// <summary>
    /// Ups this instance
    /// </summary>
    public override void Up()
    {
        if (!Schema.Table(nameof(NotificationDetail)).Exists())
        {
            Create.TableFor<NotificationDetail>();

            Create.Index("IX_NotificationDetail_Status_TargetType_DateTime")
                .OnTable(nameof(NotificationDetail))
                .OnColumn(nameof(NotificationDetail.Status)).Descending()
                .OnColumn(nameof(NotificationDetail.TargetType)).Ascending()
                .OnColumn(nameof(NotificationDetail.DateTime)).Descending();

            Create.Index("IX_NotificationDetail_UserCode_DateTime")
                .OnTable(nameof(NotificationDetail))
                .OnColumn(nameof(NotificationDetail.UserCode)).Ascending()
                .OnColumn(nameof(NotificationDetail.DateTime)).Descending();
        }

        if (!Schema.Table(nameof(UserNotifications)).Exists())
        {
            Create.TableFor<UserNotifications>();

            Create.Index("IX_UserNotifications_UserCode_NotificationID")
                .OnTable(nameof(UserNotifications))
                .OnColumn(nameof(UserNotifications.UserCode)).Ascending()
                .OnColumn(nameof(UserNotifications.NotificationID)).Ascending();

            Create.Index("IX_UserNotifications_UserCode_DateTime")
                .OnTable(nameof(UserNotifications))
                .OnColumn(nameof(UserNotifications.UserCode)).Ascending()
                .OnColumn(nameof(UserNotifications.DateTime)).Descending();
        }

        if (!Schema.Table(nameof(GroupUserNotification)).Exists())
        {
            Create.TableFor<GroupUserNotification>();

            Create.Index("IX_GroupUserNotification_GroupID_UserCode")
                .OnTable(nameof(GroupUserNotification))
                .OnColumn(nameof(GroupUserNotification.GroupID)).Ascending()
                .OnColumn(nameof(GroupUserNotification.UserCode)).Ascending();
        }
    }
}
