using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Migrations.Builders;

public class SMSProviderConfigBuilder : O24OpenAPIEntityBuilder<SMSProviderConfig>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(SMSProviderConfig.SMSProviderId)).AsString(50).NotNullable()
            .WithColumn(nameof(SMSProviderConfig.ConfigKey)).AsString(100).NotNullable()
            .WithColumn(nameof(SMSProviderConfig.ConfigValue)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(SMSProviderConfig.Description)).AsString(255).Nullable()
            .WithColumn(nameof(SMSProviderConfig.IsActive)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(SMSProviderConfig.IsMainKey)).AsBoolean().Nullable();
        ;
    }
}
