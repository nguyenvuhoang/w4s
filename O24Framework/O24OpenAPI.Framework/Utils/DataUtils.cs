using LinqToDB;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.Data.Configuration;
using O24OpenAPI.Framework.Models.UtilityModels;
using System.Collections;
using System.Linq.Expressions;

namespace O24OpenAPI.Framework.Utils;

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
        List<FileModel> listFiles = new List<FileModel>();
        Type entityType = Singleton<ITypeFinder>.Instance.FindEntityTypeByName(entityName);
        Type typeRepo = typeof(IRepository<>).MakeGenericType(entityType);
        object repo = EngineContext.Current.Resolve(typeRepo);
        System.Reflection.MethodInfo getTableMethod = typeRepo.GetMethod("GetTable");
        IQueryable queryableTable = getTableMethod?.Invoke(repo, null) as IQueryable;

        HashSet<string> requestFields = new HashSet<string>();
        ParameterExpression parameter = Expression.Parameter(entityType, "e");
        Expression finalExpression = Expression.Constant(false);

        foreach (Dictionary<string, object> requestModel in requestModels)
        {
            Expression condition = Expression.Constant(true);
            foreach (KeyValuePair<string, object> kvp in requestModel)
            {
                System.Reflection.PropertyInfo prop = entityType.GetProperty(kvp.Key);
                if (prop != null)
                {
                    requestFields.Add(kvp.Key);
                    MemberExpression entityProp = Expression.Property(parameter, prop);
                    ConstantExpression requestValue = Expression.Constant(kvp.Value);
                    BinaryExpression equalExpression = Expression.Equal(entityProp, requestValue);
                    condition = Expression.AndAlso(condition, equalExpression);
                }
            }
            finalExpression = Expression.OrElse(finalExpression, condition);
        }

        LambdaExpression lambda = Expression.Lambda(finalExpression, parameter);
        System.Reflection.MethodInfo whereMethod = typeof(Queryable)
            .GetMethods()
            .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
            .MakeGenericMethod(entityType);
        IQueryable<object> listData =
            (IQueryable<object>)whereMethod.Invoke(null, new object[] { queryableTable, lambda });

        foreach (object item in listData)
        {
            JArray jArray = new();
            string header = $"{{'type':'header','command':'Export data to Json'}}";
            jArray.Add(JToken.Parse(header));

            Dictionary<string, string> dbProperties = GetConnectionInfo();
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

            JArray jListData = new JArray { JObject.FromObject(item) };
            JObject data = new()
            {
                new JProperty("type", "data"),
                new JProperty("data", jListData),
            };
            jArray.Add(data);

            string filesName = GenerateFileName(JObject.FromObject(item), requestFields);
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
        List<FileModel> listFiles = new List<FileModel>();
        Type entityType = Singleton<ITypeFinder>.Instance.FindEntityTypeByName(entityName);
        Type typeRepo = typeof(IRepository<>).MakeGenericType(entityType);
        object repo = EngineContext.Current.Resolve(typeRepo);
        System.Reflection.MethodInfo getTableMethod = typeRepo.GetMethod("GetTable");
        IQueryable queryableTable = getTableMethod?.Invoke(repo, null) as IQueryable;

        HashSet<string> requestFields = new HashSet<string>();
        ParameterExpression parameter = Expression.Parameter(entityType, "e");
        Expression finalExpression = Expression.Constant(false);

        foreach (Dictionary<string, object> requestModel in requestModels)
        {
            Expression condition = Expression.Constant(true);
            foreach (KeyValuePair<string, object> kvp in requestModel)
            {
                System.Reflection.PropertyInfo prop = entityType.GetProperty(kvp.Key);
                if (prop != null)
                {
                    requestFields.Add(kvp.Key);
                    MemberExpression entityProp = Expression.Property(parameter, prop);
                    ConstantExpression requestValue = Expression.Constant(kvp.Value);
                    BinaryExpression equalExpression = Expression.Equal(entityProp, requestValue);
                    condition = Expression.AndAlso(condition, equalExpression);
                }
            }
            finalExpression = Expression.OrElse(finalExpression, condition);
        }

        LambdaExpression lambda = Expression.Lambda(finalExpression, parameter);
        System.Reflection.MethodInfo whereMethod = typeof(Queryable)
            .GetMethods()
            .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
            .MakeGenericMethod(entityType);
        IQueryable<object> listData =
            (IQueryable<object>)whereMethod.Invoke(null, new object[] { queryableTable, lambda });

        JArray jArray = [];
        string header = $@"{{'type':'header','command':'Export data to Json'}}";
        jArray.Add(JToken.Parse(header));

        // info
        Dictionary<string, string> dbProperties = GetConnectionInfo();
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
        JArray jListData = JArray.FromObject(listData);
        JObject data = new()
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
        foreach (string item in requestFields)
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

        IConfiguration _config = EngineContext.Current.Resolve<IConfiguration>();
        string connString = Singleton<DataConfig>.Instance.ConnectionString;
        string dataProvider = Singleton<DataConfig>.Instance.DataProvider.ToString();
        IEnumerable<string[]> items;
        switch (dataProvider.ToLower())
        {
            case "mysql"
            or "mariadb":
                items = connString
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Split('='));
                foreach (
                    string[] item in items.Where(s =>
                        s[0].ToLower() == "server" || s[0].ToLower() == "port"
                    )
                )
                {
                    result.Add(item[0].ToLower(), item[1]);
                }

                break;
            case "sqlserver":
                string server = connString
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault(s => s.Contains("server"));
                string[] temp = server?.Split(',');
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
                    string[] item in items.Where(s =>
                        s[0].ToLower() == "host" || s[0].ToLower() == "port"
                    )
                )
                {
                    result.Add(item[0].ToLower() == "host" ? "server" : item[0].ToLower(), item[1]);
                }

                break;
            case "oracle":
                string dataSource = connString
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault(s =>
                        s.Trim().StartsWith("Data Source", StringComparison.OrdinalIgnoreCase)
                    );

                if (dataSource is not null)
                {
                    string parts = dataSource.Split('=')[1];
                    string hostPort = parts.Split('/')[0];
                    string host = hostPort.Split(':')[0];
                    string port = hostPort.Split(':')[1];

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
        JArray data = JArray.Parse(content);
        JToken info = data[1];
        JToken dbProperties = info["db_properties"];
        string entityName = dbProperties[3]["value"].ToString();

        Type entity = Singleton<ITypeFinder>.Instance.FindEntityTypeByName(entityName);

        Type typeRepo = typeof(IRepository<>).MakeGenericType(entity);
        object repo = EngineContext.Current.Resolve(typeRepo);

        System.Reflection.MethodInfo getTableMethod = typeRepo.GetMethod("GetTable");
        System.Reflection.MethodInfo insertMethod = typeRepo.GetMethod("Insert");
        System.Reflection.MethodInfo deleteMethod = typeRepo.GetMethod("Delete");

        object queryableTable = getTableMethod?.Invoke(repo, null);

        JToken dataInfo = data[2];
        JToken listData = dataInfo["data"];

        foreach (JToken item in listData)
        {
            object entityObj = item.ToObject(entity);

            ParameterExpression parameter = Expression.Parameter(entity, "x");
            Expression predicate = Expression.Constant(true);

            foreach (string field in filedConstraints)
            {
                System.Reflection.PropertyInfo property = entity.GetProperty(field);
                if (property != null)
                {
                    object value = property.GetValue(entityObj);
                    MemberExpression propertyAccess = Expression.Property(parameter, field);
                    ConstantExpression valueExpression = Expression.Constant(value, property.PropertyType);
                    BinaryExpression equality = Expression.Equal(propertyAccess, valueExpression);

                    predicate = Expression.AndAlso(predicate, equality);
                }
            }

            LambdaExpression lambda = Expression.Lambda(predicate, parameter);

            System.Reflection.MethodInfo whereMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                .MakeGenericMethod(entity);

            object filteredData = whereMethod.Invoke(null, new object[] { queryableTable, lambda });

            System.Reflection.MethodInfo toListAsyncMethod = typeof(LinqToDB.AsyncExtensions)
                .GetMethod("ToListAsync")
                ?.MakeGenericMethod(entity);

            Task task = (Task)
                toListAsyncMethod.Invoke(
                    null,
                    new object[] { filteredData, CancellationToken.None }
                );
            await task.ConfigureAwait(false);

            IList listDbEntities = (IList)task.GetType().GetProperty("Result")?.GetValue(task);

            if (listDbEntities.Count > 0)
            {
                foreach (object dbEntity in listDbEntities)
                {
                    Task deleteTask = (Task)
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
        IRepository<TEntity> repo = EngineContext.Current.Resolve<IRepository<TEntity>>();

        List<TEntity> listData = await repo.Table.ToListAsync();
        if (listData.Any())
        {
            // header
            JArray jArray = new();
            string header = $@"{{'type':'header','command':'Export data to Json'}}";
            jArray.Add(JToken.Parse(header));

            // info
            Dictionary<string, string> dbProperties = GetConnectionInfo();
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
            JArray jListData = JArray.FromObject(listData);
            JObject data = new()
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
