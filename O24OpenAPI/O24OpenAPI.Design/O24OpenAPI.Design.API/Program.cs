using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.GrpcContracts.Extensions;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Configuration.AddEnvironmentVariables();
builder.Services.ConfigureApplicationServices(builder);
builder.Services.AddGrpcContracts();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.ConfigureWebHost();
WebApplication app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
using IServiceScope scope = app.Services.CreateScope();
AsyncScope.Scope = scope;
app.ConfigureRequestPipeline();

await app.StartEngine();
app.ShowStartupBanner();
await app.RunAsync();
