using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.GrpcContracts.Extensions;
using O24OpenAPI.NCH.API.Application.Extensions;
using O24OpenAPI.NCH.Infrastructure.Extensions;
using Telegram.Bot.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();


builder.Configuration.AddEnvironmentVariables();
builder.Services.ConfigureApplicationServices(builder);

builder.Services.AddControllers();
builder.Services.ConfigureTelegramBotMvc();

builder.Services.AddEndpointsApiExplorer();
builder.AddBackgroundJobs("StaticConfig/BackgroundJobsConfig.json");
builder.Services.AddInfrastructureServices().AddApplicationServices();
builder.Services.AddGrpcContracts();
builder.Services.AddHttpClient();

if (!builder.Environment.IsDevelopment())
{
    builder.ConfigureWebHost();
}
WebApplication app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
using IServiceScope scope = app.Services.CreateScope();
AsyncScope.Scope = scope;
app.ConfigureRequestPipeline();

await app.StartEngine();
app.ShowStartupBanner();

await app.RunAsync();