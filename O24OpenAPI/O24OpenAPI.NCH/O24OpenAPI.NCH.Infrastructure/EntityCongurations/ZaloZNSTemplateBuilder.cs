using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

namespace O24OpenAPI.NCH.Infrastructure.EntityCongurations;

public class ZaloZNSTemplateBuilder : O24OpenAPIEntityBuilder<ZaloZNSTemplate>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table

            .WithColumn(nameof(ZaloZNSTemplate.OaId))
                .AsString(50).NotNullable()

            .WithColumn(nameof(ZaloZNSTemplate.TemplateId))
                .AsString(50).NotNullable()

            .WithColumn(nameof(ZaloZNSTemplate.TemplateCode))
                .AsString(50).NotNullable()

            .WithColumn(nameof(ZaloZNSTemplate.TemplateName))
                .AsString(255).NotNullable()

            .WithColumn(nameof(ZaloZNSTemplate.TemplateType))
                .AsString(30).NotNullable()

            .WithColumn(nameof(ZaloZNSTemplate.Status))
                .AsString(20).NotNullable()

            .WithColumn(nameof(ZaloZNSTemplate.RejectReason))
                .AsString(500).Nullable()

            .WithColumn(nameof(ZaloZNSTemplate.RequestPayload))
                .AsString(int.MaxValue).NotNullable()

            .WithColumn(nameof(ZaloZNSTemplate.ResponsePayload))
                .AsString(int.MaxValue).Nullable();
    }
}
