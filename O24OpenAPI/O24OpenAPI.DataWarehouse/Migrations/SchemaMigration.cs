using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.DataWarehouse.Domain;

namespace O24OpenAPI.DataWarehouse.Migrations;

[O24OpenAPIMigration(
    "2025/10/18 11:43:45:0000000",
    "Add Tables AccountStatementDone/AccountStatement",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class SchemaMigration : BaseMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(D_BRANCH)).Exists())
        {
            Create.TableFor<D_BRANCH>();
        }
        if (!Schema.Table(nameof(D_CONTRACT)).Exists())
        {
            Create.TableFor<D_CONTRACT>();
        }
        if (!Schema.Table(nameof(D_CONTRACTACCOUNT)).Exists())
        {
            Create.TableFor<D_CONTRACTACCOUNT>();
        }
        if (!Schema.Table(nameof(D_CUSTOMER)).Exists())
        {
            Create.TableFor<D_CUSTOMER>();
        }
        if (!Schema.Table(nameof(D_COUNTRY)).Exists())
        {
            Create.TableFor<D_COUNTRY>();
        }
        if (!Schema.Table(nameof(D_REGION)).Exists())
        {
            Create.TableFor<D_REGION>();
        }

        if (!Schema.Table(nameof(D_TOWNSHIP)).Exists())
        {
            Create.TableFor<D_TOWNSHIP>();

            if (!Schema.Table(nameof(D_TOWNSHIP)).Constraint("UC_D_TOWNSHIP").Exists())
            {
                Create
                    .UniqueConstraint("UC_D_TOWNSHIP")
                    .OnTable(nameof(D_TOWNSHIP))
                    .Columns(nameof(D_TOWNSHIP.TownshipCode));
            }
        }

        if (!Schema.Table(nameof(C_CODELIST)).Exists())
        {
            Create.TableFor<C_CODELIST>();
            Create
                .UniqueConstraint("UC_C_CODELIST")
                .OnTable(nameof(C_CODELIST))
                .Columns(
                    nameof(C_CODELIST.CodeId),
                    nameof(C_CODELIST.CodeName),
                    nameof(C_CODELIST.CodeGroup)
                );
        }

        if (!Schema.Table(nameof(D_TRANSACTIONHISTORY)).Exists())
        {
            Create.TableFor<D_TRANSACTIONHISTORY>();
        }

        if (!Schema.Table(nameof(AccountStatement)).Exists())
        {
            Create.TableFor<AccountStatement>();
        }
        if (!Schema.Table(nameof(AccountStatementDone)).Exists())
        {
            Create.TableFor<AccountStatementDone>();
        }
    }
}
