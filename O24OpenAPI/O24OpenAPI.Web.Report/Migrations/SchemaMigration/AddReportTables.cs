using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Web.Report.Domain;

namespace O24OpenAPI.Web.Report.Migrations.SchemaMigration;

[O24OpenAPIMigration(
    "2023/07/09 22:05:07:0000000",
    "Add Report Table",
    MigrationProcessType.Installation
)]
[Environment(EnvironmentType.All)]
public class AddReportTables : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(ReportConfig)).Exists())
        {
            Create.TableFor<ReportConfig>();
        }
        if (
            Schema.Table(nameof(ReportConfig)).Exists()
            && !Schema.Table(nameof(ReportConfig)).Column(nameof(ReportConfig.Status)).Exists()
        )
        {
            Alter
                .Table(nameof(ReportConfig))
                .AddColumn(nameof(ReportConfig.Status))
                .AsString(100)
                .Nullable();
        }

        if (
            Schema.Table(nameof(ReportConfig)).Exists()
            && !Schema
                .Table(nameof(ReportConfig))
                .Column(nameof(ReportConfig.OrganizationId))
                .Exists()
        )
        {
            Alter
                .Table(nameof(ReportConfig))
                .AddColumn(nameof(ReportConfig.OrganizationId))
                .AsString(100)
                .Nullable();
        }

        if (!Schema.Table(nameof(ReportTemplate)).Exists())
        {
            Create.TableFor<ReportTemplate>();
        }

        if (!Schema.Table(nameof(ViewerSetting)).Exists())
        {
            Create.TableFor<ViewerSetting>();
        }
        if (!Schema.Table(nameof(ReportData)).Exists())
        {
            Create.TableFor<ReportData>();
        }

        if (
            Schema.Table(nameof(ReportData)).Exists()
            && !Schema
                .Table(nameof(ReportData))
                .Column(nameof(ReportData.ParentDatatable))
                .Exists()
        )
        {
            Alter
                .Table(nameof(ReportData))
                .AddColumn(nameof(ReportData.ParentDatatable))
                .AsString(100)
                .Nullable();
        }

        if (!Schema.Table(nameof(ReportParam)).Exists())
        {
            Create.TableFor<ReportParam>();
        }

        if (!Schema.Table(nameof(ReportDesign)).Exists())
        {
            Create.TableFor<ReportDesign>();
        }

    }
}
