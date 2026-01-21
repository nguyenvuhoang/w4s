<# 
.SYNOPSIS
    Script to quickly create a new service based on O24OpenAPI.Sample structure

.DESCRIPTION
    This script creates a new service with 3-layer architecture:
    - API (Web API layer)
    - Domain (Domain layer) 
    - Infrastructure (Infrastructure layer)

.PARAMETER ServiceName
    Name of the service to create (e.g., "MyService", "CustomerManagement")

.EXAMPLE
    .\create-service.ps1 -ServiceName "MyService"
    
    Creates O24OpenAPI.MyService folder with full structure:
    - O24OpenAPI.MyService.API
    - O24OpenAPI.MyService.Domain  
    - O24OpenAPI.MyService.Infrastructure
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$ServiceName
)

$BaseNamespace = "O24OpenAPI"
$ProjectRoot = $PSScriptRoot
$ServiceFolder = "$ProjectRoot\$BaseNamespace.$ServiceName"

Write-Host "[->] Creating service: $BaseNamespace.$ServiceName" -ForegroundColor Cyan

# Check if folder already exists
if (Test-Path $ServiceFolder) {
    Write-Host "[!] Folder $ServiceFolder already exists!" -ForegroundColor Yellow
    $confirm = Read-Host "Do you want to delete and recreate? (y/n)"
    if ($confirm -ne "y") {
        Write-Host "[!] Service creation cancelled." -ForegroundColor Yellow
        exit 1
    }
    Remove-Item -Recurse -Force $ServiceFolder
}

# Create main folder
New-Item -ItemType Directory -Path $ServiceFolder -Force | Out-Null
Write-Host "[OK] Created root folder: $ServiceFolder" -ForegroundColor Green

#region ===== DOMAIN PROJECT =====
$DomainPath = "$ServiceFolder\$BaseNamespace.$ServiceName.Domain"
New-Item -ItemType Directory -Path $DomainPath -Force | Out-Null
New-Item -ItemType Directory -Path "$DomainPath\AggregatesModel\${ServiceName}Aggregate" -Force | Out-Null

# Domain.csproj
@"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\..\O24Framework\O24OpenAPI.Core\O24OpenAPI.Core.csproj" />
  </ItemGroup>
</Project>
"@ | Set-Content -Path "$DomainPath\$BaseNamespace.$ServiceName.Domain.csproj" -Encoding UTF8

# Domain Entity
@"
using O24OpenAPI.Core.Domain;

namespace $BaseNamespace.$ServiceName.Domain.AggregatesModel.${ServiceName}Aggregate;

public partial class ${ServiceName}Domain : BaseEntity { }
"@ | Set-Content -Path "$DomainPath\AggregatesModel\${ServiceName}Aggregate\${ServiceName}Domain.cs" -Encoding UTF8

# Repository Interface
@"
using O24OpenAPI.Core.SeedWork;

namespace $BaseNamespace.$ServiceName.Domain.AggregatesModel.${ServiceName}Aggregate;

public interface I${ServiceName}Repository : IRepository<${ServiceName}Domain>
{
    public void Add(${ServiceName}Domain entity);
}
"@ | Set-Content -Path "$DomainPath\AggregatesModel\${ServiceName}Aggregate\I${ServiceName}Repository.cs" -Encoding UTF8

Write-Host "[OK] Created Domain project" -ForegroundColor Green
#endregion

#region ===== INFRASTRUCTURE PROJECT =====
$InfraPath = "$ServiceFolder\$BaseNamespace.$ServiceName.Infrastructure"
New-Item -ItemType Directory -Path $InfraPath -Force | Out-Null
New-Item -ItemType Directory -Path "$InfraPath\Configurations" -Force | Out-Null
New-Item -ItemType Directory -Path "$InfraPath\EntityConfigurations" -Force | Out-Null
New-Item -ItemType Directory -Path "$InfraPath\Migrations" -Force | Out-Null
New-Item -ItemType Directory -Path "$InfraPath\Repositories" -Force | Out-Null
New-Item -ItemType Directory -Path "$InfraPath\Persistances" -Force | Out-Null

# Infrastructure.csproj
@"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\..\O24Framework\O24OpenAPI.Logging\O24OpenAPI.Logging.csproj" />
    <ProjectReference Include="..\..\..\O24Framework\O24OpenAPI.Core\O24OpenAPI.Core.csproj" />
    <ProjectReference Include="..\..\..\O24Framework\O24OpenAPI.Data\O24OpenAPI.Data.csproj" />
    <ProjectReference Include="..\..\..\O24Framework\O24OpenAPI.Client\O24OpenAPI.Client.csproj" />
    <ProjectReference Include="..\..\..\O24Framework\O24OpenAPI.Framework\O24OpenAPI.Framework.csproj" />
    <ProjectReference Include="..\$BaseNamespace.$ServiceName.Domain\$BaseNamespace.$ServiceName.Domain.csproj" />
  </ItemGroup>
  <ItemGroup>
    <Folder Include="Persistances\" />
  </ItemGroup>
</Project>
"@ | Set-Content -Path "$InfraPath\$BaseNamespace.$ServiceName.Infrastructure.csproj" -Encoding UTF8

# InfrastructureExtensions.cs
@"
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.Logging.Interceptors;

namespace $BaseNamespace.$ServiceName.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<GrpcServerInboundInterceptor>();
        });
        services.AddLinKitDependency();
        return services;
    }

    public static async Task ConfigureInfrastructure(this IApplicationBuilder app)
    {
        app.ConfigureRequestPipeline();
        await app.StartEngine();
    }
}
"@ | Set-Content -Path "$InfraPath\InfrastructureExtensions.cs" -Encoding UTF8

# Config class
@"
using O24OpenAPI.Core.Configuration;

namespace $BaseNamespace.$ServiceName.Infrastructure.Configurations;

public class ${ServiceName}Config : IConfig
{
}
"@ | Set-Content -Path "$InfraPath\Configurations\${ServiceName}Config.cs" -Encoding UTF8

# Entity Configuration
@"
using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using $BaseNamespace.$ServiceName.Domain.AggregatesModel.${ServiceName}Aggregate;

namespace $BaseNamespace.$ServiceName.Infrastructure.EntityConfigurations;

internal class ${ServiceName}Configuration : O24OpenAPIEntityBuilder<${ServiceName}Domain>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        // TODO: Define table structure
        throw new NotImplementedException();
    }
}
"@ | Set-Content -Path "$InfraPath\EntityConfigurations\${ServiceName}Configuration.cs" -Encoding UTF8

# Entity Migration
@"
using FluentMigrator;
using O24OpenAPI.Data.Extensions;
using $BaseNamespace.$ServiceName.Domain.AggregatesModel.${ServiceName}Aggregate;

namespace $BaseNamespace.$ServiceName.Infrastructure.Migrations;

public class EntityMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<${ServiceName}Domain>();
    }
}
"@ | Set-Content -Path "$InfraPath\Migrations\EntityMigration.cs" -Encoding UTF8

# Repository
@"
using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using $BaseNamespace.$ServiceName.Domain.AggregatesModel.${ServiceName}Aggregate;

namespace $BaseNamespace.$ServiceName.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ${ServiceName}Repository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<${ServiceName}Domain>(dataProvider, staticCacheManager), I${ServiceName}Repository
{
    public void Add(${ServiceName}Domain entity)
    {
        // TODO: Implement
    }
}
"@ | Set-Content -Path "$InfraPath\Repositories\${ServiceName}Repository.cs" -Encoding UTF8

Write-Host "[OK] Created Infrastructure project" -ForegroundColor Green
#endregion

#region ===== API PROJECT =====
$ApiPath = "$ServiceFolder\$BaseNamespace.$ServiceName.API"
New-Item -ItemType Directory -Path $ApiPath -Force | Out-Null
New-Item -ItemType Directory -Path "$ApiPath\Properties" -Force | Out-Null
New-Item -ItemType Directory -Path "$ApiPath\StaticConfig" -Force | Out-Null
New-Item -ItemType Directory -Path "$ApiPath\Controllers" -Force | Out-Null
New-Item -ItemType Directory -Path "$ApiPath\Application" -Force | Out-Null
New-Item -ItemType Directory -Path "$ApiPath\Application\BackgroundJobs" -Force | Out-Null
New-Item -ItemType Directory -Path "$ApiPath\Application\Features" -Force | Out-Null
New-Item -ItemType Directory -Path "$ApiPath\Application\Migrations" -Force | Out-Null

# Generate random ports (5xxx for http, 7xxx for https)
$HttpPort = Get-Random -Minimum 5100 -Maximum 5999
$HttpsPort = Get-Random -Minimum 7100 -Maximum 7999

# API.csproj
@"
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <Nullable>disable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="LinKit.Core" />
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference
      Include="..\..\..\O24Framework\O24OpenAPI.Generator\O24OpenAPI.Generator.csproj"
      OutputItemType="Analyzer"
      ReferenceOutputAssembly="false"
    />
    <ProjectReference Include="..\$BaseNamespace.$ServiceName.Infrastructure\$BaseNamespace.$ServiceName.Infrastructure.csproj" />
  </ItemGroup>
  <ItemGroup>
    <Folder Include="Controllers\" />
    <Folder Include="StaticConfig\" />
  </ItemGroup>
</Project>
"@ | Set-Content -Path "$ApiPath\$BaseNamespace.$ServiceName.API.csproj" -Encoding UTF8

# Program.cs
$ServiceNameLower = $ServiceName.ToLower()
@"
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using $BaseNamespace.$ServiceName.API.Application;
using $BaseNamespace.$ServiceName.Infrastructure;

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
"@ | Set-Content -Path "$ApiPath\Program.cs" -Encoding UTF8

# appsettings.json
@"
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
"@ | Set-Content -Path "$ApiPath\appsettings.json" -Encoding UTF8

# appsettings.Development.json
@"
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
"@ | Set-Content -Path "$ApiPath\appsettings.Development.json" -Encoding UTF8

# launchSettings.json
$launchSettings = @"
{
  "`$schema": "https://json.schemastore.org/launchsettings.json",
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "http://localhost:$HttpPort",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "https://localhost:$HttpsPort;http://localhost:$HttpPort",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
"@
$launchSettings | Set-Content -Path "$ApiPath\Properties\launchSettings.json" -Encoding UTF8

# BackgroundJobs.json
@"
{
  "BackgroundJobConfig": {
    "BackgroundJobs": [
      {
        "Name": "HealthCheck",
        "IsActive": true,
        "ScheduleType": "Interval",
        "TimeIntervalSeconds": 300
      }
    ]
  }
}
"@ | Set-Content -Path "$ApiPath\StaticConfig\BackgroundJobs.json" -Encoding UTF8

# ApplicationExtensions.cs
@"
using O24OpenAPI.Framework.Abstractions;

namespace $BaseNamespace.$ServiceName.API.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs();
        services.AddKeyedSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>(
            "$ServiceNameLower"
        );
        return services;
    }
}
"@ | Set-Content -Path "$ApiPath\Application\ApplicationExtensions.cs" -Encoding UTF8

# HealthCheckJob.cs
@"
using LinKit.Core.BackgroundJobs;
using LinKit.Core.Cqrs;

namespace $BaseNamespace.$ServiceName.API.Application.BackgroundJobs;

[BackgroundJob("HealthCheck")]
public class HealthCheckJob : BackgroundJobCommand { }

[CqrsHandler]
public class HealthCheckJobHandler : ICommandHandler<HealthCheckJob>
{
    public async Task<Unit> HandleAsync(
        HealthCheckJob request,
        CancellationToken cancellationToken = default
    )
    {
        // TODO: Implement health check logic
        return Unit.Value;
    }
}
"@ | Set-Content -Path "$ApiPath\Application\BackgroundJobs\HealthCheckJob.cs" -Encoding UTF8

# Sample Feature - Create command
@"
using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using $BaseNamespace.$ServiceName.Domain.AggregatesModel.${ServiceName}Aggregate;

namespace $BaseNamespace.$ServiceName.API.Application.Features;

public class Create${ServiceName}Command : BaseTransactionModel, ICommand<Create${ServiceName}Response>
{
    public string? Data { get; set; }
}

public record Create${ServiceName}Response();

[CqrsHandler]
public class Create${ServiceName}CommandHandler(I${ServiceName}Repository repository)
    : ICommandHandler<Create${ServiceName}Command, Create${ServiceName}Response>
{
    [WorkflowStep("Create$ServiceName")]
    public Task<Create${ServiceName}Response> HandleAsync(
        Create${ServiceName}Command request,
        CancellationToken cancellationToken = default
    )
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
}
"@ | Set-Content -Path "$ApiPath\Application\Features\Create$ServiceName.cs" -Encoding UTF8

# Data Migration
@"
using O24OpenAPI.Data.Migrations;

namespace $BaseNamespace.$ServiceName.API.Application.Migrations;

public class Data${ServiceName}Migration : BaseMigration
{
    public override void Up()
    {
        // TODO: Implement data migration
        throw new NotImplementedException();
    }
}
"@ | Set-Content -Path "$ApiPath\Application\Migrations\Data${ServiceName}Migration.cs" -Encoding UTF8

# .http file for testing
@"
@$BaseNamespace.$ServiceName.API_HostAddress = http://localhost:$HttpPort

GET {{$BaseNamespace.$ServiceName.API_HostAddress}}/weatherforecast/
Accept: application/json

###
"@ | Set-Content -Path "$ApiPath\$BaseNamespace.$ServiceName.API.http" -Encoding UTF8

Write-Host "[OK] Created API project" -ForegroundColor Green
#endregion

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  SERVICE CREATED SUCCESSFULLY!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "[->] Service name: $BaseNamespace.$ServiceName" -ForegroundColor Cyan
Write-Host "[->] Path: $ServiceFolder" -ForegroundColor Cyan
Write-Host ""
Write-Host "Folder structure:" -ForegroundColor Yellow
Write-Host "  $BaseNamespace.$ServiceName/" -ForegroundColor White
Write-Host "  +-- $BaseNamespace.$ServiceName.API/" -ForegroundColor White
Write-Host "  |   +-- Application/" -ForegroundColor Gray
Write-Host "  |   |   +-- BackgroundJobs/" -ForegroundColor Gray
Write-Host "  |   |   +-- Features/" -ForegroundColor Gray
Write-Host "  |   |   +-- Migrations/" -ForegroundColor Gray
Write-Host "  |   +-- Controllers/" -ForegroundColor Gray
Write-Host "  |   +-- Properties/" -ForegroundColor Gray
Write-Host "  |   +-- StaticConfig/" -ForegroundColor Gray
Write-Host "  +-- $BaseNamespace.$ServiceName.Domain/" -ForegroundColor White
Write-Host "  |   +-- AggregatesModel/" -ForegroundColor Gray
Write-Host "  |       +-- ${ServiceName}Aggregate/" -ForegroundColor Gray
Write-Host "  +-- $BaseNamespace.$ServiceName.Infrastructure/" -ForegroundColor White
Write-Host "      +-- Configurations/" -ForegroundColor Gray
Write-Host "      +-- EntityConfigurations/" -ForegroundColor Gray
Write-Host "      +-- Migrations/" -ForegroundColor Gray
Write-Host "      +-- Persistances/" -ForegroundColor Gray
Write-Host "      +-- Repositories/" -ForegroundColor Gray
Write-Host ""
Write-Host "Port: HTTP=$HttpPort, HTTPS=$HttpsPort" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "  1. Add projects to solution:" -ForegroundColor White
Write-Host "     dotnet sln add `"$ServiceFolder\$BaseNamespace.$ServiceName.API\$BaseNamespace.$ServiceName.API.csproj`"" -ForegroundColor Gray
Write-Host "     dotnet sln add `"$ServiceFolder\$BaseNamespace.$ServiceName.Domain\$BaseNamespace.$ServiceName.Domain.csproj`"" -ForegroundColor Gray
Write-Host "     dotnet sln add `"$ServiceFolder\$BaseNamespace.$ServiceName.Infrastructure\$BaseNamespace.$ServiceName.Infrastructure.csproj`"" -ForegroundColor Gray
Write-Host ""
Write-Host "  2. Build project:" -ForegroundColor White
Write-Host "     cd `"$ServiceFolder\$BaseNamespace.$ServiceName.API`"" -ForegroundColor Gray
Write-Host "     dotnet build" -ForegroundColor Gray
Write-Host ""
