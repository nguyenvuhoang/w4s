using Newtonsoft.Json.Linq;
using O24OpenAPI.CMS.API.Application.Models.Workflow;

namespace O24OpenAPI.CMS.API.Application.Utils;

/// <summary>
///
/// </summary>
public static class LinqExtensions
{
    /// <summary>
    ///
    /// </summary>
    public static IOrderedEnumerable<JObject> ApplyOrdering(
        this IEnumerable<JObject> source,
        List<ConditionOrder> orders
    )
    {
        IOrderedEnumerable<JObject> orderedQuery = null;

        foreach (var order in orders)
        {
            Func<JObject, int> keySelector = x =>
            {
                var token = x[order.Key];
                if (token == null)
                {
                    return 1;
                }
                var result = !string.IsNullOrEmpty(token?.ToObject<string>()) ? 1 : 0;
                return result;
            };

            if (orderedQuery == null)
            {
                if (order.IsNullFirst)
                {
                    if (order.Type == "Descending")
                    {
                        orderedQuery = source
                            .OrderBy(x => keySelector(x))
                            .ThenByDescending(keySelector);
                    }
                    else
                    {
                        orderedQuery = source.OrderBy(x => keySelector(x)).ThenBy(keySelector);
                    }
                }
                else
                {
                    if (order.Type == "Descending")
                    {
                        orderedQuery = source
                            .OrderByDescending(x => keySelector(x))
                            .ThenByDescending(keySelector);
                    }
                    else
                    {
                        orderedQuery = source
                            .OrderByDescending(x => keySelector(x))
                            .ThenBy(keySelector);
                    }
                }
            }
            else
            {
                if (order.IsNullFirst)
                {
                    if (order.Type == "Descending")
                    {
                        orderedQuery = orderedQuery
                            .ThenBy(x => keySelector(x))
                            .ThenByDescending(keySelector);
                    }
                    else
                    {
                        orderedQuery = orderedQuery.ThenBy(x => keySelector(x)).ThenBy(keySelector);
                    }
                }
                else
                {
                    if (order.Type == "Descending")
                    {
                        orderedQuery = orderedQuery
                            .ThenByDescending(x => keySelector(x))
                            .ThenByDescending(keySelector);
                    }
                    else
                    {
                        orderedQuery = orderedQuery
                            .ThenByDescending(x => keySelector(x))
                            .ThenBy(keySelector);
                    }
                }
            }
        }

        return orderedQuery;
    }
}

/// <summary>
/// The list extensions class
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Lists the to dis object using the specified source
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="source">The source</param>
    /// <returns>The result</returns>
    public static Dictionary<string, List<object>> ListToDisObject<T>(this List<T> source)
    {
        var result = new Dictionary<string, List<object>>();

        foreach (var item in source)
        {
            // var item = obj.t
            foreach (var property in item.GetType().GetProperties())
            {
                if (!result.ContainsKey(property.Name))
                {
                    result[property.Name] = new List<object>();
                }

                result[property.Name].Add(property.GetValue(item));
            }
        }
        return result;
    }
}
