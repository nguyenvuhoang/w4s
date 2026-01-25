using O24OpenAPI.CMS.API.Application.GrpcServices;
using O24OpenAPI.CMS.API.Application.Services.Services;
using O24OpenAPI.CMS.Infrastructure.Extensions;
using O24OpenAPI.CMS.Infrastructure.Middlewares;
using O24OpenAPI.CMS.Infrastructure.RateLimiting;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.Framework.Middlewares;
using O24OpenAPI.GrpcContracts.Extensions;
using O24OpenAPI.Logging.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureApplicationServices(builder);
builder.Services.AddGrpcContracts();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiRateLimiting();
if (!builder.Environment.IsDevelopment())
{
    builder.ConfigureWebHost();
}
builder.AddO24Logging();
builder.Configuration.AddJsonFile(
    path: "StaticConfig/ReverseProxyConfig.json",
    optional: false,
    reloadOnChange: true
);

//builder.AddBackgroundJobs("StaticConfig/BackgroundJobsConfig.json");
builder.Services.AddInfrastructureServices();

//builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddYarpWithTransforms(builder.Configuration);
WebApplication app = builder.Build();
app.MapHub<SignalHubService>("/signal");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseApiRateLimiting();
app.UseAuthorization();
app.UseProxyAuthentication();

// app.UseMiddleware<HttpsLoggingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
using IServiceScope scope = app.Services.CreateScope();
AsyncScope.Scope = scope;
app.ConfigureRequestPipeline();
await app.StartEngine();
app.ShowStartupBanner();
app.MapGrpcService<CMSGrpcServer>();
app.MapGeneratedEndpoints();
app.MapControllers().RequireRateLimiting("api-rate-limit");
app.MapReverseProxy();
await app.RunAsync();
