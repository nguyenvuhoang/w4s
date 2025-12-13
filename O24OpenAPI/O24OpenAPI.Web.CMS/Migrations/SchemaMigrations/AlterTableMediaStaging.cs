using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;

namespace O24OpenAPI.Web.CMS.Migrations;

[O24OpenAPIMigration(
    "2025/10/30 13:02:02:0000000",
    "Add Column FolderName into Table MediaStaging/MediaFile",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AlterTableMediaStaging : AutoReversingMigration
{
    public override void Up()
    {
        if (
            Schema.Table(nameof(MediaStaging)).Exists()
            && !Schema
                .Table(nameof(MediaStaging))
                .Column(nameof(MediaStaging.FolderName))
                .Exists()
        )
        {
            Alter
                .Table(nameof(MediaStaging))
                .AddColumn(nameof(MediaStaging.FolderName))
                .AsString(255)
                .Nullable();
        }

        if (
            Schema.Table(nameof(MediaFile)).Exists()
            && !Schema
                .Table(nameof(MediaFile))
                .Column(nameof(MediaFile.FolderName))
                .Exists()
        )
        {
            Alter
                .Table(nameof(MediaFile))
                .AddColumn(nameof(MediaFile.FolderName))
                .AsString(255)
                .Nullable();
        }
    }
}
