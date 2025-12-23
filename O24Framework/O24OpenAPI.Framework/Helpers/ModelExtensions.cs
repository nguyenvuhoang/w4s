using O24OpenAPI.Core;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.Framework.Helpers;

/// <summary>
/// The model extensions class
/// </summary>
public static class ModelExtensions
{
    /// <summary>
    /// Returns the paged list model using the specified items
    /// </summary>
    /// <typeparam name="TEntity">The entity</typeparam>
    /// <typeparam name="T">The </typeparam>
    /// <param name="items">The items</param>
    /// <returns>A paged list model of t entity and t</returns>
    public static PagedListModel<TEntity, T> ToPagedListModel<TEntity, T>(
        this IPagedList<TEntity> items
    )
        where T : BaseO24OpenAPIModel
    {
        return new PagedListModel<TEntity, T>(items);
    }
}
