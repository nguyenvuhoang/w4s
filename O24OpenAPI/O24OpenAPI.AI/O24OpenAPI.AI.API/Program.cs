using O24OpenAPI.AI.Infrastructure;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Web.Framework.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
AsyncScope.Scope = scope;

await app.ConfigureInfrastructure();
app.ShowStartupBanner();

app.Run();
