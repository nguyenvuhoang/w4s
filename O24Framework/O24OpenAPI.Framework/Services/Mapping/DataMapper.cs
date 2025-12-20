using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using LinKit.Json.Runtime;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Helper;
using O24OpenAPI.Framework.Constants;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.Framework.Services.Mapping;

public class DataMapper : IDataMapper
{
    public record MappingContext(JsonNode DataSource, string JsonPath);

    private static readonly Dictionary<string, Delegate> _handlers = new()
    {
        ["dataS"] = (GetValue<string>),
        ["dataSNull"] = (GetValue<string>),
        ["dataI"] = (GetValue<int>),
        ["dataINull"] = (GetValue<int?>),
        ["dataB"] = (GetValue<bool>),
        ["dataBNull"] = (GetValue<bool?>),
        ["dataN"] = (GetValue<decimal>),
        ["dataNNull"] = (GetValue<decimal?>),
        ["dataDt"] = (GetValue<DateTime>),
        ["dataDtNull"] = (GetValue<DateTime?>),
        ["dataD"] = (GetValue<double>),
        ["dataDNull"] = (GetValue<double?>),
        ["dataL"] = (GetValue<long>),
        ["dataLNull"] = (GetValue<long?>),
        ["dataO"] = (GetValue<JsonObject>),
        ["dataONull"] = (GetValue<JsonObject>),
        ["dataUnixToDate"] = GetDateTimeFromUnix,
        ["dataDateToUnix"] = GetUnixFromDateTime,
        ["dataUnixToDateLocal"] = GetDateTimeLocalFromUnix,
        ["dataDateToUnixLocal"] = GetUnixLocalFromDateTime,
        ["dataA"] = (GetValue<JsonArray>),
        ["dataANull"] = (GetValue<JsonArray>),
        ["dataSet"] = GetDataSet,
        ["dataUnixMilToDateLocal"] = GetDateTimeLocalFromUnixMil,
        ["dataFormatDate"] = GetFormatCustomDate,
    };

    public async Task<JsonNode> MapDataAsync(
        JsonNode source,
        JsonNode target,
        Func<string, Task<object>> func = null
    )
    {
        if (target is not JsonObject targetObj)
        {
            return target;
        }

        try
        {
            foreach (var property in targetObj.ToList())
            {
                if (property.Key == "condition" || property.Value == null)
                {
                    continue;
                }

                string configMapping =
                    property.Value is JsonValue value && value.TryGetValue<string>(out var str)
                        ? str.Trim()
                        : null;

                if (!string.IsNullOrEmpty(configMapping))
                {
                    if (func != null && configMapping.StartsWith("dataFunc"))
                    {
                        string jsonPath = ExtractJsonPath(configMapping);
                        targetObj[property.Key] = JsonValue.Create(await func(jsonPath));
                    }
                    else
                    {
                        var mappedValue = await Map(source, configMapping);

                        targetObj[property.Key] = mappedValue switch
                        {
                            JsonNode node => node.DeepClone(),
                            _ => JsonValue.Create(mappedValue),
                        };
                    }
                    continue;
                }

                switch (property.Value)
                {
                    case JsonArray array:
                        targetObj[property.Key] = await MapArrayDataAsync(source, array);
                        break;
                    case JsonObject obj:
                        targetObj[property.Key] = await MapDataAsync(source, obj, func);
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in MapDataAsync: {ex.Message}");
        }
        return target;
    }

    public async Task<Dictionary<string, object>> MapDataToDictionaryAsync(
        JsonNode source,
        JsonNode target,
        Func<string, Task<object>> func = null
    )
    {
        var result = new Dictionary<string, object>();

        if (target is not JsonObject targetObj)
        {
            return result;
        }

        try
        {
            foreach (var property in targetObj)
            {
                if (property.Key == "condition" || property.Value == null)
                {
                    continue;
                }

                if (property.Value is JsonValue value)
                {
                    if (value.TryGetValue<string>(out var str))
                    {
                        string configMapping = str.Trim();

                        if (!string.IsNullOrEmpty(configMapping))
                        {
                            if (func != null && configMapping.StartsWith("dataFunc"))
                            {
                                string jsonPath = ExtractJsonPath(configMapping);
                                result[property.Key] = await func(jsonPath);
                            }
                            else
                            {
                                try
                                {
                                    var mappedValue = await Map(source, configMapping);
                                    if (mappedValue is JsonNode node)
                                    {
                                        result[property.Key] = ConvertJsonNode(node);
                                    }
                                    else
                                    {
                                        result[property.Key] = mappedValue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    result[property.Key] = null;
                                    await ex.LogErrorAsync(
                                        new
                                        {
                                            source,
                                            target,
                                            property.Key,
                                            configMapping,
                                        }
                                    );
                                }
                            }
                        }
                        else
                        {
                            result[property.Key] = value.Deserialize<object>();
                        }
                    }
                    else
                    {
                        result[property.Key] = value.Deserialize<object>();
                    }

                    continue;
                }

                result[property.Key] = property.Value switch
                {
                    JsonArray array => await MapArrayDataAsync(source, array),
                    JsonObject obj => await MapDataAsync(source, obj, func),
                    _ => property.Value.Deserialize<object>(),
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in MapDataToDictionaryAsync: {ex.Message}");
            await ex.LogErrorAsync(new { source, target });
        }

        return result;
    }

    private static object ConvertJsonNode(JsonNode node)
    {
        return node switch
        {
            JsonObject obj => obj.ToDictionary(kvp => kvp.Key, kvp => ConvertJsonNode(kvp.Value)),
            JsonArray array => array.Select(ConvertJsonNode).ToList(),
            JsonValue value => GetPrimitiveValue(value),
            _ => null,
        };
    }

    private static object GetPrimitiveValue(JsonValue value)
    {
        if (value.TryGetValue(out bool b))
        {
            return b;
        }

        if (value.TryGetValue(out int i))
        {
            return i;
        }

        if (value.TryGetValue(out long l))
        {
            return l;
        }

        if (value.TryGetValue(out double d))
        {
            return d;
        }

        if (value.TryGetValue(out decimal m))
        {
            return m;
        }

        if (value.TryGetValue(out string s))
        {
            return s;
        }

        if (value.TryGetValue(out DateTime dt))
        {
            return dt;
        }

        return value.ToString();
    }

    private static string ExtractJsonPath(string configMapping)
    {
        int startIndex = configMapping.IndexOf('(') + 1;
        int endIndex = configMapping.LastIndexOf(')');
        return startIndex > 0 && endIndex > startIndex
            ? configMapping.Substring(startIndex, endIndex - startIndex).Trim()
            : string.Empty;
    }

    public static Task<object> Map(JsonNode source, string configMapping)
    {
        if (string.IsNullOrWhiteSpace(configMapping))
        {
            return Task.FromResult<object>(null);
        }

        var lastIndexOfFunc = configMapping.IndexOf('(');
        if (lastIndexOfFunc >= 0)
        {
            string function = configMapping[..lastIndexOfFunc].Trim();
            string jsonPath = ExtractJsonPath(configMapping);

            if (_handlers.TryGetValue(function, out var handler))
            {
                var context = new MappingContext(source, jsonPath);
                return handler switch
                {
                    Func<MappingContext, object> syncHandler => Task.FromResult(
                        syncHandler(context)
                    ),
                    // Func<MappingContext, Task<object>> asyncHandler => asyncHandler(context),
                    _ => Task.FromResult<object>(null),
                };
            }

            Console.WriteLine($"Unknown function: {function}");
            return Task.FromResult<object>(null);
        }
        return Task.FromResult<object>(configMapping);
    }

    private async Task<JsonArray> MapArrayDataAsync(JsonNode source, JsonArray target)
    {
        var tasks = new Task[target.Count];
        for (int i = 0; i < target.Count; i++)
        {
            int index = i;
            tasks[i] = Task.Run(async () =>
            {
                target[index] = target[index] switch
                {
                    JsonObject obj => await MapDataAsync(source, obj),
                    JsonArray array => await MapArrayDataAsync(source, array),
                    _ => target[index],
                };
            });
        }

        await Task.WhenAll(tasks);
        return target;
    }

    private static object GetValue<T>(MappingContext context)
    {
        if (context.DataSource.TryGetValueByPath(context.JsonPath, out T value))
        {
            return value;
        }
        else
        {
            return default(T);
        }
    }

    public static T ConvertJsonValue<T>(JsonValue jsonValue)
    {
        Type targetType = typeof(T);

        if (jsonValue.TryGetValue(out object rawValue))
        {
            if (rawValue == null || rawValue is DBNull)
            {
                return default;
            }

            // Handle Nullable types
            Type underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                return (T)ConvertJsonValueInternal(rawValue, underlyingType);
            }

            return (T)ConvertJsonValueInternal(rawValue, targetType);
        }

        throw new InvalidOperationException($"Cannot convert JsonValue to {typeof(T).Name}.");
    }

    private static object ConvertJsonValueInternal(object rawValue, Type targetType)
    {
        try
        {
            if (targetType == typeof(string))
            {
                return rawValue.ToString();
            }

            if (targetType.IsInstanceOfType(rawValue))
            {
                return rawValue;
            }

            if (targetType.IsEnum)
            {
                if (rawValue is string s)
                {
                    return Enum.Parse(targetType, s, ignoreCase: true);
                }

                return Enum.ToObject(targetType, rawValue);
            }

            if (targetType == typeof(bool))
            {
                if (rawValue is string s)
                {
                    return s == "true" || s == "1";
                }

                if (rawValue is int i)
                {
                    return i != 0;
                }

                if (rawValue is long l)
                {
                    return l != 0;
                }
            }

            if (targetType == typeof(DateTime))
            {
                string str = rawValue.ToString();

                if (
                    DateTime.TryParse(
                        str,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var dtGlobal
                    )
                )
                {
                    return dtGlobal;
                }

                if (
                    DateTime.TryParse(
                        str,
                        CultureInfo.CurrentCulture,
                        DateTimeStyles.None,
                        out var dtLocal
                    )
                )
                {
                    return dtLocal;
                }

                if (
                    DateTime.TryParseExact(
                        str,
                        DateTimeFormat.DateFormats,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var dt
                    )
                )
                {
                    return dt;
                }
            }

            if (targetType == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse(
                    rawValue.ToString(),
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind
                );
            }

            if (rawValue is JsonElement el)
            {
                return el.Deserialize(targetType, new JsonSerializerOptions());
            }
            else if (rawValue is IConvertible)
            {
                return Convert.ChangeType(rawValue, targetType, CultureInfo.InvariantCulture);
            }
            else
            {
                throw new InvalidCastException(
                    $"Cannot convert value of type {rawValue.GetType()} to {targetType.Name}"
                );
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Cannot convert value '{rawValue}' to type {targetType.Name}.",
                ex
            );
        }
    }

    private static object GetDateTimeFromUnix(MappingContext context)
    {
        var token = JsonHelper.GetValueByPath(context.DataSource, context.JsonPath);
        return token != null && long.TryParse(token.ToString(), out long unixTimestamp)
            ? DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime
            : DateTime.MinValue;
    }

    private static object GetUnixFromDateTime(MappingContext context)
    {
        var token = JsonHelper.GetValueByPath(context.DataSource, context.JsonPath);
        return token != null && DateTime.TryParse(token.ToString(), out DateTime dateTime)
            ? new DateTimeOffset(dateTime).ToUnixTimeSeconds()
            : 0;
    }

    private static object GetDateTimeLocalFromUnix(MappingContext context)
    {
        var token = JsonHelper.GetValueByPath(context.DataSource, context.JsonPath);
        return token != null && long.TryParse(token.ToString(), out long unixTimestamp)
            ? DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime.ToLocalTime()
            : DateTime.MinValue;
    }

    private static object GetUnixLocalFromDateTime(MappingContext context)
    {
        var token = JsonHelper.GetValueByPath(context.DataSource, context.JsonPath);
        return token != null && DateTime.TryParse(token.ToString(), out DateTime localDateTime)
            ? new DateTimeOffset(localDateTime.ToUniversalTime()).ToUnixTimeSeconds()
            : 0;
    }

    private static object GetDataSet(MappingContext context)
    {
        var result = new List<object>();
        var token = JsonHelper.GetValueByPath(context.DataSource, context.JsonPath);
        var configMappingItem = context.JsonPath.ToJsonNode() as JsonObject;

        if (configMappingItem == null || configMappingItem.Count == 0)
        {
            return result;
        }

        var firstMappingValue = configMappingItem.First().Value?.ToString();
        int start = firstMappingValue.IndexOf('(') + 1;
        int end = firstMappingValue.IndexOf('[', start);
        firstMappingValue = firstMappingValue.Substring(start, end - start);
        var firstSet =
            GetValue<JsonArray>(new MappingContext(context.DataSource, firstMappingValue))
            as JsonArray;
        var rowCount = firstSet?.Count ?? 0;

        for (int i = 0; i < rowCount; i++)
        {
            var obj = new Dictionary<string, object>();
            foreach (var kvp in configMappingItem)
            {
                var template = kvp.Value?.ToString() ?? "";
                var path = string.Format(template, i);
                obj[kvp.Key] = Map(context.DataSource, path).GetAwaiter().GetResult();
            }
            result.Add(obj);
        }

        return result;
    }

    private static object GetDateTimeLocalFromUnixMil(MappingContext context)
    {
        var token = JsonHelper.GetValueByPath(context.DataSource, context.JsonPath);
        if (!string.IsNullOrEmpty(token.ToString()))
        {
            long date1 = long.Parse(token.ToString());
            return DateTimeOffset
                .FromUnixTimeMilliseconds(date1)
                .UtcDateTime.ToLocalTime()
                .ToString("dd-MM-yyyy");
        }

        return null;
    }

    private static object GetFormatCustomDate(MappingContext context)
    {
        var token = JsonHelper.GetValueByPath(context.DataSource, context.JsonPath);
        if (!string.IsNullOrEmpty(token.ToString()))
        {
            string dataDate = token.ToString().Trim('"');
            DateTime date1;
            if (dataDate.Contains('|'))
            {
                var dataArray = dataDate.Split('|');
                date1 = DateTime.ParseExact(
                    dataArray[0],
                    dataArray[1],
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
                );
            }
            else
            {
                date1 = DateTime.ParseExact(
                    dataDate.ToString(),
                    DateTimeFormat.DateFormats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
                );
            }

            return date1.ToString("dd/MM/yyyy");
        }

        return null;
    }
}
