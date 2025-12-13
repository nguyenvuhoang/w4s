using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Sample.Services;
using O24OpenAPI.Sample.Services.Interfaces;
using O24OpenAPI.Web.Framework.Domain.Logging;

namespace O24OpenAPI.O24AI.Infrastructure;

public class O24OpenAPIStartup : IO24OpenAPIStartup
{
    public int Order => 2000;

    public void Configure(IApplicationBuilder application) { }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var qdrantSetting = configuration["QdrantSetting"];
        services.AddScoped<ILogService<HttpLog>, HttpLogService>();
        services.AddHttpClient("qdrant", c =>
        {
            c.BaseAddress = new Uri(qdrantSetting);
            c.Timeout = TimeSpan.FromSeconds(30);
        });
    }
}
