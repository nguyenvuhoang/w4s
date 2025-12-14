using FluentMigrator;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.W4S.Domain.AggregatesModel.SampleAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Migrations;

public class EntityMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<Sample>();
    }
}
