using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.Logger.API.Application.Extensions;
using O24OpenAPI.Logger.Infrastructure.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.ConfigureApplicationServices(builder);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
if (!builder.Environment.IsDevelopment())
{
    builder.ConfigureWebHost();
}
builder.Services.AddWFOInfrastructureServices().AddApplicationServices();

WebApplication app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
using IServiceScope scope = app.Services.CreateScope();
AsyncScope.Scope = scope;
app.ConfigureRequestPipeline();

await app.StartEngine();
app.ShowStartupBanner();
await app.RunAsync();
