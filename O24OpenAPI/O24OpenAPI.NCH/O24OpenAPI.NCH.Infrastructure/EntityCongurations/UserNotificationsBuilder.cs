using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Infrastructure.EntityCongurations;

public class UserNotificationsBuilder : O24OpenAPIEntityBuilder<UserNotification>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserNotification.NotificationID)).AsInt32().NotNullable()
            .WithColumn(nameof(UserNotification.UserCode)).AsString(100).NotNullable()
            .WithColumn(nameof(UserNotification.DateTime)).AsDateTime2().Nullable()
            .WithColumn(nameof(UserNotification.PushId)).AsString(500).Nullable()
            .WithColumn(nameof(UserNotification.PhoneNumber)).AsString(100).Nullable()
            .WithColumn(nameof(UserNotification.UserDevice)).AsString(100).Nullable();
    }
}
