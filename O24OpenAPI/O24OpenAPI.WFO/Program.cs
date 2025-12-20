using Newtonsoft.Json.Serialization;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.Logging.Interceptors;
using O24OpenAPI.WFO.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

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

builder.Configuration.AddEnvironmentVariables();
builder.Services.ConfigureApplicationServices(builder);
if (!builder.Environment.IsDevelopment())
{

    builder.ConfigureWebHost();
}
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcLoggingInterceptor>();
});
var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
using var scope = app.Services.CreateScope();
AsyncScope.Scope = scope;
app.ConfigureRequestPipeline();

await app.StartEngine();
app.AddQueue();
app.ShowStartupBanner();

await app.RunAsync();
