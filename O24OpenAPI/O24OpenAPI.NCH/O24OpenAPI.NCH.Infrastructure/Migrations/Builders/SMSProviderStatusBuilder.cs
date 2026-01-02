using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Migrations.Builders;

public class SMSProviderStatusBuilder : O24OpenAPIEntityBuilder<SMSProviderStatus>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(SMSProviderStatus.ProviderId)).AsString(100).NotNullable()
            .WithColumn(nameof(SMSProviderStatus.CheckTime)).AsDateTime().NotNullable()
            .WithColumn(nameof(SMSProviderStatus.IsOnline)).AsBoolean().NotNullable()
            .WithColumn(nameof(SMSProviderStatus.ResponseTimeMs)).AsInt32().Nullable()
            .WithColumn(nameof(SMSProviderStatus.ResponseMessage)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(SMSProviderStatus.ErrorDetail)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(SMSProviderStatus.CreatedOnUtc)).AsDateTime().Nullable()
            .WithColumn(nameof(SMSProviderStatus.UpdatedOnUtc)).AsDateTime().Nullable();
    }
}
