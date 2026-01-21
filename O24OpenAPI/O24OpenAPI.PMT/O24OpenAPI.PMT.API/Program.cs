using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.PMT.API.Application;
using O24OpenAPI.PMT.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

using IServiceScope scope = app.Services.CreateScope();
AsyncScope.Scope = scope;

await app.ConfigureInfrastructure();
app.ShowStartupBanner();

app.Run();
