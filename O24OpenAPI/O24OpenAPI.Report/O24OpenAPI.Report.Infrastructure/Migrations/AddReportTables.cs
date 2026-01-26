using FluentMigrator;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Migrations;
using O24OpenAPI.Report.Domain.AggregateModels.ReportAggregate;
using O24OpenAPI.Report.Domain.AggregateModels.ViewerSettingAggregate;

namespace O24OpenAPI.Report.Infrastructure.Migrations;

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
