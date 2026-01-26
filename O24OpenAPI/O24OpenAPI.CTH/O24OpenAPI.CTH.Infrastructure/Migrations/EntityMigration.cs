using FluentMigrator;
using O24OpenAPI.CTH.Domain.AggregatesModel.SampleAggregate;
using O24OpenAPI.Data.Extensions;

namespace O24OpenAPI.CTH.Infrastructure.Migrations;

public class EntityMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<Sample>();
    }
}
