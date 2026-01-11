using O24OpenAPI.Core;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;

namespace O24OpenAPI.Framework.Models;

/// <summary>
/// The paged list model class
/// </summary>
/// <seealso cref="PagedModel"/>
public class PagedListModel<TEntity, T> : PagedModel
    where T : BaseO24OpenAPIModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedListModel{TEntity,T}"/> class
    /// </summary>
    public PagedListModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedListModel{TEntity,T}"/> class
    /// </summary>
    /// <param name="items">The items</param>
    public PagedListModel(IPagedList<TEntity> items)
    {
        PageIndex = items.PageIndex;
        PageSize = items.PageSize;
        TotalCount = items.TotalCount;
        TotalPages = items.TotalPages;
        HasPreviousPage = items.HasPreviousPage;
        HasNextPage = items.HasNextPage;
        Items.AddRange(items.Select(p => p.ToModel<T>()));
        TotalSuccess = items.TotalSuccess;
        TotalFailed = items.TotalFailed;
    }

    /// <summary>
    /// Gets the value of the total count
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Gets the value of the total pages
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Gets the value of the has previous page
    /// </summary>
    public bool HasPreviousPage { get; }

    /// <summary>
    /// Gets the value of the has next page
    /// </summary>
    public bool HasNextPage { get; }

    /// <summary>
    /// Gets or sets the value of the items
    /// </summary>
    public List<T> Items { get; set; } = new List<T>();
    public int? TotalSuccess { get; set; } = 0;
    public int? TotalFailed { get; set; } = 0;
}

/// <summary>
/// The paged list model class
/// </summary>
/// <seealso cref="PagedModel"/>
public class PagedListModel<T> : PagedModel
{
    public int TotalCount { get; set; } = 0;
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
    public List<T> Items { get; set; } = [];
    public int? TotalSuccess { get; set; } = 0;
    public int? TotalFailed { get; set; } = 0;

    public PagedListModel() { }

    public PagedListModel(IPagedList<T> items)
    {
        PageIndex = items.PageIndex;
        PageSize = items.PageSize;
        TotalCount = items.TotalCount;
        TotalPages = items.TotalPages;
        HasPreviousPage = items.HasPreviousPage;
        HasNextPage = items.HasNextPage;
        Items = [.. items];
        TotalSuccess = items.TotalSuccess;
        TotalFailed = items.TotalFailed;
    }
}
