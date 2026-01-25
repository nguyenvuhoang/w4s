using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.CMS.API.Application.Models.ContextModels;
using O24OpenAPI.CMS.API.Application.Models.Upload;
using O24OpenAPI.Core.Extensions;
using System.Reflection;
using System.Text.Json;

namespace O24OpenAPI.CMS.API.Application.Utils;

public static class JTokenExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="jsObject"></param>
    /// <returns></returns>
    public static JObject ConvertToJArray(this JObject jsObject)
    {
        JObject jsReturn = [];
        foreach (JProperty js in jsObject.Properties())
        {
            JArray jsArray = [js.Value];
            jsReturn.Add(js.Name.ToUpper(), jsArray);
        }

        return jsReturn;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="wf"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToSerializeSystemTextWithModel<T>(this object wf)
        where T : BaseO24OpenAPIModel
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(wf.ToSerializeSystemText());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="s"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T DeserializeSystemTextWithModel<T>(this string s)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(s);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="wf"></param>
    /// <returns></returns>
    public static JArray ToJArray(this object wf)
    {
        return JArray.FromObject(wf);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="wf"></param>
    /// <returns></returns>
    public static JToken ToJToken(
        this object wf,
        bool isIgnoreNullValue = false,
        JsonSerializerSettings settings = null
    )
    {
        if (settings == null && isIgnoreNullValue)
        {
            settings = new JsonSerializerSettings
            {
                NullValueHandling = isIgnoreNullValue
                    ? NullValueHandling.Ignore
                    : NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.All,
            };
            return JToken.FromObject(wf, Newtonsoft.Json.JsonSerializer.Create(settings));
        }

        return JToken.FromObject(wf);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jT"></param>
    /// <returns></returns>
    public static UploadResponseModel ToUploadResponseModel(this JToken jT)
    {
        return new UploadResponseModel()
        {
            name = jT.SelectToken("name")?.ToString(),
            status = (int)jT.SelectToken("status"),
            user_id = jT.SelectToken("user_id").ToString(),
        };
    }

    /// <summary>
    ///
    /// /// </summary>
    /// <param name="jT"></param>
    /// <returns></returns>
    public static int GetAsInt(this JToken jT)
    {
        return int.Parse(jT.ToString());
    }
}

public static class JsonExtension
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jObject"></param>
    /// <returns></returns>
    public static T MapToModel<T>(this JToken jObject)
    {
        if (jObject == null)
        {
            return default;
        }

        try
        {
            string strObject = JsonConvert.SerializeObject(jObject);
            T result = System.Text.Json.JsonSerializer.Deserialize<T>(strObject);
            return result;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static T JsonConvertToModel<T>(this object objectValue)
    {
        if (objectValue == null)
        {
            return default;
        }

        try
        {
            if (objectValue is string v)
            {
                return JsonConvert.DeserializeObject<T>(v);
            }
            else
            {
                string strObject = JsonConvert.SerializeObject(objectValue);
                return JsonConvert.DeserializeObject<T>(strObject);
            }
        }
        catch { }

        return default;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jObject"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static T JsonSerializerToModel<T>(
        this object jObject,
        JsonSerializerOptions options = null
    )
    {
        ArgumentNullException.ThrowIfNull(jObject);

        try
        {
            string jsonSerial;
            if (jObject is string jsonString)
            {
                jsonSerial = jsonString;
            }
            else
            {
                jsonSerial = JsonConvert.SerializeObject(jObject);
            }

            T? result = System.Text.Json.JsonSerializer.Deserialize<T>(jsonSerial, options);
            return result;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jObject"></param>
    /// <returns></returns>
    public static T JsonConvertToModel<T>(this JObject jObject)
    {
        try
        {
            string strObject = JsonConvert.SerializeObject(jObject);
            return JsonConvert.DeserializeObject<T>(strObject);
        }
        catch { }

        return default;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jsObject"></param>
    /// <returns></returns>
    public static JObject ConvertToJObject(this JObject jsObject)
    {
        try
        {
            JObject jsReturn = []; // Khởi tạo jsReturn là một JObject rỗng.

            if (jsObject == null)
            {
                return jsReturn;
            }

            foreach (JProperty js in jsObject.Properties())
            {
                switch (js.Value.Type)
                {
                    case JTokenType.Array:
                        JArray ja = (JArray)js.Value;
                        JArray newArray = [];
                        foreach (JToken item in ja)
                        {
                            if (item is JObject jObject)
                            {
                                newArray.Add(jObject.ConvertToJObject());
                            }
                            else
                            {
                                newArray.Add(item);
                            }
                        }

                        // Gán giá trị cho key, thay vì dùng Add.
                        jsReturn[js.Name.ToLower()] = newArray;
                        break;
                    case JTokenType.Object:
                        // Gán giá trị cho key, thay vì dùng Add.
                        jsReturn[js.Name.ToLower()] = ((JObject)js.Value).ConvertToJObject();
                        break;
                    case JTokenType.Null:
                        // Gán giá trị null cho key.
                        jsReturn[js.Name.ToLower()] = JValue.CreateNull();
                        break;
                    default:
                        // Gán giá trị cho key, thay vì dùng Add.
                        jsReturn[js.Name.ToLower()] = js.Value;
                        break;
                }
            }

            return jsReturn;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jsObject"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static JObject UpperKey(this JObject jsObject)
    {
        JObject jsReturn = [];
        if (jsObject == null)
        {
            return jsReturn;
        }

        foreach (JProperty js in jsObject.Properties())
        {
            switch (js.Value.Type)
            {
                case JTokenType.Array:
                    JArray ja = (JArray)js.Value;
                    JArray newArray = [];
                    foreach (JToken item in ja)
                    {
                        if (item is JObject jObject)
                        {
                            newArray.Add(jObject.UpperKey());
                        }
                        else
                        {
                            newArray.Add(item);
                        }
                    }

                    jsReturn.Add(js.Name.ToUpper(), newArray);
                    break;
                case JTokenType.Object:
                    jsReturn.Add(js.Name.ToUpper(), ((JObject)js.Value).UpperKey());
                    break;
                case JTokenType.Null:
                    jsReturn.Add(js.Name.ToUpper(), JValue.CreateNull());
                    break;
                default:
                    jsReturn.Add(js.Name.ToUpper(), js.Value);
                    break;
            }
        }

        return jsReturn;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jsArray"></param>
    /// <returns></returns>
    public static JObject ConvertToJArrayDetails(this JArray jsArray)
    {
        JObject jsReturn = [];

        foreach (JToken jsItem in jsArray)
        {
            foreach (JProperty js in ((JObject)jsItem).Properties())
            {
                string key = js.Name.ToUpper();
                JArray jsSub = jsReturn.ContainsKey(key) ? (JArray)jsReturn.SelectToken(key) : [];
                jsSub.Add(js.Value);
                jsReturn.Remove(key);
                jsReturn.Add(key, jsSub);
            }
        }

        return jsReturn;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jsObject"></param>
    /// <returns></returns>
    public static JArray ConvertToJObjectArrayDetails(this JObject jsObject)
    {
        JArray jsReturn = [];

        try
        {
            if (jsObject != null)
            {
                foreach (JProperty jsItem in jsObject.Properties())
                {
                    string key = jsItem.Name.ToLower();
                    for (int i = 0; i < jsItem.Values().Count(); i++)
                    {
                        JObject jsSub;
                        if (jsReturn.Count <= i)
                        {
                            jsSub = [];
                        }
                        else
                        {
                            jsSub = (JObject)jsReturn[i];
                        }

                        jsSub ??= [];
                        jsSub.Add(key, jsItem.Value[i]);

                        if (jsReturn.Count <= i)
                        {
                            jsReturn.Add(jsSub);
                        }
                        else
                        {
                            jsReturn[i] = jsSub;
                        }
                    }
                }
            }
        }
        catch
        {
            throw;
        }

        return jsReturn;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsObject"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T GetID<T>(this JObject jsObject, string key = null)
    {
        jsObject = jsObject.ToLowerKey();
        if (key != null)
        {
            if (jsObject.ContainsKey(key.ToLower()))
            {
                return jsObject.Value<T>(key.ToLower());
            }
        }

        if (jsObject.ContainsKey("id"))
        {
            T? value = jsObject.Value<T>("id");
            jsObject.Remove("id");
            return value;
        }

        return default;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jObject"></param>
    /// <param name="fillWithNull"></param>
    /// <returns></returns>
    public static JArray ToJArrayOfObjects(this JObject jObject, bool fillWithNull = true)
    {
        int maxLength = jObject.Properties().Max(p => ((JArray)p.Value).Count);

        return new JArray(
            Enumerable
                .Range(0, maxLength)
                .Select(i => new JObject(
                    jObject
                        .Properties()
                        .Select(p => new JProperty(
                            p.Name,
                            i < ((JArray)p.Value).Count
                                ? ((JArray)p.Value)[i]
                                : (fillWithNull ? null : JValue.CreateNull())
                        ))
                ))
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static JArray RenameFields(this JArray data, string[] fields)
    {
        try
        {
            foreach (JObject row in data.Children<JObject>())
            {
                row.RenameFields(fields);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error renaming fields: {ex.Message}", ex);
        }

        return data;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fields"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="Exception"></exception>
    public static void RenameFields(this JObject obj, string[] fields)
    {
        if (fields == null || fields.Length == 0)
        {
            throw new ArgumentException("Fields array cannot be null or empty.", nameof(fields));
        }

        try
        {
            if (fields.Length == 1)
            {
                string field = fields[0];
                if (obj.ContainsKey("0"))
                {
                    Rename(obj["0"], field);
                    if (obj[field].Type == JTokenType.Array)
                    {
                        JArray? value = obj[field] as JArray;
                        if (value?.Count == 1)
                        {
                            obj[field] = value[0];
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    string index = i.ToString();
                    if (obj.ContainsKey(index))
                    {
                        Rename(obj[index], fields[i]);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error renaming fields: {ex.Message}", ex);
        }
    }

    private static void Rename(JToken token, string newName)
    {
        if (token.Parent is JProperty property)
        {
            property.Replace(new JProperty(newName, property.Value));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jObject"></param>
    /// <returns></returns>
    public static JArray ToJArrayOfObjects(this JObject jObject)
    {
        if (jObject == null || jObject.Count == 0)
        {
            return [];
        }

        int max = jObject.Properties().Max(p => ((JArray)p.Value).Count);
        JArray jArray = [];

        for (int i = 0; i < max; i++)
        {
            JObject jsObj = [];

            foreach (JProperty item in jObject.Properties())
            {
                if (item.Value is JArray array && i < array.Count)
                {
                    jsObj.Add(item.Name, array[i]);
                }
                else
                {
                    jsObj.Add(item.Name, null);
                }
            }

            jArray.Add(jsObj);
        }

        return jArray;
    }

    /// <summary>
    ///
    /// </summary>
    public static bool GetBooleanValue(this JToken data, string key, bool defaultValue = false)
    {
        bool result = defaultValue;
        if (data != null)
        {
            bool? value = data.Value<bool?>(key);
            if (value.HasValue)
            {
                result = value.Value;
            }
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="dataSearch"></param>
    /// <returns></returns>
    public static List<TModel> ToListSearch<TModel>(this JArray dataSearch)
        where TModel : class
    {
        PropertyInfo[] properties = typeof(TModel).GetProperties();

        string[] propertyNames = properties
            .Select(p => p.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? p.Name)
            .ToArray();

        JArray renamedData = dataSearch.RenameFields(propertyNames);

        List<TModel?> resultList = renamedData.Select(item => item.ToObject<TModel>()).ToList();

        return resultList;
    }

    public static JObject AddContextFo(this JObject jObject)
    {
        JWebUIObjectContextModel? _context =
            EngineContext.Current.Resolve<JWebUIObjectContextModel>();

        Dictionary<string, object> dictionary = _context.Bo.GetFoInput().input;
        foreach (KeyValuePair<string, object> kvp in dictionary)
        {
            jObject.Add(kvp.Key, JToken.FromObject(kvp.Value));
        }
        return jObject;
    }
}
