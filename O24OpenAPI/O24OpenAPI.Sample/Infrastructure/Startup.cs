using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Sample.Services;
using O24OpenAPI.Sample.Services.Interfaces;
using O24OpenAPI.Web.Framework.Domain.Logging;

namespace O24OpenAPI.Sample.Infrastructure;

public class Startup : IO24OpenAPIStartup
{
    public int Order => 2000;

    public void Configure(IApplicationBuilder application) { }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // services.AddScoped<IServiceLogService, ServiceLogService>();
        // services.AddScoped<IHttpLogService, HttpLogService>();
        services.AddScoped<ILogService<HttpLog>, HttpLogService>();
        //services.AddScoped<ILogService<ServiceLog>, ServiceLogService>();

        //services.AddScoped<LogCommandHandler<HttpLog>>();
        //services.AddScoped<LogCommandHandler<ServiceLog>>();
    }
}
