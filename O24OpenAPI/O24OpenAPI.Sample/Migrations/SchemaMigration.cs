using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Framework.Domain.Logging;

namespace O24OpenAPI.Sample.Migrations;

[O24OpenAPIMigration(
    "2025/01/01 14:00:00:0000000",
    "Create ServiceLog",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        //if (!Schema.Table(nameof(ServiceLog)).Exists())
        //{
        //    Create.TableFor<ServiceLog>();
        //}
    }
}

[O24OpenAPIMigration(
    "2025/01/01 14:00:05:0000000",
    "Create HttpLog",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AddHttpLogMigration : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(HttpLog)).Exists())
        {
            Create.TableFor<HttpLog>();
        }
    }
}
