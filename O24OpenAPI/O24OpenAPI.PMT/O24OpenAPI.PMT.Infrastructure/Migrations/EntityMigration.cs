using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.PMT.Domain.AggregatesModel.VNPayAggregate;

namespace O24OpenAPI.PMT.Infrastructure.Migrations;

[O24OpenAPIMigration(
    "2026/01/22 11:58:01:0000000",
    "6. Create SchemeMigration (Business Table) (VNPayTransactionStatusMap/VNPayResponseCodeMap)",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class EntityMigration : AutoReversingMigration
{
    public override void Up()
    {

        if (!Schema.Table(nameof(VNPayTransactionStatusMap)).Exists())
        {
            Create.TableFor<VNPayTransactionStatusMap>();

            Create.Index("UQ_VNPayTransactionStatusMap_StatusCode_StatusMessage")
                .OnTable(nameof(VNPayTransactionStatusMap))
                .OnColumn(nameof(VNPayTransactionStatusMap.StatusCode)).Ascending()
                .WithOptions().Unique();
        }
        if (!Schema.Table(nameof(VNPayResponseCodeMap)).Exists())
        {
            Create.TableFor<VNPayResponseCodeMap>();

            Create.Index("UQ_VNPayResponseCodeMap_ResponseCode_Description")
                .OnTable(nameof(VNPayResponseCodeMap))
                .OnColumn(nameof(VNPayResponseCodeMap.ResponseCode)).Ascending()
                .WithOptions().Unique();
        }
    }
}
