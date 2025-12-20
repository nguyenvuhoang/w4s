using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.O24Design.Services;
using O24OpenAPI.O24Design.Services.Interfaces;

namespace O24OpenAPI.O24Design.Infrastructure;

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
