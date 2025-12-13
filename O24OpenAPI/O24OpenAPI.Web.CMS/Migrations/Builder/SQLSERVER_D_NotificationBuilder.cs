using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

[DatabaseType(DataProviderType.SqlServer)]
public class SQLSERVER_D_NotificationBuilder : O24OpenAPIEntityBuilder<D_NOTIFICATION>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(D_NOTIFICATION.UserCode))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(D_NOTIFICATION.AppType))
            .AsString(10)
            .NotNullable()
            .WithColumn(nameof(D_NOTIFICATION.NotificationType))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(D_NOTIFICATION.DataValue))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(D_NOTIFICATION.TemplateID))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(D_NOTIFICATION.Redirect))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(D_NOTIFICATION.IsRead))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(D_NOTIFICATION.IsPushed))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(D_NOTIFICATION.DateTime))
            .AsDateTime2()
            .NotNullable()
            .WithColumn(nameof(D_NOTIFICATION.IsProcessed))
            .AsBoolean()
            .Nullable();
    }
}
