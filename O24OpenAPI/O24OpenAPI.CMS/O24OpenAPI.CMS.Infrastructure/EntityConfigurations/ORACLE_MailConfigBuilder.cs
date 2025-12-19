using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

[DatabaseType(DataProviderType.Oracle)]
public class ORACLE_MailConfigBuilder : O24OpenAPIEntityBuilder<MailConfig>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(MailConfig.Id))
            .AsInt32()
            .PrimaryKey()
            .Identity()
            .WithColumn(nameof(MailConfig.ConfigId))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(MailConfig.Host))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(MailConfig.Port))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(MailConfig.Sender))
            .AsNCLOB()
            .NotNullable()
            .WithColumn(nameof(MailConfig.Password))
            .AsNCLOB()
            .Nullable()
            .WithColumn(nameof(MailConfig.EnableTLS))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(MailConfig.EmailTest))
            .AsNCLOB()
            .Nullable();
    }
}
