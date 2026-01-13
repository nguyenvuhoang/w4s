using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Design.Domain.AggregatesModel.DesignAggregate;

namespace O24OpenAPI.Design.Infrastructure;

[O24OpenAPIMigration(
    "2025/01/01 14:00:00:0000000",
    "Create APIService/APISpec",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(APIService)).Exists())
        {
            Create.TableFor<APIService>();

            Create.Index("UQ_APIService_ServiceCode")
                .OnTable(nameof(APIService))
                .OnColumn(nameof(APIService.ServiceCode)).Ascending()
                .WithOptions().Unique();
        }

        if (!Schema.Table(nameof(APISpec)).Exists())
        {
            Create.TableFor<APISpec>();

            Create.Index("UQ_APISpec_Service_Version")
                .OnTable(nameof(APISpec))
                .OnColumn(nameof(APISpec.ApiServiceId)).Ascending()
                .OnColumn(nameof(APISpec.Version)).Ascending()
                .WithOptions().Unique();
        }
    }
}