using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.Infrastructure.EntityCongurations;

public class MailConfigBuilder : O24OpenAPIEntityBuilder<MailConfig>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(MailConfig.ConfigId)).AsString(100).Nullable()
            .WithColumn(nameof(MailConfig.Host)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(MailConfig.Port)).AsInt32().NotNullable()
            .WithColumn(nameof(MailConfig.Sender)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(MailConfig.Password)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(MailConfig.EnableTLS)).AsBoolean().NotNullable()
            .WithColumn(nameof(MailConfig.EmailTest)).AsString(int.MaxValue).Nullable();
    }
}
