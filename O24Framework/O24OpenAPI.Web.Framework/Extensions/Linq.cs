namespace O24OpenAPI.Web.Framework.Extensions;

/// <summary>
/// The async enumerable extensions class
/// </summary>
public static class AsyncIEnumerableExtensions
{
    /// <summary>
    /// Returns the list using the specified source
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <param name="source">The source</param>
    /// <returns>A task containing a list of t source</returns>
    public static Task<List<TSource>> ToListAsync<TSource>(this IEnumerable<TSource> source) =>
        source.ToAsyncEnumerable().ToListAsync().AsTask();
}
