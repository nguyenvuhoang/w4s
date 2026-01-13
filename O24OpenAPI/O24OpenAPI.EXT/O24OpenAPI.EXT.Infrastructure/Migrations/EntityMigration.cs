using FluentMigrator;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.EXT.Domain.AggregatesModel.SampleAggregate;

namespace O24OpenAPI.EXT.Infrastructure.Migrations;

public class EntityMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<SampleDomain>();
    }
}
