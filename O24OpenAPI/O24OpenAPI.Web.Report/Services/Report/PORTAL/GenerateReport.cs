using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.DBContext;
using O24OpenAPI.Framework.Services.Mapping;
using O24OpenAPI.Web.Report.Domain;
using O24OpenAPI.Web.Report.Models.Common;
using O24OpenAPI.Web.Report.Utils;
using Stimulsoft.Report;

namespace O24OpenAPI.Web.Report.Services.Report.PORTAL;

public class GenerateReport(IDataMappingService dataMappingService)
{
    private readonly IDataMappingService _dataMappingService = dataMappingService;
    private readonly IRepository<ReportComponent> reportcomponent = EngineContext.Current.Resolve<
        IRepository<ReportComponent>
    >();
    private readonly IRepository<ReportParam> reportparam = EngineContext.Current.Resolve<
        IRepository<ReportParam>
    >();
    private readonly IRepository<ReportData> reportdata = EngineContext.Current.Resolve<
        IRepository<ReportData>
    >();
    private readonly IRepository<ReportDesign> reportdesign = EngineContext.Current.Resolve<
        IRepository<ReportDesign>
    >();
    private readonly IRepository<ReportTemplate> reportTemplate = EngineContext.Current.Resolve<
        IRepository<ReportTemplate>
    >();

    public async Task<StiReport> LoadData(
        ReportConfig reportConfig,
        StiReport report,
        UserSessions userSessions,
        string lang
    )
    {
        Stopwatch sw = new();
        sw.Start();
        TimeSpan timeTaken = sw.Elapsed;
        Console.WriteLine("Startwatch: " + timeTaken.ToString(@"m\:ss\.fff"));
        StimulsoftUtils.LoadCustomFonts();
        DateTime _from = DateTime.Today.Date;
        DateTime _to = DateTime.Today.Date;
        var dbContext = new ServiceDBContext();

        var reporttemplate = (
            from rt in reportTemplate.Table
            where rt.Code == reportConfig.CodeTemplate
            select rt
        ).FirstOrDefault();

        var reportcomponentlist = await (
            from rc in reportcomponent.Table
            where rc.ReportCode == reportConfig.Code
            select rc
        ).ToListAsync();

        var reportparamlist = await (
            from rs in reportparam.Table
            where rs.ReportCode == reportConfig.Code && rs.LangId == lang
            orderby rs.Orderby
            select rs
        ).ToListAsync();

        var reportdatalist = await (
            from rs in reportdata.Table
            where rs.ReportCode == reportConfig.Code
            select rs
        ).ToListAsync();

        var reportdesignlist = await (
            from rs in reportdesign.Table
            where rs.ReportCode == reportConfig.Code
            orderby rs.OrderBy
            select rs
        ).ToListAsync();

        //ReportSanitizer.SanitizeDesigns(reportdesignlist);
        //ReportSanitizer.SanitizeDataDefs(reportdatalist);

        StiReport _report;
        if (report == null)
        {
            _report = StimulsoftUtils.Init(reportConfig, reporttemplate);
        }
        else
        {
            _report = report;
        }

        bool isViewerInteraction = _report
            .Dictionary.Variables.ToList()
            .Any(v => !string.IsNullOrEmpty(report.GetValue(v.Name)?.ToString()));

        if (!isViewerInteraction)
        {
            StimulsoftUtils.GenerateDynamicReport(_report, reportcomponentlist, reportdesignlist);
            StimulsoftUtils.GenerateStiVariables(_report, reportparamlist);
            StimulsoftUtils.GenerateDynamicDataSource(_report, reportdatalist, reportdesignlist);
        }
        else
        {
            var userInput = StimulsoftUtils.ExtractReportVariables(report);
            StimulsoftUtils.GenerateDynamicExpressions(
                _report,
                reportdesignlist,
                userInput,
                _dataMappingService
            );
            StimulsoftUtils.GenerateDynamicDatabind(
                _report,
                reportdatalist,
                reportdesignlist,
                userInput,
                _dataMappingService
            );
        }

        TimeSpan timeStopTaken = sw.Elapsed;
        Console.WriteLine("Stopwatch: " + timeStopTaken.ToString(@"m\:ss\.fff"));
        sw.Stop();
        _report.Dictionary.Synchronize();
        ReportDesignValidator.ValidateDesigns(reportdesignlist);
        try
        {
            _report.Save("debug_report.mrt");
            _report.Compile();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
        _report.Render();
        await Task.CompletedTask;
        return _report;
    }
}
