using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

[DatabaseType(DataProviderType.Oracle)]
public class ORACLE_TranslationLanguagesBuilder : O24OpenAPIEntityBuilder<TranslationLanguages>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(TranslationLanguages.ChannelId))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(TranslationLanguages.Language))
            .AsString(10)
            .NotNullable()
            .WithColumn(nameof(TranslationLanguages.JSONContent))
            .AsNCLOB()
            .NotNullable()
            .WithColumn(nameof(TranslationLanguages.Version))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(TranslationLanguages.UserCreated))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(TranslationLanguages.UserModified))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(TranslationLanguages.CreatedAt))
            .AsDateTime()
            .NotNullable()
            .WithColumn(nameof(TranslationLanguages.UpdatedAt))
            .AsDateTime()
            .NotNullable();
    }
}
