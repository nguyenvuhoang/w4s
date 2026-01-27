using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

namespace O24OpenAPI.NCH.Infrastructure.EntityCongurations;

/// <summary>
/// The Zalo ZNS sendout builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{ZaloZNSSendout}"/>
public class ZaloZNSSendoutBuilder : O24OpenAPIEntityBuilder<ZaloZNSSendout>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ZaloZNSSendout.RefId))
                .AsString(64)
                .NotNullable()

            .WithColumn(nameof(ZaloZNSSendout.OaId))
                .AsString(50)
                .NotNullable()

            .WithColumn(nameof(ZaloZNSSendout.Phone))
                .AsString(20)
                .NotNullable()

            .WithColumn(nameof(ZaloZNSSendout.TemplateId))
                .AsString(50)
                .NotNullable()

            .WithColumn(nameof(ZaloZNSSendout.PayloadJson))
                .AsString(int.MaxValue)
                .Nullable()

            .WithColumn(nameof(ZaloZNSSendout.Status))
                .AsString(20)
                .NotNullable()
                .WithDefaultValue("PENDING")

            .WithColumn(nameof(ZaloZNSSendout.ZaloMsgId))
                .AsString(100)
                .Nullable()

            .WithColumn(nameof(ZaloZNSSendout.ErrorCode))
                .AsString(50)
                .Nullable()

            .WithColumn(nameof(ZaloZNSSendout.ErrorMessage))
                .AsString(500)
                .Nullable()

            .WithColumn(nameof(ZaloZNSSendout.AttemptCount))
                .AsInt32()
                .NotNullable()
                .WithDefaultValue(0)

            .WithColumn(nameof(ZaloZNSSendout.TraceId))
                .AsString(100)
                .Nullable();
    }
}
