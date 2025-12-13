using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Core.Helper;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Report.Domain;
using O24OpenAPI.Web.Report.Mapper;
using O24OpenAPI.Web.Report.Models.Common;
using O24OpenAPI.Web.Report.Services.Interfaces;
using O24OpenAPI.Web.Report.Utils;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report.Web;
using Stimulsoft.System.Web.UI.WebControls;

namespace O24OpenAPI.Web.Report.Controllers;

public class ViewController(
    IViewerSettingService viewerSettingService,
    O24OpenAPI.Web.Framework.Services.Logging.ILogger logger,
    ICTHGrpcClientService cthService
) : BasePublicController
{
    StiReport report = EngineContext.Current.Resolve<StiReport>();
    private readonly IViewerSettingService _viewerSettingService = viewerSettingService;
    private readonly O24OpenAPI.Web.Framework.Services.Logging.ILogger _logger = logger;
    private readonly ICTHGrpcClientService _cthService = cthService;


    private async Task<UserSessions> GetUserSessionsAsync(string token)
    {
        try
        {
            var result = await _cthService.GetUserSessionAsync(token);
            return result.MapToUserSessions();
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            return null;
        }
    }

    /// <summary>
    /// GET: View
    /// </summary>
    /// <returns></returns>
    ///
    [HttpGet]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> Reports(string report_code, string token, string lang)
    {
        StimulsoftUtils.LoadCustomFonts();

        var userSession = await GetUserSessionsAsync(token);
        if (userSession == null)
        {
            return InvokeHttp401();
        }

        if (string.IsNullOrEmpty(lang))
        {
            return InvokeHttp400("Please provide language");
        }

        var getViewerSetting = EngineContext
            .Current.Resolve<IViewerSettingService>()
            .GetByCodeTemplate(report_code)
            .GetAwaiter()
            .GetResult();

        var _viewerOption = new ViewerSetting();

        if (getViewerSetting != null)
        {
            _viewerOption = getViewerSetting;
        }

        var viewerOptions = new StiNetCoreViewerOptions()
        {
            Theme = StiViewerTheme.Office2022WhiteGreen,

            Actions =
            {
                GetReport = "GetReport",
                ViewerEvent = "ViewerEvent",
                Interaction = "ViewerInteraction",
            },

            Server = new()
            {
                CacheMode = StiServerCacheMode.ObjectCache,
                AllowAutoUpdateCache = true,
                RequestTimeout = _viewerOption.RequestTimeout,
                CacheTimeout = _viewerOption.CacheTimeout,
                UseCompression = _viewerOption.UseCompression,
                ShowServerErrorPage = true,
                PassFormValues = true,
                PassQueryParametersToReport = true,
            },

            Appearance =
            {
                BackgroundColor = System.Drawing.Color.FromArgb(0xe8, 0xe8, 0xe8),
                ScrollbarsMode = false,
                ParametersPanelDateFormat = "dd/MM/yyyy hh:mm:ss tt",
                ParametersPanelPosition = StiParametersPanelPosition.Left,
                ParametersPanelColumnsCount = 1,
                FullScreenMode = true,
                PageBorderColor = System.Drawing.Color.FromArgb(0x09, 0x63, 0x3F),
                BookmarksPrint = true,
                DatePickerIncludeCurrentDayForRanges = true,
                CombineReportPages = true,
                AllowTouchZoom = true,
            },

            Toolbar =
            {
                DisplayMode = StiToolbarDisplayMode.Separated,
                ShowDesignButton = _viewerOption.ShowDesignButton,
                ShowOpenButton = _viewerOption.ShowOpenButton,
                ShowPrintButton = _viewerOption.ShowPrintButton,
                ShowResourcesButton = _viewerOption.ShowResourcesButton,
                ShowSaveButton = _viewerOption.ShowSaveButton,
                ShowSendEmailButton = _viewerOption.ShowSendEmailButton,
                ShowParametersButton = true,
                FontFamily = "Century Gothic",
                ViewMode = StiWebViewMode.MultiplePages,
                Zoom = StiZoomMode.PageWidth
            },
            Width = Unit.Percentage(100),
            Height = Unit.Percentage(100),

        };

        // Passing options via ViewBag
        ViewBag.ViewerOptions = viewerOptions;
        HttpContext.Response.Headers.Remove("X-Frame-Options");
        return View();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="report_code"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> GetReport(string report_code, string token, string lang)
    {
        var userSession = await GetUserSessionsAsync(token);
        if (userSession == null)
        {
            return InvokeHttp401();
        }

        try
        {
            var config = await EngineContext.Current.Resolve<IReportConfigService>().GetByCode(report_code);
            var para_ = new List<object> { config, report, userSession, lang };
            if (config != null)
            {
                if (
                    !string.IsNullOrEmpty(config.FullClassName)
                    && !string.IsNullOrEmpty(config.MethodName)
                )
                {
                    report = await ReflectionHelper.DynamicInvoke<Task<StiReport>>(
                        config.FullClassName,
                        config.MethodName,
                        para_.ToArray()
                    );
                }

                try
                {
                    return StiNetCoreViewer.GetReportResult(this, report);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"❌ ArgumentOutOfRangeException: {ex.Message}");
                    Console.WriteLine($"❌ ParamName: {ex.ParamName}");
                    Console.WriteLine($"❌ StackTrace: {ex.StackTrace}");
                    return InvokeHttp500($"Invalid argument: {ex.ParamName} - {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Unexpected error: {ex.Message}");
                    Console.WriteLine($"❌ StackTrace: {ex.StackTrace}");
                    return InvokeHttp500($"Unexpected error: {ex.Message}");
                }
            }
            else
            {
                return InvokeHttp404($"Report {report_code} not found, please check report config");
            }


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            _ = ex.LogErrorAsync();
            return InvokeHttp500(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult ViewerEvent()
    {
        return StiNetCoreViewer.ViewerEventResult(this);
    }

    [HttpPost]
    public async Task<IActionResult> ViewerEvent(string report_code, string token)
    {
        var userSession = await GetUserSessionsAsync(token);
        if (userSession != null && !string.IsNullOrEmpty(userSession.Token))
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }
        return InvokeHttp401();
    }

    [HttpPost]
    public async Task<IActionResult> ViewerInteraction(
        string report_code,
        string token,
        string lang
    )
    {
        var userSession = await GetUserSessionsAsync(token);
        if (userSession != null)
        {
            try
            {
                report = StiNetCoreViewer.GetReportObject(this);
                if (report != null)
                {
                    StiRequestParams requestParams = StiNetCoreViewer.GetRequestParams(this);
                    Dictionary<string, string> rptParams = [];
                    if (requestParams.Action == StiAction.Variables)
                    {
                        if (
                            requestParams.Interaction.Variables != null
                            && report.Dictionary.Variables != null
                        )
                        {
                            report.Dictionary.Variables.Clear();
                        }

                        foreach (var item in requestParams.Interaction.Variables.Keys)
                        {
                            try
                            {
                                string key = item.ToString();
                                string values =
                                    requestParams.Interaction.Variables[item.ToString()]?.ToString()
                                    ?? string.Empty;
                                rptParams[key] = values;
                                report.Dictionary.Variables.Add(key, values);
                            }
                            catch (Exception ex)
                            {
                                await ex.LogErrorAsync();
                            }
                        }
                    }

                    var config = await EngineContext.Current.Resolve<IReportConfigService>().GetByCode(report_code);

                    var userInfo = UpdateParams(userSession, rptParams);
                    var para_ = new List<object> { config, report, userInfo, lang };
                    if (config != null)
                    {
                        if (
                            !string.IsNullOrEmpty(config.FullClassName)
                            && !string.IsNullOrEmpty(config.MethodName)
                        )
                        {
                            report = await ReflectionHelper.DynamicInvoke<Task<StiReport>>(
                                config.FullClassName,
                                config.MethodName,
                                [.. para_]
                            );
                        }
                    }
                    var res = StiNetCoreViewer.InteractionResult(this, report);
                    return res;
                }
                return InvokeHttp404($"Report {report_code} have been not initialized successfully, please check config again. Maybe missing report parameter");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                report?.Dispose();
                await ex.LogErrorAsync();
                return InvokeHttp500(ex.Message);
            }
            finally { }
        }
        return InvokeHttp401();
    }

    private static UserSessions UpdateParams(
        UserSessions userInfo,
        Dictionary<string, string> rptParams
    )
    {
        try
        {
            if (userInfo != null)
            {
                userInfo.Parameter = rptParams;
            }

            return userInfo;
        }
        catch
        {
            return userInfo;
        }
    }
}
