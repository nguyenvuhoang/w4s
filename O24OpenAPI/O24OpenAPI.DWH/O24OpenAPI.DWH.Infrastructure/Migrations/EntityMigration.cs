using FluentMigrator;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.DWH.Domain.AggregatesModel.SampleAggregate;

namespace O24OpenAPI.DWH.Infrastructure.Migrations;

public class EntityMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<SampleDomain>();
    }
}
