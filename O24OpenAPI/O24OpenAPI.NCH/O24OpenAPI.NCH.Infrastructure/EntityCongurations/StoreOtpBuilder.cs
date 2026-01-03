using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.OtpAggregate;

namespace O24OpenAPI.NCH.Infrastructure.EntityCongurations;

public class StoreOtpBuilder : O24OpenAPIEntityBuilder<StoreOtp>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(StoreOtp.PhoneNumber)).AsString(64).NotNullable()
            .WithColumn(nameof(StoreOtp.OtpHash)).AsString(256).NotNullable()
            .WithColumn(nameof(StoreOtp.OtpSalt)).AsString(64).Nullable()
            .WithColumn(nameof(StoreOtp.Platform)).AsInt32().NotNullable()
            .WithColumn(nameof(StoreOtp.StartAt)).AsDateTime().Nullable()
            .WithColumn(nameof(StoreOtp.EndAt)).AsDateTime().Nullable()
            .WithColumn(nameof(StoreOtp.IsActive)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(StoreOtp.MaxUses)).AsInt32().Nullable()
            .WithColumn(nameof(StoreOtp.UsedCount)).AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(StoreOtp.Note)).AsString(512).Nullable();
    }
}
