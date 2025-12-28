using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.Migrations.Builders;

public class MailTemplateBuilder : O24OpenAPIEntityBuilder<MailTemplate>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(MailTemplate.TemplateId)).AsString(100).Nullable()
            .WithColumn(nameof(MailTemplate.Status)).AsString(1).Nullable()
            .WithColumn(nameof(MailTemplate.Description)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(MailTemplate.Subject)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(MailTemplate.Body)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(MailTemplate.DataSample)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(MailTemplate.SendAsPDF)).AsBoolean().NotNullable()
            .WithColumn(nameof(MailTemplate.Attachments)).AsString(int.MaxValue).Nullable();
    }
}
