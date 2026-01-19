namespace O24OpenAPI.Core.Infrastructure;

/// <summary>
/// The singleton class
/// </summary>
/// <seealso cref="BaseSingleton"/>
public class Singleton<T> : BaseSingleton
{
    /// <summary>
    /// The instance
    /// </summary>
    private static T? instance;

    /// <summary>
    /// Gets or sets the value of the instance
    /// </summary>
    public static T? Instance
    {
        get => instance;
        set
        {
            instance = value;
            if (value is not null)
                AllSingletons[typeof(T).Name] = value;
        }
    }

    /// <summary>
    /// Creates an instance with a specific name
    /// </summary>
    public static void CreateInstanceByName(string name, object instance)
    {
        AllSingletons[name] = instance;
    }

    /// <summary>
    /// Retrieves an instance by name
    /// </summary>
    public static T? GetInstanceByName(string name)
    {
        return AllSingletons.TryGetValue(name, out var instance) ? (T)instance : default;
    }
}
