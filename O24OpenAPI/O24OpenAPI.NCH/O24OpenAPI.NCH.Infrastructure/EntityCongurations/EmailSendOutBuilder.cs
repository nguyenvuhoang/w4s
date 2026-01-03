using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.Infrastructure.EntityCongurations;

public class EmailSendOutBuilder : O24OpenAPIEntityBuilder<EmailSendOut>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(EmailSendOut.ConfigId)).AsString(50).Nullable()
            .WithColumn(nameof(EmailSendOut.TemplateId)).AsString(50).Nullable()
            .WithColumn(nameof(EmailSendOut.Receiver)).AsString(1000).Nullable()
            .WithColumn(nameof(EmailSendOut.Subject)).AsString(2000).Nullable()
            .WithColumn(nameof(EmailSendOut.Body)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(EmailSendOut.Attachments)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(EmailSendOut.Status)).AsString(20).Nullable()
            .WithColumn(nameof(EmailSendOut.ErrorMessage)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(EmailSendOut.SentAt)).AsDateTime2().Nullable()
            .WithColumn(nameof(EmailSendOut.CreatedAt)).AsDateTime2().Nullable();
    }
}
