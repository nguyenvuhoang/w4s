using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

public class D_DEVICEBuilder : O24OpenAPIEntityBuilder<D_DEVICE>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(D_DEVICE.UserCode))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(D_DEVICE.DeviceId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(D_DEVICE.DeviceType))
            .AsString(20)
            .Nullable()
            .WithColumn(nameof(D_DEVICE.AppType))
            .AsString(6)
            .NotNullable()
            .WithColumn(nameof(D_DEVICE.Status))
            .AsString(1)
            .NotNullable()
            .WithColumn(nameof(D_DEVICE.PushID))
            .AsString(1000)
            .Nullable();
    }
}
