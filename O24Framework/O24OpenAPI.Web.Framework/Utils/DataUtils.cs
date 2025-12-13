using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Configuration;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Models.UtilityModels;
using System.Collections;
using System.Linq.Expressions;

namespace O24OpenAPI.Web.Framework.Utils;

/// <summary>
/// The data utils class
/// </summary>
public class DataUtils
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="requestAddress"></param>
    /// <param name="requestModels"></param>
    /// <param name="entityName"></param>
    /// <returns></returns>
    public static List<FileModel> ExportMultiFiles(
        string requestAddress,
        string entityName,
        List<Dictionary<string, object>> requestModels
    )
    {
        var listFiles = new List<FileModel>();
        var entityType = Singleton<ITypeFinder>.Instance.FindEntityTypeByName(entityName);
        var typeRepo = typeof(IRepository<>).MakeGenericType(entityType);
        object repo = EngineContext.Current.Resolve(typeRepo);
        var getTableMethod = typeRepo.GetMethod("GetTable");
        var queryableTable = getTableMethod?.Invoke(repo, null) as IQueryable;

        var requestFields = new HashSet<string>();
        var parameter = Expression.Parameter(entityType, "e");
        Expression finalExpression = Expression.Constant(false);

        foreach (var requestModel in requestModels)
        {
            Expression condition = Expression.Constant(true);
            foreach (var kvp in requestModel)
            {
                var prop = entityType.GetProperty(kvp.Key);
                if (prop != null)
                {
                    requestFields.Add(kvp.Key);
                    var entityProp = Expression.Property(parameter, prop);
                    var requestValue = Expression.Constant(kvp.Value);
                    var equalExpression = Expression.Equal(entityProp, requestValue);
                    condition = Expression.AndAlso(condition, equalExpression);
                }
            }
            finalExpression = Expression.OrElse(finalExpression, condition);
        }

        var lambda = Expression.Lambda(finalExpression, parameter);
        var whereMethod = typeof(Queryable)
            .GetMethods()
            .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
            .MakeGenericMethod(entityType);
        var listData =
            (IQueryable<object>)
                whereMethod.Invoke(null, new object[] { queryableTable, lambda });

        foreach (var item in listData)
        {
            JArray jArray = new();
            string header = $"{{'type':'header','command':'Export data to Json'}}";
            jArray.Add(JToken.Parse(header));

            var dbProperties = GetConnectionInfo();
            JObject info = new() { new JProperty("exported_time", DateTime.UtcNow) };
            if (requestAddress != null)
            {
                info.Add(new JProperty("host", requestAddress));
            }

            info.Add(
                new JProperty(
                    "db_properties",
                    new JArray(
                        new JObject { ["name"] = "server", ["value"] = dbProperties["server"] },
                        new JObject { ["name"] = "port", ["value"] = dbProperties["port"] },
                        new JObject
                        {
                            ["name"] = "database",
                            ["value"] =
                                $"{Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID}",
                        },
                        new JObject { ["name"] = "entity", ["value"] = entityName }
                    )
                )
            );
            info.Add(new JProperty("exported_by_fields", JArray.FromObject(requestModels)));
            jArray.Add(info);

            var jListData = new JArray { JObject.FromObject(item) };
            JObject data =
                new() { new JProperty("type", "data"), new JProperty("data", jListData) };
            jArray.Add(data);

            var filesName = GenerateFileName(JObject.FromObject(item), requestFields);
            listFiles.Add(
                new FileModel
                {
                    FileContent = jArray.ToString(),
                    FileName = entityName + $"_{filesName}.json",
                    ContentType = "application/json",
                }
            );
        }
        return listFiles;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="requestAddress"></param>
    /// <param name="requestModels"></param>
    /// <param name="entityName"></param>
    /// <returns></returns>
    public static FileModel ExportFile(
        string requestAddress,
        string entityName,
        List<Dictionary<string, object>> requestModels
    )
    {
        var listFiles = new List<FileModel>();
        var entityType = Singleton<ITypeFinder>.Instance.FindEntityTypeByName(entityName);
        var typeRepo = typeof(IRepository<>).MakeGenericType(entityType);
        object repo = EngineContext.Current.Resolve(typeRepo);
        var getTableMethod = typeRepo.GetMethod("GetTable");
        var queryableTable = getTableMethod?.Invoke(repo, null) as IQueryable;

        var requestFields = new HashSet<string>();
        var parameter = Expression.Parameter(entityType, "e");
        Expression finalExpression = Expression.Constant(false);

        foreach (var requestModel in requestModels)
        {
            Expression condition = Expression.Constant(true);
            foreach (var kvp in requestModel)
            {
                var prop = entityType.GetProperty(kvp.Key);
                if (prop != null)
                {
                    requestFields.Add(kvp.Key);
                    var entityProp = Expression.Property(parameter, prop);
                    var requestValue = Expression.Constant(kvp.Value);
                    var equalExpression = Expression.Equal(entityProp, requestValue);
                    condition = Expression.AndAlso(condition, equalExpression);
                }
            }
            finalExpression = Expression.OrElse(finalExpression, condition);
        }

        var lambda = Expression.Lambda(finalExpression, parameter);
        var whereMethod = typeof(Queryable)
            .GetMethods()
            .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
            .MakeGenericMethod(entityType);
        var listData =
            (IQueryable<object>)
                whereMethod.Invoke(null, new object[] { queryableTable, lambda });

        JArray jArray = [];
        string header = $@"{{'type':'header','command':'Export data to Json'}}";
        jArray.Add(JToken.Parse(header));

        // info
        var dbProperties = GetConnectionInfo();
        JObject info = new() { new JProperty(name: "exported_time", DateTime.UtcNow) };
        if (requestAddress != null)
        {
            info.Add(new JProperty(name: "host", requestAddress));
        }

        info.Add(
            new JProperty(
                name: "db_properties",
                JArray.Parse(
                    $@"[
                            {{""name"":""server"",""value"":""{dbProperties["server"]}""}},
                            {{""name"":""port"",""value"":""{dbProperties["port"]}""}},
                            {{""name"":""database"",""value"":""{Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID}""}},
                            {{""name"":""entity"",""value"":""{entityName}""}}
                        ]"
                )
            )
        );
        info.Add(
            new JProperty(
                name: "exported_by_fields",
                JArray.Parse(JsonConvert.SerializeObject(requestModels))
            )
        );
        jArray.Add(info.ToObject<JToken>());

        // data
        var jListData = JArray.FromObject(listData);
        JObject data =
            new()
            {
                new JProperty(name: "type", "data"),
                new JProperty(name: "data", jListData),
            };
        jArray.Add(data.ToObject<JToken>());
        return new FileModel
        {
            FileContent = jArray.ToString(),
            FileName = entityName + "Data.json",
            ContentType = "application/json",
        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jObj"></param>
    /// <param name="requestFields"></param>
    /// <returns></returns>
    public static string GenerateFileName(JObject jObj, HashSet<string> requestFields)
    {
        string fileName = "";
        foreach (var item in requestFields)
        {
            if (jObj.ContainsKey(item))
            {
                fileName += "___" + jObj.GetValue(item).ToString();
            }
        }
        return fileName;
    }

    /// <summary>
    /// Gets the connection info
    /// </summary>
    /// <returns>The result</returns>
    public static Dictionary<string, string> GetConnectionInfo()
    {
        Dictionary<string, string> result = [];

        var _config = EngineContext.Current.Resolve<IConfiguration>();
        var connString = Singleton<DataConfig>.Instance.ConnectionString;
        var dataProvider = Singleton<DataConfig>.Instance.DataProvider.ToString();
        IEnumerable<string[]> items;
        switch (dataProvider.ToLower())
        {
            case "mysql"
            or "mariadb":
                items = connString
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Split('='));
                foreach (
                    var item in items.Where(s =>
                        s[0].ToLower() == "server" || s[0].ToLower() == "port"
                    )
                )
                {
                    result.Add(item[0].ToLower(), item[1]);
                }

                break;
            case "sqlserver":
                var server = connString
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault(s => s.Contains("server"));
                var temp = server?.Split(',');
                if (temp is not null && temp.Length == 2)
                {
                    result.Add("server", temp[0]);
                    result.Add("port", temp[1]);
                }

                break;
            case "postgresql":
                items = connString
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Split('='));
                foreach (
                    var item in items.Where(s =>
                        s[0].ToLower() == "host" || s[0].ToLower() == "port"
                    )
                )
                {
                    result.Add(
                        item[0].ToLower() == "host" ? "server" : item[0].ToLower(),
                        item[1]
                    );
                }

                break;
            case "oracle":
                var dataSource = connString
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault(s => s.Trim().StartsWith("Data Source", StringComparison.OrdinalIgnoreCase));

                if (dataSource is not null)
                {
                    var parts = dataSource.Split('=')[1];
                    var hostPort = parts.Split('/')[0];
                    var host = hostPort.Split(':')[0];
                    var port = hostPort.Split(':')[1];

                    result.Add("server", host);
                    result.Add("port", port);
                }
                break;
            default:
                break;
        }
        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static async Task ImportFile(string content, List<string> filedConstraints)
    {
        var data = JArray.Parse(content);
        var info = data[1];
        var dbProperties = info["db_properties"];
        var entityName = dbProperties[3]["value"].ToString();

        var entity = Singleton<ITypeFinder>.Instance.FindEntityTypeByName(entityName);

        var typeRepo = typeof(IRepository<>).MakeGenericType(entity);
        object repo = EngineContext.Current.Resolve(typeRepo);

        var getTableMethod = typeRepo.GetMethod("GetTable");
        var insertMethod = typeRepo.GetMethod("Insert");
        var deleteMethod = typeRepo.GetMethod("Delete");

        var queryableTable = getTableMethod?.Invoke(repo, null);

        var dataInfo = data[2];
        var listData = dataInfo["data"];

        foreach (var item in listData)
        {
            var entityObj = item.ToObject(entity);

            var parameter = Expression.Parameter(entity, "x");
            Expression predicate = Expression.Constant(true);

            foreach (var field in filedConstraints)
            {
                var property = entity.GetProperty(field);
                if (property != null)
                {
                    var value = property.GetValue(entityObj);
                    var propertyAccess = Expression.Property(parameter, field);
                    var valueExpression = Expression.Constant(value, property.PropertyType);
                    var equality = Expression.Equal(propertyAccess, valueExpression);

                    predicate = Expression.AndAlso(predicate, equality);
                }
            }

            var lambda = Expression.Lambda(predicate, parameter);

            var whereMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                .MakeGenericMethod(entity);

            var filteredData = whereMethod.Invoke(
                null,
                new object[] { queryableTable, lambda }
            );

            var toListAsyncMethod = typeof(LinqToDB.AsyncExtensions)
                .GetMethod("ToListAsync")
                ?.MakeGenericMethod(entity);

            var task = (Task)
                toListAsyncMethod.Invoke(
                    null,
                    new object[] { filteredData, CancellationToken.None }
                );
            await task.ConfigureAwait(false);

            var listDbEntities = (IList)task.GetType().GetProperty("Result")?.GetValue(task);

            if (listDbEntities.Count > 0)
            {
                foreach (var dbEntity in listDbEntities)
                {
                    var deleteTask = (Task)
                        deleteMethod.Invoke(
                            repo,
                            new object[] { dbEntity, "", true, false, false }
                        );
                    await deleteTask.ConfigureAwait(false);
                }
            }

            insertMethod.Invoke(repo, new object[] { entityObj, "", true, false, false });
        }
    }

    /// <summary>
    /// Exports the all using the specified request address
    /// </summary>
    /// <typeparam name="TEntity">The entity</typeparam>
    /// <param name="requestAddress">The request address</param>
    /// <returns>A task containing the file model</returns>
    public static async Task<FileModel> ExportAll<TEntity>(string requestAddress)
        where TEntity : BaseEntity
    {
        var repo = EngineContext.Current.Resolve<IRepository<TEntity>>();

        var listData = await repo.Table.ToListAsync();
        if (listData.Any())
        {
            // header
            JArray jArray = new();
            string header = $@"{{'type':'header','command':'Export data to Json'}}";
            jArray.Add(JToken.Parse(header));

            // info
            var dbProperties = GetConnectionInfo();
            JObject info = new() { new JProperty(name: "exported_time", DateTime.UtcNow) };
            if (requestAddress != null)
            {
                info.Add(new JProperty(name: "host", requestAddress));
            }

            info.Add(
                new JProperty(
                    name: "db_properties",
                    JArray.Parse(
                        $@"[
                            {{""name"":""server"",""value"":""{dbProperties["server"]}""}},
                            {{""name"":""port"",""value"":""{dbProperties["port"]}""}},
                            {{""name"":""database"",""value"":""cms""}},
                            {{""name"":""entity"",""value"":""{typeof(TEntity).Name}""}}
                        ]"
                    )
                )
            );

            info.Add(new JProperty(name: "exported_by_fields", "Initial data"));
            jArray.Add(info.ToObject<JToken>());

            // data
            var jListData = JArray.FromObject(listData);
            JObject data =
                new()
                {
                    new JProperty(name: "type", "data"),
                    new JProperty(name: "data", jListData),
                };
            jArray.Add(data.ToObject<JToken>());

            return new FileModel
            {
                FileContent = jArray.ToString(),
                FileName = typeof(TEntity).Name + "Data.json",
                ContentType = "application/json",
            };
        }

        return new FileModel();
    }
}
