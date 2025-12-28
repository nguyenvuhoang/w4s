using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Migrations.Builders;

public class PushNotificationLogBuilder : O24OpenAPIEntityBuilder<PushNotificationLog>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(PushNotificationLog.Id)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(PushNotificationLog.Token)).AsString(500).Nullable()
            .WithColumn(nameof(PushNotificationLog.Title)).AsString(500).Nullable()
            .WithColumn(nameof(PushNotificationLog.Body)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(PushNotificationLog.Data)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(PushNotificationLog.ResponseId)).AsString(500).Nullable()
            .WithColumn(nameof(PushNotificationLog.Status)).AsString(50).NotNullable()
            .WithColumn(nameof(PushNotificationLog.ErrorMessage)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(PushNotificationLog.CreatedAt)).AsDateTime().NotNullable()
            .WithColumn(nameof(PushNotificationLog.SentAt)).AsDateTime().Nullable();
    }
}
