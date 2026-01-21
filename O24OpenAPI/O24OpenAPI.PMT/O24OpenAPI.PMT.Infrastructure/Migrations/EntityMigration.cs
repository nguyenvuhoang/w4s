using FluentMigrator;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.PMT.Domain.AggregatesModel.PMTAggregate;

namespace O24OpenAPI.PMT.Infrastructure.Migrations;

public class EntityMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<PMTDomain>();
    }
}
