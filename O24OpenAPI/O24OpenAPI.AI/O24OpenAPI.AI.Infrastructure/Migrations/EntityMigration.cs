using FluentMigrator;
using O24OpenAPI.AI.Domain.AggregatesModel.SampleAggregate;
using O24OpenAPI.Data.Extensions;

namespace O24OpenAPI.AI.Infrastructure.Migrations;

public class EntityMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<Sample>();
    }
}
