using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.CMS.API.Application.Utils;

public static class JsonExtension
{
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
}
