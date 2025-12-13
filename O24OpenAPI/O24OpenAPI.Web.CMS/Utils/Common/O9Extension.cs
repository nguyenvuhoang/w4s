using System.Globalization;
using System.Text.Json;
using Apache.NMS.ActiveMQ.Util.Synchronization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Models.O9;

namespace Jits.Neptune.Web.CMS.LogicOptimal9.Common;

/// <summary>
/// The extensions class
/// </summary>
internal static class O9Extensions
{
    /// <summary>
    /// Datas the list to paged list using the specified j token
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="jToken">The token</param>
    /// <param name="pageIndex">The page index</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="getOnlyTotalCount">The get only total count</param>
    /// <returns>A paged list of t</returns>
    public static IPagedList<T> DataListToPagedList<T>(
        this JToken jToken,
        int pageIndex,
        int pageSize,
        bool getOnlyTotalCount = false
    )
    {
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var objList = System.Text.Json.JsonSerializer.Deserialize<IList<T>>(
                JsonConvert.SerializeObject(jToken["data"]),
                options
            );

            var pagedList = objList.AsQueryable().ToPagedList(pageIndex, pageSize).GetAsyncResult(); //new PagedList<T>((IList<T>)objList, pageIndex, pageSize, new int?(0));
            return pagedList;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jToken"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchFunc"></param>
    /// <param name="getOnlyTotalCount"></param>
    /// <returns></returns>
    public static IPagedList<T> ToPagedList<T>(
        this JToken jToken,
        int pageIndex,
        int pageSize,
        string searchFunc = null,
        bool getOnlyTotalCount = false
    )
    {
        if (jToken != null)
        {
            var objList = jToken.ToObject<IList<T>>();
            if (pageSize < 5)
            {
                return objList.AsQueryable().ToPagedList(pageIndex, pageSize).GetAsyncResult();
            }
            else
            {
                return objList.AsQueryable().O9ToPagedList(pageIndex, pageSize).GetAsyncResult();
            }
        }

        return new PagedList<T>(new List<T>(), pageIndex, pageSize);
    }

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

    public static PagedListModel<TEntity> ToPagedListModel<TEntity>(this PagedList<TEntity> items)
    {
        return new PagedListModel<TEntity>(items);
    }

    /// <summary>
    /// Returns the page list model using the specified items
    /// </summary>
    /// <param name="items">The items</param>
    /// <param name="total">The total</param>
    /// <returns>The page list model</returns>
    public static PageListModel ToPageListModel(this IPagedList<JObject> items, int total)
    {
        return new PageListModel(items, total);
    }

    /// <summary>
    /// Oes the 9 to paged list using the specified source
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="source">The source</param>
    /// <param name="pageIndex">The page index</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="getOnlyTotalCount">The get only total count</param>
    /// <returns>A task containing a paged list of t</returns>
    public static async Task<IPagedList<T>> O9ToPagedList<T>(
        this IQueryable<T> source,
        int pageIndex,
        int pageSize,
        bool getOnlyTotalCount = false
    )
    {
        await Task.CompletedTask;
        if (source == null)
        {
            return new PagedList<T>(new List<T>(), pageIndex, pageSize);
        }

        if (pageSize == 0)
        {
            pageSize = int.MaxValue;
        }

        pageSize = Math.Max(pageSize, 1);
        List<T> data = new List<T>();
        if (!getOnlyTotalCount)
        {
            List<T> list = data;
            //list.AddRange(await source.TakeSearchResult(pageIndex, pageSize));
            list.AddRange(source);
        }

        return new PagedList<T>(data, pageIndex, pageSize, data.Count);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static async Task<List<T>> TakeSearchResult<T>(
        this IQueryable<T> source,
        int pageIndex,
        int pageSize
    )
    {
        pageIndex = pageIndex + 1;
        var pagingSource = (int)Math.Ceiling((double)source.Count() / pageSize);
        if (pagingSource == 0)
        {
            pagingSource = 1;
        }

        var skip =
            pageIndex % pagingSource != 0
                ? pageIndex % pagingSource - 1
                : (pageIndex - 1) % pagingSource;
        var result = await source.Skip(skip * pageSize).Take(pageSize).ToListAsync();
        return result;
    }

    /// <summary>
    /// Returns the response model using the specified source
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="source">The source</param>
    /// <returns>The obj list</returns>
    public static T ToResponseModel<T>(this JsonBackOffice source)
    {
        var objList = System.Text.Json.JsonSerializer.Deserialize<T>(
            JsonConvert.SerializeObject(source.TXBODY[0].DATA)
        );
        return objList;
    }

    /// <summary>
    /// The page list model class
    /// </summary>
    /// <seealso cref="PagedModel"/>
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

    /// <summary>
    /// Formats the datime using the specified jobject
    /// </summary>
    /// <param name="Jobject">The jobject</param>
    /// <param name="format">The format</param>
    /// <returns>The jobject</returns>
    public static JObject FormatDatime(this JObject Jobject, string format = "dd/MM/yyyy")
    {
        foreach (var item in Jobject.Properties())
        {
            string ft = "dd/MM/yyyy";
            if (item.Value.Type == JTokenType.Date)
            {
                Jobject[item.Name] = Jobject.Value<DateTime>(item.Name).ToString(format);
            }

            if (!(item.Value.Type == JTokenType.Object || item.Value.Type == JTokenType.Array))
            {
                if (O9Utils.IsDatetime(Jobject.Value<string>(item.Name), out ft))
                {
                    var datetime = Jobject.Value<string>(item.Name);
                    DateTime dt = DateTime.ParseExact(datetime, ft, CultureInfo.InvariantCulture);
                    Jobject[item.Name] = dt.ToString(format);
                }
            }
            else if (item.Value.Type == JTokenType.Object)
            {
                JObject js = Jobject.Value<JObject>(item.Name);
                FormatDatime(js, format);
                Jobject[item.Name] = js;
            }
            else if (item.Value.Type == JTokenType.Array)
            {
                JArray jr = new JArray();
                if (item.Value.Count() > 0)
                {
                    if (item.Value.Count() > 0)
                    {
                        foreach (var item2 in item.Value.Children())
                        {
                            if (item2.Type == JTokenType.Object)
                            {
                                JObject js = JObject.FromObject(item2);
                                FormatDatime(js, format);
                                jr.Add(js);
                            }
                            else
                            {
                                jr.Add(item2);
                            }
                        }

                        Jobject[item.Name] = jr;
                    }
                }
            }
        }

        return Jobject;
    }
}
