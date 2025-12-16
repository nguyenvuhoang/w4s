using Linh.CodeEngine.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Client.Infisical;
using O24OpenAPI.Contracts.Configuration.Client;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Helper;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Data.Configuration;
using O24OpenAPI.EventBus.Extensions;
using O24OpenAPI.EventBus.Submitters;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.GrpcContracts.Extensions;
using O24OpenAPI.Logging.Abstractions;
using O24OpenAPI.Logging.Extensions;
using StackExchange.Redis;

namespace O24OpenAPI.Framework.Infrastructure.Extensions;

/// <summary>
/// The service collection extensions class
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add services to the application and configure service provider
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="builder">A builder for web applications and services</param>
    public static void ConfigureApplicationServices(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        services.AddScoped<WorkContext>();
        var currentAssembly = AppDomain.CurrentDomain.FriendlyName;
        Console.WriteLine("Getting secret value ...");
        var appSettings = InfisicalManager.GetSecretByKey<JObject>(currentAssembly);
        Console.WriteLine("Getting secret value done");
        CommonHelper.DefaultFileProvider = new O24OpenAPIFileProvider(builder.Environment);
        services.AddHttpContextAccessor();

        WebAppTypeFinder typeFinder = new();
        Singleton<ITypeFinder>.Instance = typeFinder;
        services.AddSingleton<ITypeFinder>(typeFinder);

        List<IConfig> lists = [];
        var listConfigTypes = typeFinder.FindClassesOfType<IConfig>();
        var list = listConfigTypes
            .Select(configType => (IConfig)Activator.CreateInstance(configType))
            .ToList();

        foreach (IConfig instance in list)
        {
            if (appSettings.TryGetValue(instance.Name, out var tokenSetting))
            {
                var jObject = tokenSetting is JObject settingObject
                    ? settingObject
                    : JObject.FromObject(tokenSetting);

                Type configType = instance.GetType();

                var config = (IConfig)jObject.ToObject(configType);

                if (config != null)
                {
                    lists.Add(config);
                }
            }
        }

        AppSettings implementationInstance2 = AppSettingsHelper.SaveAppSettings(
            lists,
            CommonHelper.DefaultFileProvider,
            false
        );
        services.AddSingleton(implementationInstance2);
        Singleton<DataConfig>.Instance = implementationInstance2.Get<DataConfig>();
        EngineContext.Create().ConfigureServices(services, builder.Configuration);
        Singleton<O24OpenAPIClientConfiguration>.Instance = new(
            Singleton<O24OpenAPIConfiguration>.Instance
        );

        services.AddDistributedCache();
        builder.AddO24Logging();
        services.AddScoped<IStaticTokenService, StaticTokenService>();
        services.AddDynamicCodeEngineWithFileSystem(AppContext.BaseDirectory);
        services.AddGrpcContracts();
        services.AddKeyedSingleton<ILogSubmitter, RabbitMqLogSubmitter>("rabbitmq");
        services.AddLinKitCqrs("fw");
        var assembly = AppDomain
            .CurrentDomain.GetAssemblies()
            .FirstOrDefault(a =>
                !a.IsDynamic
                && a.FullName?.StartsWith(
                    Singleton<O24OpenAPIConfiguration>.Instance.AssemblyMigration
                ) == true
            );
        builder.AddRabbitMqEventBus().AddSubscriptionsFromAssemblies(assembly);
        builder.AddBackgroundJobs("StaticConfig/BackgroundJobsConfig.json");
    }

    public static void AddHttpContextAccessor(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    private static void AddDistributedCache(this IServiceCollection services)
    {
        var distributedCacheConfig = Singleton<AppSettings>.Instance.Get<DistributedCacheConfig>();

        if (!distributedCacheConfig.Enabled)
        {
            services.AddMemoryCache();
            return;
        }

        switch ((int)distributedCacheConfig.DistributedCacheType)
        {
            case 0:
                services.AddDistributedMemoryCache();
                break;
            case 1:
                services.AddDistributedSqlServerCache(options =>
                {
                    options.ConnectionString = distributedCacheConfig.ConnectionString;
                    options.SchemaName = distributedCacheConfig.SchemaName;
                    options.TableName = distributedCacheConfig.TableName;
                });
                break;
            case 2:
                services.AddStackExchangeRedisCache(distributedCacheConfig.ConnectionString);
                break;
        }
    }

    private static void AddStackExchangeRedisCache(
        this IServiceCollection services,
        string connectionString
    )
    {
        var multiplexer = ConnectionMultiplexer.Connect(
            new ConfigurationOptions { EndPoints = { connectionString } }
        );

        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
    }

    private static void RegisterInfisical(this IServiceCollection services)
    {
        services.AddScoped<InfisicalClient>();
    }
}
