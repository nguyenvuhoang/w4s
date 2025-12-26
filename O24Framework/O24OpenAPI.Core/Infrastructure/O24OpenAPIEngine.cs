using System.Collections.Concurrent;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Core.Infrastructure.Mapper;

namespace O24OpenAPI.Core.Infrastructure;

/// <summary>
/// The 24 open api engine class
/// </summary>
/// <seealso cref="IEngine"/>
public class O24OpenAPIEngine : IEngine
{
    /// <summary>
    /// Gets the service provider using the specified scope
    /// </summary>
    /// <param name="scope">The scope</param>
    /// <returns>The service provider</returns>
    protected IServiceProvider GetServiceProvider(IServiceScope scope = null)
    {
        if (scope != null)
        {
            return scope.ServiceProvider;
        }

        //IServiceProvider serviceProvider = this.ServiceProvider ?? throw new InvalidOperationException("⚠️ ServiceProvider is null. It seems the DI container has not been initialized properly or has not been registered.");
        IServiceProvider serviceProvider = this.ServiceProvider;
        var httpContextAccessor = serviceProvider?.GetService<IHttpContextAccessor>();
        var httpContext = httpContextAccessor?.HttpContext;
        if (httpContext?.RequestServices != null)
        {
            return httpContext.RequestServices;
        }

        if (AsyncScope.Scope?.ServiceProvider != null)
        {
            return AsyncScope.Scope.ServiceProvider;
        }

        if (ServiceScopeFactory is not null)
        {
            return ServiceScopeFactory.CreateScope().ServiceProvider;
        }
        if (serviceProvider is not null)
        {
            return serviceProvider.CreateScope().ServiceProvider;
        }

        return null;
    }

    /// <summary>
    /// Runs the startup tasks
    /// </summary>
    protected virtual void RunStartupTasks() { }

    /// <summary>
    /// Gets or sets the value of the service provider
    /// </summary>
    public virtual IServiceProvider? ServiceProvider { get; protected set; }

    /// <summary>
    /// Gets or sets the value of the service scope factory
    /// </summary>
    public static IServiceScopeFactory? ServiceScopeFactory { get; protected set; }

    /// <summary>
    /// Configures the request pipeline using the specified application
    /// </summary>
    /// <param name="application">The application</param>
    public void ConfigureRequestPipeline(IApplicationBuilder application)
    {
        this.ServiceProvider = application.ApplicationServices;
        ServiceScopeFactory =
            application.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
        foreach (
            IO24OpenAPIStartup o24OpenAPIStartup in (IEnumerable<IO24OpenAPIStartup>)
                this.Resolve<ITypeFinder>(null)
                    .FindClassesOfType<IO24OpenAPIStartup>()
                    .Select<Type, IO24OpenAPIStartup>(startup =>
                        (IO24OpenAPIStartup)Activator.CreateInstance(startup)
                    )
                    .OrderBy<IO24OpenAPIStartup, int>(startup => startup.Order)
        )
        {
            o24OpenAPIStartup.Configure(application);
        }
    }

    /// <summary>
    /// Adds the auto mapper
    /// </summary>
    protected virtual void AddAutoMapper()
    {
        IOrderedEnumerable<IOrderedMapperProfile> instances = Singleton<ITypeFinder>
            .Instance.FindClassesOfType<IOrderedMapperProfile>()
            .Select(mapperConfiguration =>
                (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration)
            )
            .OrderBy<IOrderedMapperProfile, int>(mapperConfiguration => mapperConfiguration.Order);
        AutoMapperConfiguration.Init(
            new MapperConfiguration(cfg =>
            {
                foreach (
                    IOrderedMapperProfile orderedMapperProfile in (IEnumerable<IOrderedMapperProfile>)
                        instances
                )
                {
                    cfg.AddProfile(orderedMapperProfile.GetType());
                }
            })
        );
    }

    /// <summary>
    /// Currents the domain assembly resolve using the specified sender
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="args">The args</param>
    /// <returns>The assembly</returns>
    private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        Assembly assembly = AppDomain
            .CurrentDomain.GetAssemblies()
            .FirstOrDefault<Assembly>(a => a.FullName == args.Name);
        if (assembly != null)
        {
            return assembly;
        }

        ITypeFinder instance = Singleton<ITypeFinder>.Instance;
        return instance != null
            ? instance.GetAssemblies().FirstOrDefault<Assembly>(a => a.FullName == args.Name)
            : null;
    }

    /// <summary>
    /// Configures the services using the specified services
    /// </summary>
    /// <param name="services">The services</param>
    /// <param name="configuration">The configuration</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEngine>(this);
        var listStartUp = Singleton<ITypeFinder>
            .Instance.FindClassesOfType<IO24OpenAPIStartup>()
            .Select(startup => (IO24OpenAPIStartup)Activator.CreateInstance(startup))
            .OrderBy(startup => startup.Order)
            .ToList();
        foreach (var o24OpenAPIStartup in listStartUp)
        {
            try
            {
                o24OpenAPIStartup.ConfigureServices(services, configuration);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error in {o24OpenAPIStartup.GetType().Name}.ConfigureServices: {ex.Message}"
                );
                throw;
            }
            services.AddSingleton(services);
            this.AddAutoMapper();
            this.RunStartupTasks();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(
                this.CurrentDomain_AssemblyResolve
            );
        }
    }

    /// <summary>
    /// Creates the scope
    /// </summary>
    /// <returns>The service scope</returns>
    public IServiceScope CreateScope()
    {
        var workContext = EngineContext.Current.Resolve<WorkContext>();
        var scope = ServiceScopeFactory.CreateScope();
        var newWorkContext = scope.ServiceProvider.GetRequiredService<WorkContext>();
        newWorkContext.SetWorkContext(workContext);
        AsyncScope.Scope = scope;
        AsyncScope.WorkContext = newWorkContext;
        return scope;
    }

    /// <summary>
    /// Resolves the scope
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="scope">The scope</param>
    /// <returns>The</returns>
    public T Resolve<T>(IServiceScope scope = null)
    {
        return (T)this.Resolve(typeof(T), scope);
    }

    /// <summary>
    /// Resolves the type
    /// </summary>
    /// <param name="type">The type</param>
    /// <param name="scope">The scope</param>
    /// <returns>The object</returns>
    public object Resolve(Type type, IServiceScope scope = null)
    {
        return this.GetServiceProvider(scope)?.GetService(type);
    }

    public T Resolve<T>(object keyed, IServiceScope scope = null)
    {
        var serviceProvider =
            GetServiceProvider(scope)
            ?? throw new InvalidOperationException("Service provider is null");
        return keyed is null || string.IsNullOrWhiteSpace(keyed.ToString())
            ? serviceProvider.GetService<T>()
            : serviceProvider.GetKeyedService<T>(keyed);
    }

    public object ResolveRequired(Type type, object keyed, IServiceScope scope = null)
    {
        var serviceProvider =
            GetServiceProvider(scope)
            ?? throw new InvalidOperationException("Service provider is null");
        return serviceProvider.GetRequiredKeyedService(type, keyed);
    }

    /// <summary>
    /// Resolves the all
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <returns>An enumerable of t</returns>
    public IEnumerable<T> ResolveAll<T>()
    {
        return (IEnumerable<T>)this.GetServiceProvider().GetServices(typeof(T));
    }

    // /// <summary>
    // /// Resolves the unregistered using the specified type
    // /// </summary>
    // /// <param name="type">The type</param>
    // /// <exception cref="NotImplementedException"></exception>
    // /// <returns>The object</returns>
    // public virtual object ResolveUnregistered(Type type)
    // {
    //     Exception innerException = (Exception)null;
    //     foreach (ConstructorInfo constructor in type.GetConstructors())
    //     {
    //         try
    //         {
    //             IEnumerable<object> source = (
    //                 (IEnumerable<ParameterInfo>)constructor.GetParameters()
    //             ).Select<ParameterInfo, object>(
    //                 (Func<ParameterInfo, object>)(
    //                     parameter =>
    //                     {
    //                         return this.Resolve(parameter.ParameterType, (IServiceScope)null)
    //                             ?? throw new O24OpenAPIException("Unknown dependency");
    //                     }
    //                 )
    //             );
    //             return Activator.CreateInstance(type, [.. source]);
    //         }
    //         catch (Exception ex)
    //         {
    //             innerException = ex;
    //         }
    //     }
    //     throw new O24OpenAPIException(
    //         "No constructor was found for " + type.FullName + ".",
    //         innerException
    //     );
    // }

    /// <summary>
    /// Resolves the unregistered using the specified type
    /// </summary>
    /// <param name="type">The type</param>
    /// <exception cref="InvalidOperationException">Cannot resolve dependency: {param.ParameterType.FullName}</exception>
    /// <exception cref="O24OpenAPIException">No constructor found for {type.FullName}. Errors: {string.Join("; ", exceptions.Select(e =&gt; e.Message))} </exception>
    /// <returns>The object</returns>
    public virtual object ResolveUnregistered(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        List<Exception> exceptions = [];
        IServiceProvider serviceProvider = GetServiceProvider();

        foreach (
            ConstructorInfo constructor in type.GetConstructors()
                .OrderBy(c => c.GetParameters().Length)
        )
        {
            try
            {
                // Resolve tất cả parameter của constructor
                object[] parameters = constructor
                    .GetParameters()
                    .Select(param =>
                        serviceProvider?.GetService(param.ParameterType)
                        ?? Resolve(param.ParameterType, null)
                        ?? throw new InvalidOperationException(
                            $"Cannot resolve dependency: {param.ParameterType.FullName}"
                        )
                    )
                    .ToArray();

                return Activator.CreateInstance(type, parameters);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        throw new O24OpenAPIException(
            $"No constructor found for {type.FullName}. Errors: {string.Join("; ", exceptions.Select(e => e.Message))}",
            exceptions.FirstOrDefault()
        );
    }

    private static readonly ConcurrentDictionary<
        Type,
        Func<IServiceProvider, object>
    > _unregisteredFactoryCache = new();

    public virtual object ResolveTypeInstance(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var factory = _unregisteredFactoryCache.GetOrAdd(type, CreateFactory);

        try
        {
            return factory(GetServiceProvider());
        }
        catch (Exception ex)
        {
            throw new O24OpenAPIException($"Failed to resolve type: {type.FullName}", ex);
        }
    }

    private Func<IServiceProvider, object> CreateFactory(Type type)
    {
        if (type.IsAbstract || type.IsInterface)
        {
            throw new InvalidOperationException(
                $"Cannot create instance of abstract/interface: {type.FullName}"
            );
        }

        var defaultCtor = type.GetConstructor(Type.EmptyTypes);
        if (defaultCtor != null)
        {
            return _ => Activator.CreateInstance(type)!;
        }

        var constructors = type.GetConstructors();
        var constructor = constructors
            .OrderByDescending(c =>
                c.IsDefined(typeof(ActivatorUtilitiesConstructorAttribute), true) ? 1 : 0
            )
            .ThenByDescending(c => c.GetParameters().Length)
            .FirstOrDefault();

        if (constructor == null)
        {
            throw new InvalidOperationException($"No public constructor found for {type.FullName}");
        }

        var parameters = constructor.GetParameters();

        return serviceProvider =>
        {
            var args = parameters
                .Select(p =>
                {
                    var service = serviceProvider?.GetService(p.ParameterType);
                    if (service != null)
                    {
                        return service;
                    }

                    return Resolve(p.ParameterType, null)
                        ?? throw new InvalidOperationException(
                            $"Cannot resolve dependency: {p.ParameterType.FullName}"
                        );
                })
                .ToArray();

            return constructor.Invoke(args);
        };
    }

    /// <summary>
    /// Resolves the interface unregistered using the specified type
    /// </summary>
    /// <param name="type">The type</param>
    /// <exception cref="O24OpenAPIException"></exception>
    /// <exception cref="O24OpenAPIException"></exception>
    /// <exception cref="O24OpenAPIException">Unknown dependency</exception>
    /// <returns>The object</returns>
    public virtual object ResolveInterfaceUnregistered(Type type)
    {
        Exception innerException = null;
        var implementingClasses = type
            .Assembly.GetExportedTypes()
            .FirstOrDefault(t => type.IsAssignableFrom(t) && t.IsClass);
        if (implementingClasses == null)
        {
            throw new O24OpenAPIException(
                "No constructor was found for " + type.FullName + ".",
                innerException
            );
        }

        foreach (ConstructorInfo constructor in implementingClasses.GetConstructors())
        {
            try
            {
                IEnumerable<object> source = constructor
                    .GetParameters()
                    .Select<ParameterInfo, object>(parameter =>
                    {
                        return this.Resolve(parameter.ParameterType, null)
                            ?? throw new O24OpenAPIException("Unknown dependency");
                    });
                return Activator.CreateInstance(implementingClasses, source.ToArray<object>());
            }
            catch (Exception ex)
            {
                innerException = ex;
            }
        }

        throw new O24OpenAPIException(
            "No constructor was found for " + type.FullName + ".",
            innerException
        );
    }

    public IServiceScope CreateQueueScope(WorkContext workContext)
    {
        var scope = ServiceScopeFactory.CreateScope();
        var newWorkContext = scope.ServiceProvider.GetRequiredService<WorkContext>();
        newWorkContext.SetWorkContext(workContext);
        AsyncScope.Scope = scope;
        AsyncScope.WorkContext = newWorkContext;
        return scope;
    }
}
