using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Services.Mapping;
using O24OpenAPI.Web.Report.GrpcServices;
using O24OpenAPI.Web.Report.Services.Interfaces;
using O24OpenAPI.Web.Report.Services.Services;

namespace O24OpenAPI.Web.Report.Infrastructure;

public class Startup : IO24OpenAPIStartup
{
    public int Order => 2000;

    public void Configure(IApplicationBuilder application)
    {
        application.UseRouting();
        application.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<RPTGrpcService>();
            endpoints.MapControllers();
        });
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IReportConfigService, ReportConfigService>();
        services.AddScoped<ITemplateReportService, TemplateReportService>();
        services.AddScoped<IViewerSettingService, ViewerSettingService>();
        services.AddScoped<IDataMappingService, DataMappingService>();
    }
}
