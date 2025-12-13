namespace O24OpenAPI.O24NCH.Migrations
{
    using FluentMigrator;
    using O24OpenAPI.Core.Attributes;
    using O24OpenAPI.Data;
    using O24OpenAPI.Data.Attributes;
    using O24OpenAPI.Data.Migrations;
    using O24OpenAPI.O24NCH.Domain;

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
            Schema.Table(nameof(UserNotifications)).Exists()
            && !Schema
                .Table(nameof(UserNotifications))
                .Column(nameof(UserNotifications.PushId))
                .Exists()
            )
            {
                Alter
                    .Table(nameof(UserNotifications))
                    .AddColumn(nameof(UserNotifications.PushId))
                    .AsString(500)
                    .Nullable();

            }

            if (
             Schema.Table(nameof(UserNotifications)).Exists()
             && !Schema
                 .Table(nameof(UserNotifications))
                 .Column(nameof(UserNotifications.PhoneNumber))
                 .Exists()
             )
            {
                Alter
                    .Table(nameof(UserNotifications))
                    .AddColumn(nameof(UserNotifications.PhoneNumber))
                    .AsString(100)
                    .Nullable();

            }
            if (
              Schema.Table(nameof(UserNotifications)).Exists()
              && !Schema
                  .Table(nameof(UserNotifications))
                  .Column(nameof(UserNotifications.UserDevice))
                  .Exists()
              )
            {
                Alter
                    .Table(nameof(UserNotifications))
                    .AddColumn(nameof(UserNotifications.UserDevice))
                    .AsString(100)
                    .Nullable();

            }
        }
    }
}
