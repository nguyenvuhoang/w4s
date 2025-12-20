using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain.AggregateModels.Digital;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

public class SmartOTPInfoBuilder : O24OpenAPIEntityBuilder<SmartOTPInfo>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(SmartOTPInfo.UserCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(SmartOTPInfo.DeviceId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(SmartOTPInfo.PrivateKey))
            .AsString(200)
            .NotNullable()
            .WithColumn(nameof(SmartOTPInfo.Status))
            .AsString(10)
            .NotNullable()
            .WithColumn(nameof(SmartOTPInfo.PinCode))
            .AsString(6)
            .NotNullable()
            .WithColumn(nameof(SmartOTPInfo.CreateTime))
            .AsDateTimeOffset()
            .Nullable();
    }
}
