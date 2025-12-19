using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

[DatabaseType(DataProviderType.Oracle)]
public class ORACLE_MailTemplateBuilder : O24OpenAPIEntityBuilder<MailTemplate>
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
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(MailTemplate.Description))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(MailTemplate.Subject))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(MailTemplate.Body))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(MailTemplate.DataSample))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(MailTemplate.SendAsPDF))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(MailTemplate.Attachments))
            .AsNCLOB()
            .Nullable();
    }
}
