using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Migrations.Builders;

public class NotificationBuilder : O24OpenAPIEntityBuilder<Notification>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Notification.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(Notification.UserCode)).AsString(100).NotNullable()
            .WithColumn(nameof(Notification.AppType)).AsString(10).NotNullable()
            .WithColumn(nameof(Notification.NotificationType)).AsString(10).Nullable()
            .WithColumn(nameof(Notification.DataValue)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(Notification.TemplateID)).AsString(50).NotNullable()
            .WithColumn(nameof(Notification.Redirect)).AsString(50).Nullable()
            .WithColumn(nameof(Notification.IsRead)).AsBoolean().NotNullable()
            .WithColumn(nameof(Notification.IsPushed)).AsBoolean().NotNullable()
            .WithColumn(nameof(Notification.DateTime)).AsDateTime2().Nullable()
            .WithColumn(nameof(Notification.IsProcessed)).AsBoolean().Nullable()
            .WithColumn(nameof(Notification.Message)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(Notification.Title)).AsString(2000).Nullable()
            .WithColumn(nameof(Notification.ImageUrl)).AsString(2000).Nullable();
    }
}
