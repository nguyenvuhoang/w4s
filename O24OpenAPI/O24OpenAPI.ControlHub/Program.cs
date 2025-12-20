using Newtonsoft.Json.Serialization;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// builder.Configuration.AddJsonFile(O24OpenAPIConfigurationDefaults.AppSettingsFilePath, true, true);

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
var environment =
    builder.Configuration.GetValue<string>("EnvConfig")?.ToEnum<EnvironmentType>()
    ?? EnvironmentType.Dev;

builder.Configuration.AddEnvironmentVariables();
builder.Services.ConfigureApplicationServices(builder);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.ConfigureWebHost();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
using var scope = app.Services.CreateScope();
AsyncScope.Scope = scope;
app.ConfigureRequestPipeline();

await app.StartEngine();
app.ShowStartupBanner();

await app.RunAsync();
