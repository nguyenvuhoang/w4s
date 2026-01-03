namespace O24OpenAPI.NCH.Migrations;

using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

/// <summary>
/// Defines the <see cref="AlterUserNotifications" />
/// </summary>
[O24OpenAPIMigration(
    "2025/12/01 13:22:01:0000000",
    "Update column PushId into UserNotifications",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
[DatabaseType(DataProviderType.SqlServer)]
public class AlterUserNotifications : AutoReversingMigration
{
    /// <summary>
    /// The Up
    /// </summary>
    public override void Up()
    {
        if (
        Schema.Table(nameof(UserNotification)).Exists()
        && !Schema
            .Table(nameof(UserNotification))
            .Column(nameof(UserNotification.PushId))
            .Exists()
        )
        {
            Alter
                .Table(nameof(UserNotification))
                .AddColumn(nameof(UserNotification.PushId))
                .AsString(500)
                .Nullable();

        }

        if (
         Schema.Table(nameof(UserNotification)).Exists()
         && !Schema
             .Table(nameof(UserNotification))
             .Column(nameof(UserNotification.PhoneNumber))
             .Exists()
         )
        {
            Alter
                .Table(nameof(UserNotification))
                .AddColumn(nameof(UserNotification.PhoneNumber))
                .AsString(100)
                .Nullable();

        }
        if (
          Schema.Table(nameof(UserNotification)).Exists()
          && !Schema
              .Table(nameof(UserNotification))
              .Column(nameof(UserNotification.UserDevice))
              .Exists()
          )
        {
            Alter
                .Table(nameof(UserNotification))
                .AddColumn(nameof(UserNotification.UserDevice))
                .AsString(100)
                .Nullable();

        }
    }
}
