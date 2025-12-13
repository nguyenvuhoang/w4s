using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using O24OpenAPI.Core;

namespace O24OpenAPI.APIContracts.Models.NCH;

[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class PageListModel
{
    /// <summary>
    /// Gets or sets the value of the page index
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// Gets or sets the value of the page size
    /// </summary>
    public int PageSize { get; set; }
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
        PageIndex = items.PageIndex;
        PageSize = items.PageSize;
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
