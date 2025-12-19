using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

[DatabaseType(DataProviderType.SqlServer)]
public class SQLSERVER_MediaFileBuilder : O24OpenAPIEntityBuilder<MediaFile>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(MediaFile.TrackerCode))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(MediaFile.FileUrl))
            .AsString(500)
            .Nullable()
            .WithColumn(nameof(MediaFile.FileHash))
            .AsString(128)
            .Nullable()
            .WithColumn(nameof(MediaFile.Base64String))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(MediaFile.FolderName))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MediaFile.FileName))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(MediaFile.FileExtension))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(MediaFile.FileSize))
            .AsInt64()
            .Nullable()
            .WithColumn(nameof(MediaFile.Status))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(MediaFile.CreatedOnUtc))
            .AsDateTime2()
            .NotNullable()
            .WithColumn(nameof(MediaFile.ExpiredOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(MediaFile.CreatedBy))
            .AsString(100)
            .Nullable();
    }
}
