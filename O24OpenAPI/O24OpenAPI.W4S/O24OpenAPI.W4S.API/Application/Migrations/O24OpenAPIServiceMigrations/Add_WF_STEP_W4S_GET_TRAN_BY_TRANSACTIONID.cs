using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Framework.Domain;

namespace O24OpenAPI.W4S.API.Application.Migrations.O24OpenAPIServiceMigrations;

[O24OpenAPIMigration(
    "2026/11/01 22:04:04:0000000",
    "Add_WF_STEP_W4S_GET_TRAN_BY_TRANSACTIONID",
    MigrationProcessType.Update
)]
[Environment(EnvironmentType.All)]
public class Add_WF_STEP_W4S_GET_TRAN_BY_TRANSACTIONID : BaseMigration
{
    public override void Up()
    {
        var step = new O24OpenAPIService
        {
            StepCode = WorkflowStepCode.W4S.WF_STEP_W4S_GET_TRAN_BY_TRANSACTIONID,
            MediatorKey = MediatorKey.W4S,
            IsInquiry = true,
        };
        SeedData(step, [nameof(O24OpenAPIService.StepCode)]).GetAwaiter();
    }
}
