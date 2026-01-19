using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.NCH.API.Application.Utils;

public static class JsonExtension
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="jsObject"></param>
    /// <returns></returns>
    public static JObject ConvertToJObject(this JObject jsObject)
    {
        try
        {
            JObject jsReturn = new JObject(); // Khởi tạo jsReturn là một JObject rỗng.

            if (jsObject == null)
            {
                return jsReturn;
            }

            foreach (var js in jsObject.Properties())
            {
                switch (js.Value.Type)
                {
                    case JTokenType.Array:
                        JArray ja = (JArray)js.Value;
                        JArray newArray = new JArray();
                        foreach (var item in ja)
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

    public static JObject? SafeParseJson(this string jsonString)
    {
        try
        {
            return JObject.Parse(jsonString);
        }
        catch
        {
            try
            {
                var unescaped = JsonConvert.DeserializeObject<string>(jsonString);
                if (string.IsNullOrWhiteSpace(unescaped))
                    return null;
                return JObject.Parse(unescaped);
            }
            catch
            {
                return null;
            }
        }
    }
}
