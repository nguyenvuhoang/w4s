using FluentMigrator;
using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.CMS.Domain.AggregateModels.AppAggregate;
using O24OpenAPI.CMS.Domain.AggregateModels.LearnApiAggregate;
using O24OpenAPI.CMS.Domain.AggregateModels.PORTAL;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;

namespace O24OpenAPI.CMS.Infrastructure.Migrations;

[O24OpenAPIMigration(
    "2025/01/01 21:18:07:0000000",
    "4. Init table CMS Form",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class TableMigration : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(LearnApi)).Exists())
        {
            Create.TableFor<LearnApi>();
            Create
                .UniqueConstraint("UC_LearnApi_LearnApiId_Channel")
                .OnTable(nameof(LearnApi))
                .Columns(nameof(LearnApi.LearnApiId), nameof(LearnApi.Channel));
        }

        if (!Schema.Table(nameof(Form)).Exists())
        {
            Create.TableFor<Form>();
            Create.UniqueConstraint("UC_Form").OnTable(nameof(Form)).Column(nameof(Form.FormId));
        }

        if (!Schema.Table(nameof(MediaFile)).Exists())
        {
            Create.TableFor<MediaFile>();

            Create
                .Index("IDX_MediaFile_TrackerCode")
                .OnTable(nameof(MediaFile))
                .OnColumn(nameof(MediaFile.TrackerCode))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IDX_MediaFile_Status")
                .OnTable(nameof(MediaFile))
                .OnColumn(nameof(MediaFile.Status))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IDX_MediaFile_ExpiredOnUtc")
                .OnTable(nameof(MediaFile))
                .OnColumn(nameof(MediaFile.ExpiredOnUtc))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IDX_MediaFile_FileHash")
                .OnTable(nameof(MediaFile))
                .OnColumn(nameof(MediaFile.FileHash))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IDX_MediaFile_CreatedOnUtc")
                .OnTable(nameof(MediaFile))
                .OnColumn(nameof(MediaFile.CreatedOnUtc))
                .Ascending()
                .WithOptions()
                .NonClustered();
            Create
                .UniqueConstraint("UQ_MediaFile_TrackerCode_FileHash")
                .OnTable(nameof(MediaFile))
                .Columns(nameof(MediaFile.TrackerCode), nameof(MediaFile.FileHash));
        }

        if (!Schema.Table(nameof(MediaStaging)).Exists())
        {
            Create.TableFor<MediaStaging>();

            Create
                .Index("IDX_MediaStaging_FileHash")
                .OnTable(nameof(MediaStaging))
                .OnColumn(nameof(MediaStaging.FileHash))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IDX_MediaStaging_Status")
                .OnTable(nameof(MediaStaging))
                .OnColumn(nameof(MediaStaging.Status))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create
                .Index("IDX_MediaStaging_ExpiredOnUtc")
                .OnTable(nameof(MediaStaging))
                .OnColumn(nameof(MediaStaging.ExpiredOnUtc))
                .Ascending()
                .WithOptions()
                .NonClustered();
        }
    }
}
