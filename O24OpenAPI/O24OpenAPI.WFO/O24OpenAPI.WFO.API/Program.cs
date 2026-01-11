using Newtonsoft.Json.Serialization;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.GrpcContracts.Interceptors;
using O24OpenAPI.WFO.API.Application.Extensions;
using O24OpenAPI.WFO.API.GrpcServices;
using O24OpenAPI.WFO.Infrastructure.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder
    .Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy(),
        };
    });

//builder.Configuration.AddEnvironmentVariables();
builder.Services.ConfigureApplicationServices(builder);
if (!builder.Environment.IsDevelopment())
{
    builder.ConfigureWebHost();
}
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcServerInboundInterceptor>();
});
builder.Services.AddApplicationServices().AddWFOInfrastructureServices();
WebApplication app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
using IServiceScope scope = app.Services.CreateScope();
AsyncScope.Scope = scope;
app.ConfigureRequestPipeline();

await app.StartEngine();
app.UseWFOInfrastructure();
app.ShowStartupBanner();
app.MapControllers();
app.MapGrpcService<WFOGrpcService>();
app.MapGeneratedEndpoints();

await app.RunAsync();
