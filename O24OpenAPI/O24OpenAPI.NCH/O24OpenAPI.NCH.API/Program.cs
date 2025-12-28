using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using Telegram.Bot.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();


builder.Configuration.AddEnvironmentVariables();
builder.Services.ConfigureApplicationServices(builder);

builder.Services.AddControllers();
builder.Services.ConfigureTelegramBotMvc();

builder.Services.AddEndpointsApiExplorer();
builder.AddBackgroundJobs("StaticConfig/BackgroundJobsConfig.json");
builder.ConfigureWebHost();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
using var scope = app.Services.CreateScope();
AsyncScope.Scope = scope;
app.ConfigureRequestPipeline();

await app.StartEngine();
app.ShowStartupBanner();

await app.RunAsync();
