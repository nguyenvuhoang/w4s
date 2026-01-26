using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Infrastructure.EntityCongurations;

public class GroupUserNotificationBuilder : O24OpenAPIEntityBuilder<GroupUserNotification>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(GroupUserNotification.GroupID)).AsInt32().NotNullable()
            .WithColumn(nameof(GroupUserNotification.UserCode)).AsString(100).NotNullable();
    }
}
