using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

[DatabaseType(DataProviderType.SqlServer)]
public class SQLSERVER_MediaStagingBuilder : O24OpenAPIEntityBuilder<MediaStaging>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(MediaStaging.TrackerCode))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(MediaStaging.FileUrl))
            .AsString(500)
            .Nullable()
            .WithColumn(nameof(MediaStaging.Base64String))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(MediaStaging.FolderName))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MediaStaging.FileName))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MediaStaging.FileExtension))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(MediaStaging.FileSize))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(MediaStaging.FileHash))
            .AsString(128)
            .Nullable()
            .WithColumn(nameof(MediaStaging.Status))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(MediaStaging.CreatedOnUtc))
            .AsDateTime2()
            .NotNullable()
            .WithColumn(nameof(MediaStaging.ExpiredOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(MediaStaging.CreatedBy))
            .AsString(100)
            .Nullable();
    }
}
