using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.CMS.API.Application.Models.ContextModels;
using O24OpenAPI.CMS.Domain.AggregateModels.Digital;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data.Configuration;
using O24OpenAPI.Data.System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using ILogger = O24OpenAPI.Framework.Services.Logging.ILogger;

namespace O24OpenAPI.CMS.API.Application.Utils;

public static class Utils
{
    private static AsyncLocal<string> _requestLanguage = new();

    /// <summary>
    ///
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static DateTime ConvertFromUnixTimestamp(long timestamp)
    {
        // return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
        DateTime origin = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return origin.AddTicks(timestamp);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static DateTime ConvertFromUnixTimestampMillisecond(long timestamp)
    {
        // return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
        DateTime origin = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return origin.AddMilliseconds(timestamp);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="timestamp"></param>
    /// <param name="isUtc"></param>
    /// <returns></returns>
    public static DateTime ConvertFromUnixTimestampMillisecond(long timestamp, bool isUtc = true)
    {
        // return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
        DateTime origin = new(
            1970,
            1,
            1,
            0,
            0,
            0,
            0,
            isUtc ? DateTimeKind.Utc : DateTimeKind.Local
        );
        return origin.AddMilliseconds(timestamp).ToLocalTime();
    }

    public static string FormatUnixTimestamp(
        long timestampMilliseconds,
        string formatString,
        bool isUtc = true
    )
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(
            timestampMilliseconds
        );
        DateTime dateTime;
        dateTime = dateTimeOffset.LocalDateTime;
        return dateTime.ToString(formatString);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static double ConvertToUnixTimestamp(DateTime date)
    {
        DateTime origin = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return Math.Floor(diff.TotalMilliseconds);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dataKey"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string GetDataJsonForKey(string dataKey, JObject data)
    {
        if (dataKey.Contains('.'))
        {
            string[] s_ = dataKey.Split("\\.");
            if (s_.Length > 0)
            {
                JObject obLoop = data;
                for (int i = 0; i < s_.Length; i++)
                {
                    if (i == (s_.Length - 1))
                    {
                        if (obLoop[s_[i]] == null)
                        {
                            return null;
                        }
                        else if (!IsValidJsonArray(obLoop[s_[i]].ToString()))
                        {
                            return obLoop[s_[i]].ToString();
                        }

                        return "";
                    }
                    else if (obLoop.ContainsKey(s_[i]))
                    {
                        obLoop = JObject.Parse(obLoop[s_[i]].ToString());
                    }
                }
            }
        }
        else
        {
            if (data.ContainsKey(dataKey))
            {
                return data[dataKey].ToString();
            }
        }

        return "";
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dataKey"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static JToken GetDataJsonForKeyOfContext(string dataKey, JObject data)
    {
        if (dataKey.Contains('.'))
        {
            string[] s_ = dataKey.Split("\\.");
            if (s_.Length > 0)
            {
                JObject obLoop = data;
                for (int i = 0; i < s_.Length; i++)
                {
                    if (i == (s_.Length - 1))
                    {
                        if (obLoop[s_[i]] == null)
                        {
                            return null;
                        }
                        else if (!IsValidJsonArray(obLoop[s_[i]].ToString()))
                        {
                            return obLoop[s_[i]];
                        }
                    }
                    else if (obLoop.ContainsKey(s_[i]))
                    {
                        obLoop = JObject.Parse(obLoop[s_[i]].ToString());
                    }
                }
            }
        }
        else
        {
            if (data.ContainsKey(dataKey))
            {
                return data[dataKey].ToString();
            }
        }

        return null;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="strInput"></param>
    /// <returns></returns>
    public static bool IsValidJsonObject(string strInput)
    {
        if (string.IsNullOrWhiteSpace(strInput))
        {
            return false;
        }

        strInput = strInput.Trim();
        if (strInput.StartsWith("{") && strInput.EndsWith('}')) //For array
        {
            try
            {
                JObject obj = JObject.Parse(strInput);
                return true;
            }
            catch (JsonReaderException jex)
            {
                //Exception in parsing json
                Console.WriteLine(jex.Message);
                return false;
            }
            catch (Exception ex) //some other exception
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="strInput"></param>
    /// <returns></returns>
    public static bool IsValidJsonArray(string strInput)
    {
        if (string.IsNullOrWhiteSpace(strInput))
        {
            return false;
        }

        strInput = strInput.Trim();
        if (strInput.StartsWith('[') && strInput.EndsWith(']')) //For array
        {
            try
            {
                JArray obj = JArray.Parse(strInput);
                return true;
            }
            catch (JsonReaderException jex)
            {
                //Exception in parsing json
                Console.WriteLine(jex.Message);
                return false;
            }
            catch (Exception ex) //some other exception
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Get date time
    /// </summary>
    /// <returns></returns>
    public static DateTime GetBusDate()
    {
        return DateTime.UtcNow;
    }

    public static Dictionary<string, string> ToDictionaryString(this object model)
    {
        string serializedModel = JsonConvert.SerializeObject(model, Formatting.None);
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedModel);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static Dictionary<string, object> ToDictionarySystemText(this object model)
    {
        string serializedModel = System.Text.Json.JsonSerializer.Serialize(model);
        return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(
            serializedModel
        );
    }

    /// <summary>
    /// Merge 2 dictionary
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns></returns>
    public static Dictionary<string, object> MergeDictionary(
        this Dictionary<string, object> obj1,
        Dictionary<string, object> obj2
    )
    {
        try
        {
            Dictionary<string, object> mergeDic = obj1;
            foreach (KeyValuePair<string, object> item in obj2)
            {
                if (!mergeDic.ContainsKey(item.Key))
                {
                    mergeDic.Add(item.Key, item.Value);
                }
            }

            return mergeDic;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="txcode"></param>
    /// <param name="foInput"></param>
    /// <returns></returns>
    public static JObject CreateFoQuick(string txcode, JObject foInput)
    {
        JObject rs_ = new();
        JArray listAction = new();
        JObject foOne = new() { new JProperty("txcode", txcode), new JProperty("input", foInput) };
        listAction.Add(foOne);
        rs_.Add(new JProperty("fo", listAction));
        return rs_;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="str"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public static string ReplaceFirst(this string str, string oldValue, string newValue)
    {
        int startIndex = str.IndexOf(oldValue);

        if (startIndex == -1)
        {
            return str;
        }

        return str.Remove(startIndex, oldValue.Length).Insert(startIndex, newValue);
    }

    /// <summary>
    /// Convert Dictionary to JObject
    /// </summary>
    /// <param name="lst"></param>
    /// <returns></returns>
    public static JObject ConvertToJObject(Dictionary<string, object> lst)
    {
        JObject jsResult = new();
        foreach (KeyValuePair<string, object> item in lst) { }

        return jsResult;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="anonymousObj"></param>
    /// <returns></returns>
    public static object RemoveNull(object anonymousObj)
    {
        string serilaizeJson = JsonConvert.SerializeObject(
            anonymousObj,
            Formatting.None,
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
        );

        dynamic result = JsonConvert.DeserializeObject<dynamic>(serilaizeJson);
        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userAgent"></param>
    /// <param name="device"></param>
    /// <returns></returns>
    public static string GetMobileVersion(string userAgent, string device)
    {
        string temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
        string version = string.Empty;

        foreach (char character in temp)
        {
            bool validCharacter = false;
            int test = 0;

            if (int.TryParse(character.ToString(), out test))
            {
                version += character;
                validCharacter = true;
            }

            if (character == '.' || character == '_')
            {
                version += '.';
                validCharacter = true;
            }

            if (validCharacter == false)
            {
                break;
            }
        }

        return version;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static string GetClientOs(HttpContext httpContext)
    {
        string rs_ = "";

        string ua = httpContext.Request.Headers["User-Agent"].ToString();

        if (ua.Contains("Android"))
        {
            return string.Format("Android {0}", GetMobileVersion(ua, "Android"));
        }

        if (ua.Contains("iPad"))
        {
            return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));
        }

        if (ua.Contains("iPhone"))
        {
            return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));
        }

        if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
        {
            return "Kindle Fire";
        }

        if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
        {
            return "Black Berry";
        }

        if (ua.Contains("Windows Phone"))
        {
            return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));
        }

        if (ua.Contains("Mac OS"))
        {
            return "Mac OS";
        }

        if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
        {
            return "Windows XP";
        }

        if (ua.Contains("Windows NT 6.0"))
        {
            return "Windows Vista";
        }

        if (ua.Contains("Windows NT 6.1"))
        {
            return "Windows 7";
        }

        if (ua.Contains("Windows NT 6.2"))
        {
            return "Windows 8";
        }

        if (ua.Contains("Windows NT 6.3"))
        {
            return "Windows 8.1";
        }

        if (ua.Contains("Windows NT 10"))
        {
            return "Windows 10";
        }

        return rs_;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static string GetClientBrowser(HttpContext httpContext)
    {
        string browser = "";
        string browserDetails = httpContext.Request.Headers["User-Agent"].ToString();
        string user = browserDetails.ToLower();
        if (user.Contains("msie"))
        {
            string Substring = browserDetails.Substring(browserDetails.IndexOf("MSIE")).Split(";")[
                0
            ];
            browser = Substring.Split(" ")[0].Replace("MSIE", "IE") + "-" + Substring.Split(" ")[1];
        }
        else if (user.Contains("safari") && user.Contains("version"))
        {
            browser =
                browserDetails.Substring(browserDetails.IndexOf("Safari")).Split(" ")[0].Split("/")[
                    0
                ]
                + "-"
                + browserDetails
                    .Substring(browserDetails.IndexOf("Version"))
                    .Split(" ")[0]
                    .Split("/")[1];
        }
        else if (user.Contains("opr") || user.Contains("opera"))
        {
            if (user.Contains("opera"))
            {
                browser =
                    browserDetails
                        .Substring(browserDetails.IndexOf("Opera"))
                        .Split(" ")[0]
                        .Split("/")[0]
                    + "-"
                    + browserDetails
                        .Substring(browserDetails.IndexOf("Version"))
                        .Split(" ")[0]
                        .Split("/")[1];
            }
            else if (user.Contains("opr"))
            {
                browser = browserDetails
                    .Substring(browserDetails.IndexOf("OPR"))
                    .Split(" ")[0]
                    .Replace("/", "-")
                    .Replace("OPR", "Opera");
            }
        }
        else if (
            (user.IndexOf("mozilla/7.0") > -1)
            || (user.IndexOf("netscape6") != -1)
            || (user.IndexOf("mozilla/4.7") != -1)
            || (user.IndexOf("mozilla/4.78") != -1)
            || (user.IndexOf("mozilla/4.08") != -1)
            || (user.IndexOf("mozilla/3") != -1)
        )
        {
            // browser=(userAgent.Substring(userAgent.IndexOf("MSIE")).Split("
            // ")[0]).Replace("/", "-");
            browser = "Netscape-?";
        }
        else if (user.Contains("edg"))
        {
            browser = browserDetails
                .Substring(browserDetails.IndexOf("Edg"))
                .Split(" ")[0]
                .Replace("/", "-");
        }
        else if (user.Contains("firefox"))
        {
            browser = browserDetails
                .Substring(browserDetails.IndexOf("Firefox"))
                .Split(" ")[0]
                .Replace("/", "-");
        }
        else if (user.Contains("chrome"))
        {
            browser = browserDetails
                .Substring(browserDetails.IndexOf("Chrome"))
                .Split(" ")[0]
                .Replace("/", "-");
        }
        else if (user.Contains("rv"))
        {
            browser = "IE";
        }
        else
        {
            browser = "UnKnown, More-Info: " + browserDetails;
            //browser = "UnKnown, More-Info: " + browserDetails.Substring(0, 30);
        }

        return browser;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static string GetClientIPAddress(HttpContext httpContext)
    {
        return httpContext.Connection.RemoteIpAddress.ToString();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetHeaders(HttpContext httpContext)
    {
        Dictionary<string, string> requestHeaders = [];
        foreach (KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> header in httpContext.Request.Headers)
        {
            requestHeaders.Add(header.Key.ToLower(), header.Value);
            Console.WriteLine($"{header.Key}: {header.Value}");
        }

        return requestHeaders;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetCookies(HttpContext httpContext)
    {
        Dictionary<string, string> requestCookies = [];
        foreach (KeyValuePair<string, string> header in httpContext.Request.Cookies)
        {
            requestCookies.Add(header.Key.ToLower(), header.Value);
        }

        return requestCookies;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="tableCode"></param>
    /// <returns></returns>
    public static JArray BuildTableCodeForArray(JArray arr, string tableCode)
    {
        JArray arrRes = new();
        foreach (JToken itemArr in arr)
        {
            JObject obj = new() { new JProperty(tableCode, itemArr) };
            arrRes.Add(obj);
        }

        return arrRes;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    public static void BuildStatusErrorResponse(ref JWebUIObjectContextModel context)
    {
        JObject obErr = new() { new JProperty("count", context.Bo.GetActionErrors().Count) };
        context.Bo.AddPackFo("errorJWebUI", obErr);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="mediaData"></param>
    public static bool FileWriter(string fullPath, string mediaData)
    {
        // Stopwatch sw = new Stopwatch();
        // sw.Start();
        try
        {
            File.WriteAllText(fullPath, mediaData);
            System.Console.WriteLine("Media Insert Done at: " + fullPath);
            return true;
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("Exception: " + ex);
            // return false;
            throw;
        }
        // sw.Stop();
        // System.Console.WriteLine("==== FileWriter exec time : " + sw.ElapsedMilliseconds);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="path"></param>
    public static void CreateDirectoryIfNotExist(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jObj"></param>
    /// <param name="requestFields"></param>
    /// <returns></returns>
    public static string generateFileName(JObject jObj, HashSet<string> requestFields)
    {
        string fileName = "";
        foreach (string item in requestFields)
        {
            if (jObj.ContainsKey(item))
            {
                fileName += "___" + jObj.GetValue(item).ToString();
            }
        }

        System.Console.WriteLine(fileName);
        return fileName;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="requestAddress"></param>
    /// <param name="requestModels"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TRequestModel"></typeparam>
    /// <returns></returns>
    public static async Task<List<FileModel>> ExportListFiles<TEntity, TRequestModel>(
        string requestAddress,
        List<TRequestModel> requestModels
    )
        where TEntity : BaseEntity
        where TRequestModel : BaseO24OpenAPIModel
    {
        List<FileModel> listFiles = new List<FileModel>();
        IRepository<TEntity> repo = EngineContext.Current.Resolve<IRepository<TEntity>>();
        HashSet<string> requestFields = new HashSet<string>();
        Func<TEntity, bool> conditionExport = s =>
        {
            bool result = false;
            foreach (TRequestModel requestModel in requestModels)
            {
                bool resultChild = true;
                PropertyInfo[] propsRequest = typeof(TRequestModel).GetProperties();
                foreach (PropertyInfo propRequest in propsRequest)
                {
                    requestFields.Add(propRequest.Name);
                    PropertyInfo propsEntity = typeof(TEntity).GetProperty(propRequest.Name);

                    resultChild &=
                        (propRequest.GetValue(requestModel) is not null)
                            ? propsEntity.GetValue(s).ToString()
                                == propRequest.GetValue(requestModel).ToString()
                            : true;
                }

                result |= resultChild;
            }

            return result;
        };
        IEnumerable<TEntity> listData = repo.Table.Where(conditionExport);
        foreach (TEntity item in listData)
        {
            JArray jArray = new();
            string header = $@"{{'type':'header','command':'Export data to Json'}}";
            jArray.Add(JToken.Parse(header));

            // info
            Dictionary<string, string> dbProperties = await GetConnectionInfo();
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
            info.Add(
                new JProperty(
                    name: "exported_by_fields",
                    JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(requestModels))
                )
            );
            jArray.Add(info.ToObject<JToken>());

            // data
            // var jListData = JArray.FromObject(listData);
            JArray jListData = new JArray { item.ToJObject() };
            JObject data = new()
            {
                new JProperty(name: "type", "data"),
                new JProperty(name: "data", jListData),
            };
            jArray.Add(data.ToObject<JToken>());
            Console.WriteLine("requestFields===" + requestFields.ToSerialize());
            string filesName = generateFileName(item.ToJObject(), requestFields);
            listFiles.Add(
                new FileModel
                {
                    FileContent = jArray.ToString(),
                    FileName = typeof(TEntity).Name + $"_{filesName}.json",
                    ContentType = "application/json",
                }
            );
        }

        return listFiles;
    }

    /// <summary>
    /// Export Data From DB
    /// </summary>
    /// <param name="requestAddress"></param>
    /// <param name="requestModels"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TRequestModel"></typeparam>
    /// <returns></returns>
    public static async Task<FileModel> ExportData<TEntity, TRequestModel>(
        string requestAddress,
        List<TRequestModel> requestModels
    )
        where TEntity : BaseEntity
        where TRequestModel : BaseO24OpenAPIModel
    {
        IRepository<TEntity> repo = EngineContext.Current.Resolve<IRepository<TEntity>>();
        Expression<Func<TEntity, bool>> condition = e => false;

        ParameterExpression param = condition.Parameters[0];

        foreach (TRequestModel request in requestModels)
        {
            Expression andExpr = Expression.Constant(true);

            foreach (PropertyInfo propReq in typeof(TRequestModel).GetProperties())
            {
                object value = propReq.GetValue(request);
                if (value == null) continue;

                PropertyInfo propEntity = typeof(TEntity).GetProperty(propReq.Name);
                if (propEntity == null) continue;

                MemberExpression left = Expression.Property(param, propEntity);
                ConstantExpression right = Expression.Constant(value);

                BinaryExpression equal = Expression.Equal(left, right);

                andExpr = Expression.AndAlso(andExpr, equal);
            }

            condition = Expression.Lambda<Func<TEntity, bool>>(
                Expression.OrElse(condition.Body, andExpr),
                param
            );
        }

        List<TEntity> listData = await repo.Table.Where(condition).ToListAsync();
        if (listData.Count != 0)
        {
            // header
            JArray jArray = new();
            string header = $@"{{'type':'header','command':'Export data to Json'}}";
            jArray.Add(JToken.Parse(header));

            // info
            Dictionary<string, string> dbProperties = await GetConnectionInfo();
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
                FileName = typeof(TEntity).Name + "Data.json",
                ContentType = "application/json",
            };
        }

        return new FileModel();
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="requestAddress"></param>
    /// <returns></returns>
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
            Dictionary<string, string> dbProperties = await GetConnectionInfo();
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="requestAddress"></param>
    /// <param name="requestModels"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TRequestModel"></typeparam>
    /// <returns></returns>
    public static async Task<FileModel> ExportDataWithContains<TEntity, TRequestModel>(
        string requestAddress,
        List<TRequestModel> requestModels
    )
        where TEntity : BaseEntity
        where TRequestModel : BaseO24OpenAPIModel
    {
        IRepository<TEntity> repo = EngineContext.Current.Resolve<IRepository<TEntity>>();
        Func<TEntity, bool> conditionExport = s =>
        {
            bool result = false;
            foreach (TRequestModel requestModel in requestModels)
            {
                bool resultChild = true;
                PropertyInfo[] propsRequest = typeof(TRequestModel).GetProperties();
                foreach (PropertyInfo propRequest in propsRequest)
                {
                    PropertyInfo propsEntity = typeof(TEntity).GetProperty(propRequest.Name);
                    resultChild &=
                        (propRequest.GetValue(requestModel) is not null)
                            ? propsEntity
                                .GetValue(s)
                                .ToString()
                                .Contains(propRequest.GetValue(requestModel).ToString())
                            : true;
                }

                result |= resultChild;
            }

            return result;
        };
        IEnumerable<TEntity> listData = repo.Table.Where(conditionExport);
        if (listData.Any())
        {
            // header
            JArray jArray = new();
            string header = $@"{{'type':'header','command':'Export data to Json'}}";
            jArray.Add(JToken.Parse(header));

            // info
            Dictionary<string, string> dbProperties = await GetConnectionInfo();
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
            info.Add(
                new JProperty(
                    name: "exported_by_fields",
                    JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(requestModels))
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
                FileName = typeof(TEntity).Name + "Data.json",
                ContentType = "application/json",
            };
        }

        return new FileModel();
    }

    /// <summary>
    /// UploadData
    /// </summary>
    /// <param name="content"></param>
    /// <param name="skipValue"></param>
    /// <param name="maximunValue"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TRequestModel"></typeparam>
    /// <returns></returns>
    public static async Task UploadData<TEntity, TRequestModel>(
    string content,
    int? skipValue = null,
    int? maximunValue = null
)
    where TEntity : BaseEntity, new()
    where TRequestModel : BaseO24OpenAPIModel
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        // 1. Parse upload data
        List<TEntity> uploadData = await GetListEntityFromJson<TEntity>(content);
        Console.WriteLine($"--- ListUpload Count: {uploadData.Count}");

        // 2. Apply skip / take
        if (skipValue is > 0)
            uploadData = uploadData.Skip(skipValue.Value).ToList();

        if (maximunValue is > 0)
            uploadData = uploadData.Take(maximunValue.Value).ToList();

        // 3. Prepare metadata
        PropertyInfo[] requestProps = typeof(TRequestModel).GetProperties();

        HashSet<string> primaryKeyNames = requestProps.Select(p => p.Name).ToHashSet();

        HashSet<string> ignoreFields = new HashSet<string>
    {
        "Id",
        "CreatedOnUtc",
        "UpdatedOnUtc"
    };
        foreach (string pk in primaryKeyNames)
            ignoreFields.Add(pk);

        // 4. Resolve repository
        IRepository<TEntity> repo = EngineContext.Current.Resolve<IRepository<TEntity>>();

        // ⚠️ intentional sync load (reconciliation in memory)
        List<TEntity> dbData = repo.Table.ToList();

        // 5. Prepare result collections
        List<TEntity> insertList = new List<TEntity>(uploadData);
        List<TEntity> updateList = new List<TEntity>();

        // 6. Build key selector (avoid reflection in inner loop)
        string BuildKey(object obj)
        {
            return string.Join('|',
                requestProps.Select(p =>
                    typeof(TEntity)
                        .GetProperty(p.Name)?
                        .GetValue(obj)?
                        .ToString() ?? string.Empty
                ));
        }

        // 7. Index DB data by key
        Dictionary<string, TEntity> dbLookup = dbData.ToDictionary(BuildKey);

        // 8. Process upload data
        foreach (TEntity entity in uploadData)
        {
            string key = BuildKey(entity);

            if (!dbLookup.TryGetValue(key, out TEntity dbEntity))
                continue;

            // matched → not insert
            insertList.Remove(entity);

            // different → update
            if (!IsModelEqual(entity, dbEntity, ignoreFields.ToArray()))
            {
                entity.Id = dbEntity.Id;
                repo.Update(entity);
                updateList.Add(entity);
            }
        }

        Console.WriteLine($"==== ListUpdate: Count: {updateList.Count}");
        Console.WriteLine($"==== ListInsert: Count: {insertList.Count}");

        // 9. Bulk insert new records
        IO24OpenAPIDataProvider dataProvider = EngineContext.Current.Resolve<IO24OpenAPIDataProvider>();
        await dataProvider.BulkInsertEntities(insertList);

        stopwatch.Stop();
        Console.WriteLine($"--- Total time upload (ms): {stopwatch.ElapsedMilliseconds}");

        await EngineContext.Current
            .Resolve<ILogger>()
            .Information($"{DateTime.Now} --- Upload completed");
    }


    /// <summary>
    /// UploadData
    /// </summary>
    /// <param name="content"></param>
    /// <param name="isTruncate"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task MigrateData<TEntity>(string content, bool isTruncate = false)
        where TEntity : BaseEntity
    {
        List<TEntity> listEntity = await GetListEntityFromJson<TEntity>(content);
        if (listEntity.Count > 0)
        {
            IO24OpenAPIDataProvider neptuneDataProvider = EngineContext.Current.Resolve<IO24OpenAPIDataProvider>();
            if (isTruncate)
            {
                await neptuneDataProvider.Truncate<TEntity>(true);
            }

            await neptuneDataProvider.BulkInsertEntities(listEntity);
        }
    }

    /// <summary>
    /// Read data from container
    /// </summary>
    /// <param name="content"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<List<TEntity>> ReadDataContent<TEntity>(string content)
        where TEntity : BaseEntity
    {
        try
        {
            JArray jArray = JArray.Parse(content);

            JObject jObject = jArray.Children<JObject>().FirstOrDefault(o => o["data"] != null);
            if (jObject is not null)
            {
                JArray data = jObject.Value<JArray>("data");
                string strData = JsonConvert.SerializeObject(data);

                List<TEntity> listData = JsonConvert.DeserializeObject<List<TEntity>>(strData);
                await Task.CompletedTask;
                return listData;
            }

            return new List<TEntity>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<TEntity>();
        }
    }

    /// <summary>
    /// Read data from container
    /// </summary>
    /// <param name="path"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<List<TEntity>> ReadData<TEntity>(string path)
        where TEntity : BaseEntity
    {
        try
        {
            JArray jArray = JArray.Parse(File.ReadAllText(path));

            JObject jObject = jArray.Children<JObject>().FirstOrDefault(o => o["data"] != null);
            if (jObject is not null)
            {
                JArray data = jObject.Value<JArray>("data");
                string strData = JsonConvert.SerializeObject(data);

                List<TEntity> listData = JsonConvert.DeserializeObject<List<TEntity>>(strData);
                await Task.CompletedTask;
                return listData;
            }

            return new List<TEntity>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<TEntity>();
        }
    }

    /// <summary>
    /// ImportData
    /// </summary>
    /// <param name="fileContent"></param>
    /// <param name="contentType"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<FileModel> ImportDataToContainer<TEntity>(
        string fileContent,
        string contentType
    )
        where TEntity : BaseEntity
    {
        JArray jArray = JArray.Parse(fileContent);
        JObject dbProperties = jArray
            .Children<JObject>()
            .FirstOrDefault(o => o["db_properties"] != null);
        if (dbProperties is not null)
        {
            // Check whether the Entity name is correct
            JArray jArrayProperties = dbProperties.Value<JArray>("db_properties");
            JToken jName = jArrayProperties
                .Children<JToken>()
                .FirstOrDefault(o => o["name"].ToString() == "entity");
            string name = jName?.Value<string>("value");
            if (name == typeof(TEntity).Name)
            {
                string filtPath = typeof(TEntity).Name + "Data.json";
                await File.WriteAllTextAsync(filtPath, fileContent);
                return new FileModel
                {
                    FileName = filtPath,
                    FileContent = fileContent,
                    ContentType = contentType,
                };
            }
        }

        return new FileModel();
    }

    /// <summary>
    /// GetConnectioInfo
    /// </summary>
    /// <returns></returns>
    public static async Task<Dictionary<string, string>> GetConnectionInfo()
    {
        Dictionary<string, string> result = new();
        // JObject jObject = JObject.Parse(File.ReadAllText(NeptuneConfigurationDefaults.AppSettingsFilePath));
        // var jToken = jObject.Value<JToken>("ConnectionStrings");
        // var connString = jToken.Value<string>("ConnectionString");
        // var dataProvider = jToken.Value<string>("DataProvider");

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
                items = connString
                    .Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Split(new[] { '=' }));
                foreach (
                    string[] item in items.Where(s =>
                        s[0].ToLower() == "host" || s[0].ToLower() == "port"
                    )
                )
                {
                    result.Add(item[0].ToLower() == "host" ? "server" : item[0].ToLower(), item[1]);
                }

                break;
            default:
                break;
        }

        await Task.CompletedTask;
        return result;
    }

    /// <summary>
    /// Check whether value of T1 properties equal T1 properties by property name
    /// </summary>
    /// <param name="baseModel"></param>
    /// <param name="comparedModel"></param>
    /// <param name="ignoreFields"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static bool IsModelEqual<T1, T2>(T1 baseModel, T2 comparedModel, string[] ignoreFields)
    {
        PropertyInfo[] propsT1 = typeof(T1).GetProperties();

        foreach (PropertyInfo propT1 in propsT1.Where(s => !s.Name.In(ignoreFields)))
        {
            PropertyInfo propT2 = typeof(T2).GetProperty(propT1.Name);
            try
            {
                if (
                    propT1.GetValue(baseModel)?.ToString()
                    != propT2.GetValue(comparedModel)?.ToString()
                )
                {
                    return false;
                }
            }
            catch
            {
                System.Console.WriteLine($"==== Missing property {propT1.Name} of model compared");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// GetListEntityFromJson
    /// </summary>
    /// <param name="content"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<List<TEntity>> GetListEntityFromJson<TEntity>(string content)
        where TEntity : BaseEntity
    {
        JArray jArray = JArray.Parse(content);
        JObject dbProperties = jArray
            .Children<JObject>()
            .FirstOrDefault(o => o["db_properties"] != null);

        // Check whether the Entity name is correct
        JArray jArrayProperties = dbProperties?.Value<JArray>("db_properties");
        JToken jName = jArrayProperties
            ?.Children<JToken>()
            .FirstOrDefault(o => o["name"].ToString() == "entity");
        string name = jName?.Value<string>("value");
        if (name == typeof(TEntity).Name)
        {
            IRepository<TEntity> repo = EngineContext.Current.Resolve<IRepository<TEntity>>();
            JObject jObject = jArray.Children<JObject>().FirstOrDefault(o => o["data"] != null);
            JArray data = jObject?.Value<JArray>("data");
            if (data is not null && data.Count > 0)
            {
                string strData = JsonConvert.SerializeObject(data);
                List<TEntity> listUploadData = JsonConvert.DeserializeObject<List<TEntity>>(strData);
                if (listUploadData.Count > 0)
                {
                    return listUploadData;
                }
            }
        }

        await Task.CompletedTask;
        return new List<TEntity>();
    }

    public static string GetRequestLanguage()
    {
        if (_requestLanguage.Value == null)
        {
            JWebUIObjectContextModel context = EngineContext.Current.Resolve<JWebUIObjectContextModel>();
            _requestLanguage.Value = context?.InfoRequest?.Language ?? "en";
        }
        return _requestLanguage.Value;
    }

    public static async Task<List<TEntity>> ReadDataCSV<TEntity>(string path)
        where TEntity : BaseEntity
    {
        List<TEntity> listData = new List<TEntity>();

        using (StreamReader reader = new StreamReader(path))
        using (
            CsvReader csv = new CsvReader(
                reader,
                new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    MissingFieldFound = null,
                    HeaderValidated = null,
                    // PrepareHeaderForMatch = args => args.Header.ToLower(),
                }
            )
        )
        {
            await csv.ReadAsync();
            csv.ReadHeader();

            while (await csv.ReadAsync())
            {
                TEntity entity = csv.GetRecord<TEntity>();
                listData.Add(entity);
            }
        }

        return listData;
    }

    public static async Task<List<TEntity>> ReadCSV<TEntity>(IFormFile file)
        where TEntity : class
    {
        try
        {
            List<TEntity> listData = new List<TEntity>();

            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            using (
                CsvReader csv = new CsvReader(
                    reader,
                    new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ",",
                        MissingFieldFound = null,
                        HeaderValidated = null,
                        // PrepareHeaderForMatch = args => args.Header.ToLower(),
                    }
                )
            )
            {
                await csv.ReadAsync();
                csv.ReadHeader();

                while (await csv.ReadAsync())
                {
                    TEntity entity = csv.GetRecord<TEntity>();
                    listData.Add(entity);
                }
            }
            return listData;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public static async Task<List<C_CODELIST>> ReadCodeListCSV(string path, string appCode)
    {
        List<C_CODELIST> listData = new List<C_CODELIST>();

        using (StreamReader reader = new StreamReader(path))
        using (
            CsvReader csv = new CsvReader(
                reader,
                new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    MissingFieldFound = null,
                    HeaderValidated = null,
                    // PrepareHeaderForMatch = args => args.Header.ToLower(),
                }
            )
        )
        {
            await csv.ReadAsync();
            csv.ReadHeader();

            while (await csv.ReadAsync())
            {
                TellerAppCdlist cdlist = csv.GetRecord<TellerAppCdlist>();
                C_CODELIST entity = new C_CODELIST()
                {
                    CodeId = cdlist.CDID,
                    CodeName = cdlist.CDNAME,
                    CodeGroup = cdlist.CDGRP,
                    Caption = cdlist.CAPTION,
                    CodeIndex = cdlist.CDIDX,
                    CodeValue = cdlist.CDVAL,
                    Ftag = cdlist.FTAG,
                    Visible = cdlist.VISIBLE == 1,
                    MCaption = new JObject() { { "lo", cdlist.LAO_LANGUAGE } }.ToSerialize(),
                    App = appCode,
                };
                listData.Add(entity);
            }
        }

        return listData;
    }



    public static string GetGroupId(S_USRCMD record, Dictionary<string, string> values)
    {
        if (record.CMDTYPE != "T")
        {
            return "";
        }

        if (values.TryGetValue(record.PARENTID, out string result))
        {
            return result;
        }
        else
        {
            return "";
        }
    }
}
