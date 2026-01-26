using O24OpenAPI.AI.API.Application;
using O24OpenAPI.AI.API.Endpoints;
using O24OpenAPI.AI.Infrastructure;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.GrpcContracts.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplicationServices(builder);
builder.Services.AddApplicationServices(builder);
builder.Services.AddInfrastructureServices();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddGrpcContracts();
if (!builder.Environment.IsDevelopment())
{
    builder.ConfigureWebHost();
}
WebApplication app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapChatEndpoints();
app.MapControllers();

using IServiceScope scope = app.Services.CreateScope();
AsyncScope.Scope = scope;

await app.ConfigureInfrastructure();
app.ShowStartupBanner();

app.Run();
