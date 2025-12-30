using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.Report.API.GrpcServices;
using O24OpenAPI.Report.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.ConfigureApplicationServices(builder);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    var corsSettings = builder.Configuration.GetSection("Cors");
    options.AddPolicy(
        "AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});
if (!builder.Environment.IsDevelopment())
{
    builder.ConfigureWebHost();
}
builder.Services.AddInfrastructureServices();
var app = builder.Build();
app.UseRouting();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapGrpcService<RPTGrpcService>();
app.MapControllers();

app.UseStaticFiles();
using var scope = app.Services.CreateScope();
AsyncScope.Scope = scope;
app.ConfigureRequestPipeline();

await app.StartEngine();

Stimulsoft.Base.StiLicense.Key =
    "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHl2MpceTPreDxoefztMOVfid+rYzdXcHHGQh/5LK0TW+xW3fWdebQNmadIrTMLw5RRdoUkeng3+eeHjjZSeD4MsSa0WViH99Y0srhA6RH0hoYhlDQj8Dbj+2F6R7iBjtedlqHaqvV+ItmlXB6LiaLYDtLSoiWQZrr6zoNfozfTtjDguGYEicDNPhVfaz4NNDFrXTEFh1AuCAVKw8NMqeU4BE8FHchUHQiEcBedM2skmOvo9iEAWPmN+BHoiIaf4xqIPXtijZR/mg+zbl/3bMcMCpi096H7FwvbZxaYVp9scnfvF/cLRb5LphNp1NEspIkzI5K1qBoxhKZoI0AVqKgElNWDxGQoCIKrwwIogzlLTzlBNzwVN8KGI9J2DrTLyZpabKWho79QfrWwCK0RqcXihUDs00ujpfytqFi7EDq2Is5UYZXgCFYoHQyVDJfjkxTk7rxG/bfRq0RAFyIEhf/w4SDSjrDHAsxte40LpMaYmVjVibe2nNsgof1bxMArFZr6tm2zGYYaQIMeX869IfXgGXoXBU9RPwNe9Y8GnNk7BoSwS/s/1qZ9H3pwh2iMlYCXkIHPwWrJ7BRAjb0K6M64X6YRGczMLRNAi0svu7J9MqmwdsdDtVPlGYh/x6xZZ+Fc=";
app.ShowStartupBanner();
await app.RunAsync();
