using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.DBContext;
using O24OpenAPI.Framework.Domain;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services.Mapping;

namespace O24OpenAPI.Report.API.Application.Utils;

public static class DataSourceProcessor
{
    public static async Task<object> ProcessDataSource(
        string jsonConfig,
        Dictionary<string, object> userInput,
        string targetValue,
        IDataMappingService dataMappingService,
        bool useCache = false,
        Dictionary<string, object> cache = null
    )
    {
        if (string.IsNullOrEmpty(jsonConfig))
        {
            throw new ArgumentException("JSON config is null or empty");
        }

        try
        {
            var config = JObject.Parse(jsonConfig);
            string type = config["type"]?.ToString();

            object result = null;
            string cacheKey = $"{jsonConfig}|{JsonConvert.SerializeObject(userInput)}";
            if (useCache && cache != null && cache.TryGetValue(cacheKey, out var cachedResult))
            {
                result = cachedResult;
            }
            else
            {
                JObject configObj = config.SelectToken("config.parameter").ToObject<JObject>();
                string name = config["name"]?.ToString();
                var parameters = await ExtractParameters(configObj, userInput, dataMappingService);

                result = type switch
                {
                    "STORE" => await ExecuteStoredProcedureAsync(
                        config.SelectToken("config.store")?.ToString() ?? "STORE",
                        parameters
                    ),
                    "VIEW" => await ExecuteViewAsync(
                        config.SelectToken("config.view")?.ToString() ?? "VIEW",
                        parameters
                    ),
                    "QUERY" => await ExecuteQueryAsync(
                        config.SelectToken("config.query")?.ToString() ?? "QUERY",
                        parameters,
                        name
                    ),
                    "INPUT" => userInput.TryGetValue(type, out var value),
                    _ => userInput.TryGetValue(type, out var value)
                        ? value
                        : throw new NotSupportedException($"Unsupported DataSource type: {type}"),
                };
                if (useCache)
                {
                    cache.Add(cacheKey, result);
                }
            }

            string configTargetResult = config.SelectToken("config.targetResult")?.ToString();

            // Nếu có configTargetResult → lấy block tương ứng
            if (!string.IsNullOrEmpty(configTargetResult) && type.Equals("STORE"))
            {
                if (result is List<Dictionary<string, object>> list && list.Count > 0)
                {
                    var first = list[0];
                    if (first.TryGetValue(configTargetResult, out var block))
                    {
                        // Nếu block là object
                        if (block is JObject jObj)
                        {
                            if (
                                !string.IsNullOrEmpty(targetValue)
                                && jObj.TryGetValue(targetValue, out var val)
                            )
                            {
                                return val?.ToString();
                            }

                            return jObj.ToString(Formatting.None);
                        }

                        if (block is JArray jArr && jArr.Count > 0)
                        {
                            var listDict = jArr.ToObject<List<Dictionary<string, object>>>();
                            if (!string.IsNullOrEmpty(targetValue))
                            {
                                foreach (var row in listDict)
                                {
                                    if (row.TryGetValue(targetValue, out var targetResult))
                                    {
                                        return targetResult?.ToString() ?? string.Empty;
                                    }
                                }
                            }
                            return JsonConvert.SerializeObject(listDict);
                        }

                        return string.Empty;
                    }
                }
            }

            // Nếu không có configTargetValue → xử lý logic cũ
            if (result is List<Dictionary<string, object>> dataDict && dataDict.Count > 0)
            {
                if (!string.IsNullOrEmpty(targetValue))
                {
                    foreach (var row in dataDict)
                    {
                        if (row.TryGetValue(targetValue, out var targetResult))
                        {
                            return targetResult?.ToString() ?? string.Empty;
                        }
                    }
                }
                return JsonConvert.SerializeObject(dataDict);
            }

            return result?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            Console.WriteLine($"⚠ Error processing DataSource: {ex.Message}");
            return null;
        }
    }

    private static async Task<Dictionary<string, object>> ExtractParameters(
        JObject parameterConfig,
        Dictionary<string, object> userInput,
        IDataMappingService dataMappingService
    )
    {
        if (parameterConfig == null)
        {
            return null;
        }
        return await dataMappingService.MapDataToDictionaryAsync(
            JObject.FromObject(userInput),
            parameterConfig
        );
    }

    private static async Task<List<Dictionary<string, object>>> ExecuteStoredProcedureAsync(
        string storeName,
        Dictionary<string, object> parameters
    )
    {
        if (string.IsNullOrEmpty(storeName))
        {
            throw new ArgumentException("Stored Procedure name is required");
        }

        Console.WriteLine(
            $"ℹ Executing Stored Procedure: {storeName} with params {JsonConvert.SerializeObject(parameters)}"
        );

        var dbContext = new ServiceDBContext();

        try
        {
            var result = await dbContext.CallServiceStoredProcedure(
                storeName,
                parameters,
                Singleton<O24OpenAPIConfiguration>.Instance.DWHSchema
            );

            if (result == null || result is DBNull)
            {
                return null;
            }

            if (result is string jsonString)
            {
                try
                {
                    jsonString = jsonString.Trim();
                    JToken jsonToken = JToken.Parse(jsonString);

                    switch (jsonToken.Type)
                    {
                        case JTokenType.Array:
                            // kiểu cũ: [ {...}, {...} ]
                            return jsonToken.ToObject<List<Dictionary<string, object>>>() ?? [];

                        case JTokenType.Object:
                            // kiểu mới: { "Header": {...}, "Detail": [...] }
                            // không chọn block nào, trả nguyên object vào list để ProcessDataSource xử lý tiếp
                            var obj = (JObject)jsonToken;
                            var wrapper = new Dictionary<string, object>();
                            foreach (var prop in obj.Properties())
                            {
                                wrapper[prop.Name] = prop.Value;
                            }
                            return [wrapper];

                        default:
                            return [];
                    }
                }
                catch (Exception ex)
                {
                    await ex.LogErrorAsync();
                    Console.WriteLine($"⚠ JSON parse error: {ex.Message}");
                    return
                    [
                        new()
                        {
                            { "Error", "Unexpected error occurred" },
                            { "Details", ex.Message },
                        },
                    ];
                }
            }

            return (List<Dictionary<string, object>>)(
                result ?? new List<Dictionary<string, object>>()
            );
        }
        catch (SqlException sqlEx)
        {
            await sqlEx.LogErrorAsync();
            Console.WriteLine($"❌ SQL Error (SP): {sqlEx.Message}");
            return [new() { { "Error", "Database error occurred" }, { "Details", sqlEx.Message } }];
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            Console.WriteLine($"⚠ Unexpected error (SP): {ex.Message}");
            return [new() { { "Error", "Unexpected error occurred" }, { "Details", ex.Message } }];
        }
    }

    private static async Task<List<Dictionary<string, object>>> ExecuteViewAsync(
        string viewName,
        Dictionary<string, object> parameters
    )
    {
        if (string.IsNullOrEmpty(viewName))
        {
            throw new ArgumentException("View name is required");
        }

        Console.WriteLine(
            $"ℹ Executing View: {viewName} with params {JsonConvert.SerializeObject(parameters)}"
        );

        string query = $"SELECT * FROM {viewName}";
        return await ExecuteQueryAsync(query, parameters, "VIEW");
    }

    private static async Task<List<Dictionary<string, object>>> ExecuteQueryAsync(
        string query,
        Dictionary<string, object> parameters,
        string name
    )
    {
        if (string.IsNullOrEmpty(query))
        {
            throw new ArgumentException("Query is required");
        }

        Console.WriteLine(
            $"ℹ Executing Query: {query} with params {JsonConvert.SerializeObject(parameters)}"
        );

        SQLAuditLog sQLAuditLog = new()
        {
            CommandName = $"SELECT_{name}",
            CommandType = "SELECT",
            SourceService = "RPT",
            ExecutedBy = "RPT",
            Params = System.Text.Json.JsonSerializer.Serialize(parameters),
        };

        var dbContext = new ServiceDBContext();

        try
        {
            var result = await dbContext.ExecuteQueryAsync(query, sQLAuditLog, parameters);
            return result ?? [];
        }
        catch (SqlException sqlEx)
        {
            await sqlEx.LogErrorAsync();
            Console.WriteLine($"❌ SQL Error: {sqlEx.Message}");
            return
            [
                new Dictionary<string, object>
                {
                    { "Error", "Database error occurred" },
                    { "Details", sqlEx.Message },
                },
            ];
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            Console.WriteLine($"⚠ Unexpected error: {ex.Message}");
            return
            [
                new Dictionary<string, object>
                {
                    { "Error", "Unexpected error occurred" },
                    { "Details", ex.Message },
                },
            ];
        }
    }
}
