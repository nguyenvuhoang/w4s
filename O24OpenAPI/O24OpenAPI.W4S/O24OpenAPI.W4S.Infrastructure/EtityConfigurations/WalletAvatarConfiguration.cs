using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.EtityConfigurations;

public class WalletAvatarConfiguration : O24OpenAPIEntityBuilder<WalletAvatar>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(WalletAvatar.WalletId))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(WalletAvatar.UserCode))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(WalletAvatar.FileKey))
            .AsString(500)
            .NotNullable()
            .WithColumn(nameof(WalletAvatar.FileUrl))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(WalletAvatar.ContentType))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(WalletAvatar.FileSize))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(WalletAvatar.IsCurrent))
            .AsBoolean()
            .NotNullable()
            .WithDefaultValue(false)
            .WithColumn(nameof(WalletAvatar.SortOrder))
            .AsInt32()
            .NotNullable()
            .WithDefaultValue(0)
            .WithColumn(nameof(WalletAvatar.Status))
            .AsString(20)
            .NotNullable()
            .WithDefaultValue("ACTIVE")
            .WithColumn(nameof(WalletAvatar.CreatedOnUtc))
            .AsDateTime2()
            .NotNullable()
            .WithDefaultValue(SystemMethods.CurrentUTCDateTime)
            .WithColumn(nameof(WalletAvatar.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}
