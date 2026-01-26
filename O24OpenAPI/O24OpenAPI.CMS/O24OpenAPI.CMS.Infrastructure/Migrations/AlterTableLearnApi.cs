using FluentMigrator;
using O24OpenAPI.CMS.Domain.AggregateModels.LearnApiAggregate;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;

namespace O24OpenAPI.CMS.Infrastructure.Migrations
{
    [O24OpenAPIMigration(
        "2025/12/25 21:19:07:0000000",
        "Add field IsInternal",
        MigrationProcessType.Installation
    )]
    [Environment(EnvironmentType.All)]
    public class AlterTableLearnApi : AutoReversingMigration
    {
        public override void Up()
        {
            if (
                Schema.Table(nameof(LearnApi)).Exists()
                && !Schema.Table(nameof(LearnApi)).Column(nameof(LearnApi.IsInternal)).Exists()
            )
            {
                Alter
                    .Table(nameof(LearnApi))
                    .AddColumn(nameof(LearnApi.IsInternal))
                    .AsBoolean()
                    .Nullable();
            }
        }
    }
}
