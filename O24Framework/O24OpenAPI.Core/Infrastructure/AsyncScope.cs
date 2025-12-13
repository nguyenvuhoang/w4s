using Microsoft.Extensions.DependencyInjection;

namespace O24OpenAPI.Core.Infrastructure;

/// <summary>
/// The async scope class
/// </summary>
public class AsyncScope
{
    /// <summary>
    /// The scope
    /// </summary>
    private static readonly AsyncLocal<IServiceScope> _scope = new();
    private static readonly AsyncLocal<WorkContext> _workContext = new();

    /// <summary>
    /// Gets or sets the value of the scope
    /// </summary>
    public static IServiceScope Scope
    {
        get => _scope.Value;
        set => _scope.Value = value;
    }

    /// <summary>
    /// Gets or sets the value of the work context
    /// </summary>
    public static WorkContext WorkContext
    {
        get => _workContext.Value;
        set => _workContext.Value = value;
    }

    /// <summary>
    /// Gets the value of the service provider
    /// </summary>
    public static IServiceProvider ServiceProvider => Scope.ServiceProvider;

    /// <summary>
    /// Clears
    /// </summary>
    public static void Clear()
    {
        _scope.Value?.Dispose();
        _scope.Value = null;
    }
}
