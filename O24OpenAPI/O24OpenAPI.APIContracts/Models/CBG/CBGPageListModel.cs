using Newtonsoft.Json.Linq;

namespace O24OpenAPI.APIContracts.Models.CBG;

public class CBGPageListModel
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
}
