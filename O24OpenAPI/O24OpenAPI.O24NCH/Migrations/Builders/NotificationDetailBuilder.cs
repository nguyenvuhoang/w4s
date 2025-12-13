using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.O24NCH.Domain;

namespace O24OpenAPI.O24NCH.Migrations.Builders;

public class NotificationDetailBuilder : O24OpenAPIEntityBuilder<NotificationDetail>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(NotificationDetail.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(NotificationDetail.AppType)).AsString(10).NotNullable()
            .WithColumn(nameof(NotificationDetail.NotificationType)).AsString(10).Nullable()
            .WithColumn(nameof(NotificationDetail.NotificationCategory)).AsString(50).Nullable()
            .WithColumn(nameof(NotificationDetail.Description)).AsString(500).Nullable()
            .WithColumn(nameof(NotificationDetail.TargetType)).AsString(10).Nullable()
            .WithColumn(nameof(NotificationDetail.GroupID)).AsInt32().Nullable()
            .WithColumn(nameof(NotificationDetail.UserCode)).AsString(100).Nullable()
            .WithColumn(nameof(NotificationDetail.Body)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(NotificationDetail.IsPushed)).AsBoolean().NotNullable()
            .WithColumn(nameof(NotificationDetail.DateTime)).AsDateTime2().Nullable()
            .WithColumn(nameof(NotificationDetail.Status)).AsBoolean().Nullable();
    }
}
