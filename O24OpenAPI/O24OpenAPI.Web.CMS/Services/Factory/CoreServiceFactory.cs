using System.Collections.Concurrent;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Web.CMS.Models.O9;

namespace O24OpenAPI.Web.CMS.Services.Factory;

public class CoreServiceFactory(IServiceProvider serviceProvider) : ICoreServiceFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private static readonly ConcurrentDictionary<Type, Type> _typeCache = new();

    public T CreateService<T>()
        where T : class
    {
        var implementationType =
            _typeCache.GetOrAdd(typeof(T), serviceType => ResolveImplementationType(serviceType))
            ?? throw new ArgumentException($"Could not resolve implementation for {typeof(T)}");
        return _serviceProvider.GetRequiredService(implementationType) as T;
    }

    private Type ResolveImplementationType(Type serviceType)
    {
        var coreMode = Singleton<AppSettings>.Instance.Get<CommonConfig>().CoreMode;
        var serviceName = serviceType.Name.Replace("ICore", "");

        var assemblyName = typeof(CoreServiceFactory).Assembly.FullName;

        var implementationType =
            coreMode switch
            {
                "Optimal9" => Type.GetType(
                    $"O24OpenAPI.Web.CMS.Services.O9Service.O9{serviceName}, {assemblyName}",
                    true,
                    true
                ),
                "Neptune" => Type.GetType(
                    $"O24OpenAPI.Web.CMS.Services.NeptuneService.Neptune{serviceName}, {assemblyName}",
                    true,
                    true
                ),
                _ => throw new ArgumentException($"Invalid CoreMode: {coreMode}"),
            }
            ?? throw new ArgumentException(
                $"Service implementation not found for {serviceType.FullName}"
            );
        return implementationType;
    }
}
