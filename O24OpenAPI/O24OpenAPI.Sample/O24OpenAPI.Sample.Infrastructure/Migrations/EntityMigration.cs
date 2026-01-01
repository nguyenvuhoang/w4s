using FluentMigrator;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Sample.Domain.AggregatesModel.SampleAggregate;

namespace O24OpenAPI.Sample.Infrastructure.Migrations;

public class EntityMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<SampleDomain>();
    }
}
