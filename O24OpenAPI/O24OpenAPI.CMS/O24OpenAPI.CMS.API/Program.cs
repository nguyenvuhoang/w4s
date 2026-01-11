using O24OpenAPI.CMS.API.Application.GrpcServices;
using O24OpenAPI.CMS.API.Application.Services.Services;
using O24OpenAPI.CMS.Infrastructure.Extensions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.Framework.Middlewares;
using O24OpenAPI.Logging.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureApplicationServices(builder);

builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
if (!builder.Environment.IsDevelopment())
{
    builder.ConfigureWebHost();
}
builder.AddO24Logging();

//builder.AddBackgroundJobs("StaticConfig/BackgroundJobsConfig.json");
builder.Services.AddInfrastructureServices();
WebApplication app = builder.Build();
app.MapHub<SignalHubService>("/signal");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();

// app.UseMiddleware<HttpsLoggingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
using IServiceScope scope = app.Services.CreateScope();
AsyncScope.Scope = scope;
app.ConfigureRequestPipeline();
app.MapGrpcService<CMSGrpcServer>();

await app.StartEngine();
app.ShowStartupBanner();
app.MapGeneratedEndpoints();
await app.RunAsync();
