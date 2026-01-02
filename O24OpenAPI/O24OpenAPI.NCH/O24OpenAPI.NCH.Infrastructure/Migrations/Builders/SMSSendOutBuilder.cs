using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Migrations.Builders;

public class SMSSendOutBuilder : O24OpenAPIEntityBuilder<SMSSendOut>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
             .WithColumn(nameof(SMSSendOut.EndToEnd)).AsString(100).Nullable()
             .WithColumn(nameof(SMSSendOut.PhoneNumber)).AsString(20).NotNullable()
             .WithColumn(nameof(SMSSendOut.MessageContent)).AsString(int.MaxValue).NotNullable()
             .WithColumn(nameof(SMSSendOut.SentAt)).AsDateTime2().NotNullable()
             .WithColumn(nameof(SMSSendOut.Status)).AsString(20).NotNullable()
             .WithColumn(nameof(SMSSendOut.FullRequest)).AsString(int.MaxValue).Nullable()
             .WithColumn(nameof(SMSSendOut.RequestHeader)).AsString(int.MaxValue).Nullable()
             .WithColumn(nameof(SMSSendOut.RequestMessage)).AsString(int.MaxValue).Nullable()
             .WithColumn(nameof(SMSSendOut.ResponseMessage)).AsString(int.MaxValue).Nullable()
             .WithColumn(nameof(SMSSendOut.SMSProviderId)).AsString(200).Nullable()
             .WithColumn(nameof(SMSSendOut.OtpRequestId)).AsGuid().NotNullable()
             .WithColumn(nameof(SMSSendOut.FallbackFromProvider)).AsString(100).Nullable()
             .WithColumn(nameof(SMSSendOut.IsFallback)).AsBoolean().Nullable()
             .WithColumn(nameof(SMSSendOut.IsResend)).AsBoolean().Nullable()
             .WithColumn(nameof(SMSSendOut.RetryCount)).AsInt32().Nullable()
             .WithColumn(nameof(SMSSendOut.ElapsedMilliseconds)).AsInt32().Nullable()
             .WithColumn(nameof(SMSSendOut.ErrorMessage)).AsString(int.MaxValue).Nullable();
    }
}
