using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Web.Report.Domain.MIS;

namespace O24OpenAPI.Web.Report.Migrations.SchemaMigration;

[O24OpenAPIMigration(
    "2025/07/09 22:08:07:0000000",
    "Add MIS report",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class MISReportTable : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(TrialBalance)).Exists())
        {
            Create.TableFor<TrialBalance>();
            Create.Index($"TrialBalance_SubCOA_Idx")
                .OnTable(nameof(TrialBalance))
                .OnColumn(nameof(TrialBalance.SubCOA)).Ascending();
            Create.Index($"TrialBalance_BranchCode_Idx")
                .OnTable(nameof(TrialBalance))
                .OnColumn(nameof(TrialBalance.BranchCode)).Ascending();
            Create.Index($"TrialBalance_StatementDate_Idx")
                .OnTable(nameof(TrialBalance))
                .OnColumn(nameof(TrialBalance.StatementDate)).Ascending();
            Create.Index($"TrialBalance_Currency_Idx")
                .OnTable(nameof(TrialBalance))
                .OnColumn(nameof(TrialBalance.Currency)).Ascending();
        }

        if (!Schema.Table(nameof(DepositListing)).Exists())
        {
            Create.TableFor<DepositListing>();
            Create.Index($"DepositListing_AccountNumber_Idx")
                .OnTable(nameof(DepositListing))
                .OnColumn(nameof(DepositListing.AccountNumber)).Ascending();
            Create.Index($"DepositListing_CustomerCode_Idx")
                .OnTable(nameof(DepositListing))
                .OnColumn(nameof(DepositListing.CustomerCode)).Ascending();
            Create.Index($"DepositListing_StatementDate_Idx")
                .OnTable(nameof(DepositListing))
                .OnColumn(nameof(DepositListing.StatementDate)).Ascending();
            Create.Index($"DepositListing_DepositStatus_Idx")
                .OnTable(nameof(DepositListing))
                .OnColumn(nameof(DepositListing.DepositStatus)).Ascending();
            Create.Index($"DepositListing_DepositType_Idx")
                .OnTable(nameof(DepositListing))
                .OnColumn(nameof(DepositListing.DepositType)).Ascending();
        }

        if (!Schema.Table(nameof(CustomerListing)).Exists())
        {
            Create.TableFor<CustomerListing>();
            Create.Index($"CustomerListing_CustomerCode_Idx")
                .OnTable(nameof(CustomerListing))
                .OnColumn(nameof(CustomerListing.CustomerCode)).Ascending();
            Create.Index($"CustomerListing_CreateDate_Idx")
                .OnTable(nameof(CustomerListing))
                .OnColumn(nameof(CustomerListing.CreateDate)).Ascending();
            Create.Index($"CustomerListing_Province_Idx")
                .OnTable(nameof(CustomerListing))
                .OnColumn(nameof(CustomerListing.Province)).Ascending();
            Create.Index($"CustomerListing_EconomicSector_Idx")
                .OnTable(nameof(CustomerListing))
                .OnColumn(nameof(CustomerListing.EconomicSector)).Ascending();
        }

        if (!Schema.Table(nameof(LoanListing)).Exists())
        {
            Create.TableFor<LoanListing>();
            Create.Index($"LoanListing_AccountNumber_Idx")
                .OnTable(nameof(LoanListing))
                .OnColumn(nameof(LoanListing.AccountNumber)).Ascending();
            Create.Index($"LoanListing_CustomerCode_Idx")
                .OnTable(nameof(LoanListing))
                .OnColumn(nameof(LoanListing.CustomerCode)).Ascending();
            Create.Index($"LoanListing_StatementDate_Idx")
                .OnTable(nameof(LoanListing))
                .OnColumn(nameof(LoanListing.StatementDate)).Ascending();
            Create.Index($"LoanListing_LoanStatus_Idx")
                .OnTable(nameof(LoanListing))
                .OnColumn(nameof(LoanListing.LoanStatus)).Ascending();
            Create.Index($"LoanListing_ClassificationStatus_Idx")
                .OnTable(nameof(LoanListing))
                .OnColumn(nameof(LoanListing.ClassificationStatus)).Ascending();
        }

        if (!Schema.Table(nameof(Branch)).Exists())
        {
            Create.TableFor<Branch>();
            Create.Index($"Branch_BranchCode_Idx")
                .OnTable(nameof(Branch))
                .OnColumn(nameof(Branch.BranchCode)).Ascending();
        }

        if (!Schema.Table(nameof(MISReport)).Exists())
        {
            Create.TableFor<MISReport>();
            Create.Index($"MISReport_ReportCode_Idx")
                .OnTable(nameof(MISReport))
                .OnColumn(nameof(MISReport.ReportCode)).Ascending();
            Create.Index($"MISReport_Item_Idx")
                .OnTable(nameof(MISReport))
                .OnColumn(nameof(MISReport.Item)).Ascending();
        }

        if (!Schema.Table(nameof(MISIndicatorDefinition)).Exists())
        {
            Create.TableFor<MISIndicatorDefinition>();
            Create.Index($"MISIndicatorDefinition_ReportCode_Idx")
                .OnTable(nameof(MISIndicatorDefinition))
                .OnColumn(nameof(MISIndicatorDefinition.ReportCode)).Ascending();
            Create.Index($"MISIndicatorDefinition_ColumnName_Idx")
                .OnTable(nameof(MISIndicatorDefinition))
                .OnColumn(nameof(MISIndicatorDefinition.ColumnName)).Ascending();
        }

    }
}
