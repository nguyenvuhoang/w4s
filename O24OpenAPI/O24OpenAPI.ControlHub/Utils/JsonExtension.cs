using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.ControlHub.Utils;

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
        catch (Exception ex)
        {
            System.Console.WriteLine(ex);
        }

        return default;
    }

    /// <summary>
    /// ToJArray
    /// </summary>
    /// <param name="wf"></param>
    /// <returns></returns>
    public static JArray ToJArray(this object wf)
    {
        return JArray.FromObject(wf);
    }
}
