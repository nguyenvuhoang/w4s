using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

public class UserDeviceBuilder : O24OpenAPIEntityBuilder<UserDevice>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserDevice.UserCode))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserDevice.DeviceId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserDevice.DeviceType))
            .AsString(20)
            .Nullable()
            .WithColumn(nameof(UserDevice.ChannelId))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(UserDevice.Status))
            .AsString(1)
            .NotNullable()
            .WithColumn(nameof(UserDevice.PushId))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(UserDevice.UserAgent))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(UserDevice.IpAddress))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(UserDevice.OsVersion))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserDevice.AppVersion))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserDevice.DeviceName))
            .AsString(200)
            .Nullable()
            .WithColumn(nameof(UserDevice.Brand))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(UserDevice.IsEmulator))
            .AsBoolean()
            .WithDefaultValue(false)
            .WithColumn(nameof(UserDevice.IsRootedOrJailbroken))
            .AsBoolean()
            .WithDefaultValue(false)
            .WithColumn(nameof(UserDevice.Network))
            .AsString(500)
            .Nullable()
            .WithColumn(nameof(UserDevice.Memory))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(UserDevice.LastSeenDateUtc))
            .AsDateTime()
            .Nullable(); ;
    }
}
