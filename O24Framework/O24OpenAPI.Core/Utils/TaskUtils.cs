using O24OpenAPI.Core.Infrastructure;

namespace O24OpenAPI.Core.Utils;

/// <summary>
/// The task utils class
/// </summary>
public static class TaskUtils
{
    /// <summary>
    /// Runs the function
    /// </summary>
    /// <param name="function">The function</param>
    public static Task RunAsync(Func<Task> function)
    {
        return Task.Run(async () =>
        {
            using var scope = EngineContext.Current.CreateScope();
            try
            {
                await function();
            }
            finally
            {
                AsyncScope.Clear();
            }
        });
    }

    /// <summary>
    /// Runs the in new scope using the specified function
    /// </summary>
    /// <param name="function">The function</param>
    public static async Task RunInNewScope(Func<Task> function)
    {
        try
        {
            await function();
        }
        finally
        {
            AsyncScope.Clear();
        }
    }
}
