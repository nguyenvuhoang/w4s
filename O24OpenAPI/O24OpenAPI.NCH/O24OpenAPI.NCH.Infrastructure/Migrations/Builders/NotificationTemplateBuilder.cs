using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Migrations.Builders;

public class NotificationTemplateBuilder : O24OpenAPIEntityBuilder<NotificationTemplate>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(NotificationTemplate.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(NotificationTemplate.TemplateID)).AsString(50).NotNullable()
            .WithColumn(nameof(NotificationTemplate.Title)).AsString(200).NotNullable()
            .WithColumn(nameof(NotificationTemplate.Body)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(NotificationTemplate.Data)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(NotificationTemplate.IsShowButton)).AsBoolean().Nullable()
            .WithColumn(nameof(NotificationTemplate.LearnApiSending)).AsString(700).Nullable();
    }
}
