using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.CTH.Domain.AggregatesModel.TestAggregate;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;

namespace O24OpenAPI.CTH.Infrastructure.Migrations;

[O24OpenAPIMigration(
    "2025/12/26 16:19:07:0000000",
    "4. Init table CMS Form",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AddTableTest : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<Test>();
    }
}
