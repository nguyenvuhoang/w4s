using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.Framework.Middlewares;
using O24OpenAPI.Logging.Extensions;
using O24OpenAPI.Web.CMS.Services.Services;

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
