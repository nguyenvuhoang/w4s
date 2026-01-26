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
    "2025/09/05 13:25:06:0000000",
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

            Create
                .Index("IX_NotificationDetail_Status_TargetType_DateTime")
                .OnTable(nameof(NotificationDetail))
                .OnColumn(nameof(NotificationDetail.Status))
                .Descending()
                .OnColumn(nameof(NotificationDetail.TargetType))
                .Ascending()
                .OnColumn(nameof(NotificationDetail.DateTime))
                .Descending();

            Create
                .Index("IX_NotificationDetail_UserCode_DateTime")
                .OnTable(nameof(NotificationDetail))
                .OnColumn(nameof(NotificationDetail.UserCode))
                .Ascending()
                .OnColumn(nameof(NotificationDetail.DateTime))
                .Descending();
        }

        if (!Schema.Table(nameof(UserNotification)).Exists())
        {
            Create.TableFor<UserNotification>();

            Create
                .Index("IX_UserNotifications_UserCode_NotificationID")
                .OnTable(nameof(UserNotification))
                .OnColumn(nameof(UserNotification.UserCode))
                .Ascending()
                .OnColumn(nameof(UserNotification.NotificationID))
                .Ascending();

            Create
                .Index("IX_UserNotifications_UserCode_DateTime")
                .OnTable(nameof(UserNotification))
                .OnColumn(nameof(UserNotification.UserCode))
                .Ascending()
                .OnColumn(nameof(UserNotification.DateTime))
                .Descending();
        }

        if (!Schema.Table(nameof(GroupUserNotification)).Exists())
        {
            Create.TableFor<GroupUserNotification>();

            Create
                .Index("IX_GroupUserNotification_GroupID_UserCode")
                .OnTable(nameof(GroupUserNotification))
                .OnColumn(nameof(GroupUserNotification.GroupID))
                .Ascending()
                .OnColumn(nameof(GroupUserNotification.UserCode))
                .Ascending();
        }
    }
}
