using O24OpenAPI.Core.Logging.Extensions;
using O24OpenAPI.Web.CMS.Services.Services;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Infrastructure.Extensions;
using O24OpenAPI.Web.Framework.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureApplicationServices(builder);

builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.ConfigureWebHost();
builder.AddO24Logging();
builder.AddBackgroundJobs("StaticConfig/BackgroundJobsConfig.json");
var app = builder.Build();
app.MapHub<SignalHubService>("/signal");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();

// app.UseMiddleware<HttpsLoggingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
using var scope = app.Services.CreateScope();
AsyncScope.Scope = scope;
app.ConfigureRequestPipeline();

await app.StartEngine();
app.ShowStartupBanner();
await app.RunAsync();
