using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace O24OpenAPI.Core.Infrastructure;

/// <summary>
/// The engine interface
/// </summary>
public interface IEngine
{
    /// <summary>
    /// Configures the services using the specified services
    /// </summary>
    /// <param name="services">The services</param>
    /// <param name="configuration">The configuration</param>
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);

    /// <summary>
    /// Configures the request pipeline using the specified application
    /// </summary>
    /// <param name="application">The application</param>
    void ConfigureRequestPipeline(IApplicationBuilder application);

    /// <summary>
    /// Resolves the scope
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="scope">The scope</param>
    /// <returns>The</returns>
    T? Resolve<T>(IServiceScope? scope = null);

    /// <summary>
    /// Resolves the type
    /// </summary>
    /// <param name="type">The type</param>
    /// <param name="scope">The scope</param>
    /// <returns>The object</returns>
    object? Resolve(Type type, IServiceScope? scope = null);

    /// <summary>
    /// Resolves the all
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <returns>An enumerable of t</returns>
    IEnumerable<T> ResolveAll<T>();

    /// <summary>
    /// Resolves the unregistered using the specified type
    /// </summary>
    /// <param name="type">The type</param>
    /// <returns>The object</returns>
    object ResolveUnregistered(Type type);

    /// <summary>
    /// Resolves the interface unregistered using the specified type
    /// </summary>
    /// <param name="type">The type</param>
    /// <returns>The object</returns>
    object ResolveInterfaceUnregistered(Type type);

    /// <summary>
    /// Creates the scope
    /// </summary>
    /// <returns>The service scope</returns>
    IServiceScope? CreateScope();
    T Resolve<T>(object keyed, IServiceScope? scope = null);
    object ResolveRequired(Type type, object keyed, IServiceScope? scope = null);
    object ResolveTypeInstance(Type type);
    IServiceScope CreateQueueScope(WorkContext workContext);
}
