using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

[DatabaseType(DataProviderType.SqlServer)]
public class SQLSERVER_MailTemplateBuilder : O24OpenAPIEntityBuilder<MailTemplate>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(MailTemplate.Id))
            .AsInt32()
            .PrimaryKey()
            .Identity()
            .WithColumn(nameof(MailTemplate.TemplateId))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(MailTemplate.Status))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(MailTemplate.Description))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(MailTemplate.Subject))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(MailTemplate.Body))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(MailTemplate.DataSample))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(MailTemplate.SendAsPDF))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(MailTemplate.Attachments))
            .AsString(int.MaxValue)
            .Nullable();
    }
}
