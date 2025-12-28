using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Migrations.Builders;

public class SMSProviderBuilder : O24OpenAPIEntityBuilder<SMSProvider>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(SMSProvider.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(SMSProvider.ProviderName)).AsString(100).NotNullable()
            .WithColumn(nameof(SMSProvider.ApiUrl)).AsString(1000).NotNullable()
            .WithColumn(nameof(SMSProvider.CountryPrefix)).AsString(10).NotNullable()
            .WithColumn(nameof(SMSProvider.AllowedPrefix)).AsString(100).Nullable()
            .WithColumn(nameof(SMSProvider.ApiUsername)).AsString(100).Nullable()
            .WithColumn(nameof(SMSProvider.ApiPassword)).AsString(100).Nullable()
            .WithColumn(nameof(SMSProvider.ApiKey)).AsString(200).Nullable()
            .WithColumn(nameof(SMSProvider.IsActive)).AsBoolean().NotNullable().WithDefaultValue(true);
    }
}
