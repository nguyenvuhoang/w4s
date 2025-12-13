using Newtonsoft.Json.Linq;
using O24OpenAPI.Core;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.WFO.Models;

public class PageListModel : PagedModel
{
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
    public List<JObject> Items { get; set; } = new List<JObject>();

    /// <summary>
    /// Initializes a new instance of the <see cref="PageListModel"/> class
    /// </summary>
    public PageListModel() { }

    //
    // Parameters:
    //   items:
    /// <summary>
    /// Initializes a new instance of the <see cref="PageListModel"/> class
    /// </summary>
    /// <param name="items">The items</param>
    /// <param name="total">The total</param>
    public PageListModel(IPagedList<JObject> items, int total)
    {
        base.PageIndex = items.PageIndex;
        base.PageSize = items.PageSize;
        TotalCount = total;
        TotalPages =
            items.PageSize != 0
                ? (int)Math.Ceiling((double)(total / items.PageSize))
                : items.TotalPages;
        HasPreviousPage = items.HasPreviousPage;
        HasNextPage = items.HasNextPage;
        Items.AddRange(items);
    }
}
