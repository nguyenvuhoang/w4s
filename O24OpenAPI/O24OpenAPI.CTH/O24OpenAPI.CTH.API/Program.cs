using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.CTH.API.Application.Extensions;
using O24OpenAPI.CTH.API.Application.GrpcServices;
using O24OpenAPI.CTH.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.Framework.Middlewares;
using O24OpenAPI.GrpcContracts.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureApplicationServices(builder);
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
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
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<WorkContextPropagationMiddleware>();

using IServiceScope scope = app.Services.CreateScope();
AsyncScope.Scope = scope;

await app.ConfigureInfrastructure();
await app.StartEngine();
app.UseResponseWrapperExceptGrpc();
app.ShowStartupBanner();
app.MapControllers();
app.MapGrpcService<CTHGrpcService>();
app.Run();
