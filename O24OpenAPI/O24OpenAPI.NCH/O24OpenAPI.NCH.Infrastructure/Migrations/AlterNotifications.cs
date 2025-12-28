namespace O24OpenAPI.NCH.Migrations;

using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.NCH.Domain;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

/// <summary>
/// Defines the <see cref="AlterUserNotifications" />
/// </summary>
[O24OpenAPIMigration(
    "2025/12/03 17:11:01:0000000",
    "Update column Title into Notification",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
[DatabaseType(DataProviderType.SqlServer)]
public class AlterNotifications : AutoReversingMigration
{
    /// <summary>
    /// The Up
    /// </summary>
    public override void Up()
    {
        if (
        Schema.Table(nameof(Notification)).Exists()
        && !Schema
            .Table(nameof(Notification))
            .Column(nameof(Notification.NotificationCategory))
            .Exists()
        )
        {
            Alter
                .Table(nameof(Notification))
                .AddColumn(nameof(Notification.NotificationCategory))
                .AsString(500)
                .Nullable();

        }
        if (
           Schema.Table(nameof(Notification)).Exists()
           && !Schema
               .Table(nameof(Notification))
               .Column(nameof(Notification.Message))
               .Exists()
        )
        {
            Alter
                .Table(nameof(Notification))
                .AddColumn(nameof(Notification.Message))
                .AsString(int.MaxValue)
                .Nullable();

        }
        if (
           Schema.Table(nameof(Notification)).Exists()
           && !Schema
               .Table(nameof(Notification))
               .Column(nameof(Notification.Title))
               .Exists()
        )
        {
            Alter
                .Table(nameof(Notification))
                .AddColumn(nameof(Notification.Title))
                .AsString(2000)
                .Nullable();

        }
        if (
          Schema.Table(nameof(Notification)).Exists()
          && !Schema
              .Table(nameof(Notification))
              .Column(nameof(Notification.ImageUrl))
              .Exists()
       )
        {
            Alter
                .Table(nameof(Notification))
                .AddColumn(nameof(Notification.ImageUrl))
                .AsString(2000)
                .Nullable();

        }
    }
}
