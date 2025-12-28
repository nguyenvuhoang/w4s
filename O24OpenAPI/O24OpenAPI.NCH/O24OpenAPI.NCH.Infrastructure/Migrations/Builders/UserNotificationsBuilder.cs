using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Migrations.Builders;

public class UserNotificationsBuilder : O24OpenAPIEntityBuilder<UserNotifications>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserNotifications.NotificationID)).AsInt32().NotNullable()
            .WithColumn(nameof(UserNotifications.UserCode)).AsString(100).NotNullable()
            .WithColumn(nameof(UserNotifications.DateTime)).AsDateTime2().Nullable()
            .WithColumn(nameof(UserNotifications.PushId)).AsString(500).Nullable()
            .WithColumn(nameof(UserNotifications.PhoneNumber)).AsString(100).Nullable()
            .WithColumn(nameof(UserNotifications.UserDevice)).AsString(100).Nullable();
    }
}
