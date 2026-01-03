using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Infrastructure.EntityCongurations;

public class SMSMappingResponseBuilder : O24OpenAPIEntityBuilder<SMSMappingResponse>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(SMSMappingResponse.ProviderName)).AsString(100).NotNullable()
            .WithColumn(nameof(SMSMappingResponse.ResponseCode)).AsString(50).NotNullable()
            .WithColumn(nameof(SMSMappingResponse.ResponseDescription)).AsString(1000).Nullable()
            .WithColumn(nameof(SMSMappingResponse.IsSuccess)).AsBoolean().NotNullable()
            .WithColumn(nameof(SMSMappingResponse.MsgTemplate)).AsString(1000).Nullable()
            .WithColumn(nameof(SMSMappingResponse.ServiceType)).AsString(50).Nullable()
            .WithColumn(nameof(SMSMappingResponse.IsFallback)).AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn(nameof(SMSMappingResponse.IsRetry)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(SMSMappingResponse.DateCreated)).AsDateTime2().NotNullable()
            .WithColumn(nameof(SMSMappingResponse.DateUpdated)).AsDateTime2().NotNullable()
            .WithColumn(nameof(SMSMappingResponse.CreatedBy)).AsString(100).Nullable()
            .WithColumn(nameof(SMSMappingResponse.UpdatedBy)).AsString(100).Nullable();
    }
}
