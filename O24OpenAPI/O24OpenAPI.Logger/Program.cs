using MediatR;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Infrastructure.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.ConfigureApplicationServices(builder);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.ConfigureWebHost();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly())
);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
using var scope = app.Services.CreateScope();
AsyncScope.Scope = scope;
app.ConfigureRequestPipeline();

await app.StartEngine();
app.ShowStartupBanner();
await app.RunAsync();
