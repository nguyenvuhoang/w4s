using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Models.O9;

/// <summary>
///
/// </summary>
public class JsonData
{
    /// <summary>
    ///
    /// </summary>
    public string TABLENAME { get; set; }

    /// <summary>
    ///
    /// </summary>
    public object DATA { get; set; }

    /// <summary>
    ///
    /// </summary>
    public object FPK { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JsonData() { }

    /// <summary>
    ///
    /// </summary>
    public JsonData(JsonDataMapping clsJsonDataMapping)
    {
        if (clsJsonDataMapping != null)
        {
            TABLENAME = clsJsonDataMapping.T;
            if (clsJsonDataMapping.D is JObject)
            {
                DATA = ConvertMappingToOriginal((JObject)clsJsonDataMapping.D);
            }
            else
            {
                DATA = clsJsonDataMapping.D;
            }
            FPK = clsJsonDataMapping.P;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public JsonData(string tableName, object data, object fpk = null)
    {
        TABLENAME = tableName;
        DATA = data;
        FPK = fpk;
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject ConvertMappingToOriginal(string data)
    {
        return ConvertMappingToOriginal(JObject.Parse(data));
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject ConvertMappingToOriginal(JObject data)
    {
        JArray jsFieldOfTable = O9Utils.GetFieldOfTable();

        if (jsFieldOfTable != null && jsFieldOfTable.Count > 0)
        {
            JObject original = new();

            foreach (var jsValue in data)
            {
                int r = -1;
                int c = -1;
                string row = "";
                string col = "";

                if (
                    !string.IsNullOrEmpty(jsValue.Key)
                    && jsValue.Key.Length > 0
                    && jsValue.Key.StartsWith("#")
                )
                {
                    string key = jsValue.Key.Substring(1);

                    if (key.Length == 2)
                    {
                        row = key.Substring(0, 1);
                        col = key.Substring(1, 1);
                    }
                    else if (key.Length == 3)
                    {
                        row = key.Substring(0, 2);
                        col = key.Substring(2, 1);
                    }
                }

                if (!string.IsNullOrEmpty(row))
                {
                    if (row.Length == 1)
                    {
                        r = ConvertHashCodeToCount(char.ConvertToUtf32(row, 0));
                    }
                    else if (row.Length == 2)
                    {
                        r =
                            (ConvertHashCodeToCount(char.ConvertToUtf32(row, 0)) + 1) * 36
                            + ConvertHashCodeToCount(char.ConvertToUtf32(row, 1));
                    }
                }

                if (!string.IsNullOrEmpty(col))
                {
                    c = ConvertHashCodeToCount(char.ConvertToUtf32(col, 0));
                }

                string keyMap = "";

                if (r >= 0 && c >= 0)
                {
                    keyMap = ((JArray)jsFieldOfTable[r])[c].ToString();
                }

                var obj = new object();
                if (jsValue.Value.GetType() == typeof(JArray) && jsValue.Value.Any())
                {
                    obj = ((JArray)jsValue.Value)[0];
                }
                if (!string.IsNullOrEmpty(keyMap))
                {
                    original.Add(keyMap.ToLower(), obj.ToJToken());
                }
                else
                {
                    original.Add(jsValue.Key.ToLower(), obj.ToJToken());
                }
            }

            return original;
        }
        return null;
    }

    /// <summary>
    ///
    /// </summary>
    private static int ConvertHashCodeToCount(int hashCode)
    {
        if (hashCode >= 48 && hashCode <= 57)
        {
            hashCode -= 48;
        }
        else if (hashCode >= 65 && hashCode <= 90)
        {
            hashCode -= 55;
        }
        return hashCode;
    }
}

/// <summary>
///
/// </summary>
public class JsonDataMapping
{
    /// <summary>
    ///
    /// </summary>
    public string T { get; set; }

    /// <summary>
    ///
    /// </summary>
    public object D { get; set; }

    /// <summary>
    ///
    /// </summary>
    public object P { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JsonDataMapping() { }

    /// <summary>
    ///
    /// </summary>
    public JsonDataMapping(JsonData clsJsonData, bool isMappingToArray = false)
    {
        if (clsJsonData != null)
        {
            T = clsJsonData.TABLENAME;
            if (clsJsonData.DATA is JObject @object)
            {
                D = ConvertOriginalToMapping(@object, isMappingToArray);
            }
            else
            {
                D = clsJsonData.DATA;
            }
            //P = clsJsonData.FPK;
            if (clsJsonData.FPK is JObject object1)
            {
                P = object1.ConvertToJArray();
            }
            else
            {
                P = clsJsonData.FPK;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public JsonDataMapping(string t, JObject d, object p = null)
    {
        T = t;
        D = d;
        P = p;
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject ConvertOriginalToMapping(string data)
    {
        return ConvertOriginalToMapping(JObject.Parse(data));
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject ConvertOriginalToMapping(JObject data, bool isMappingToArray = false)
    {
        JArray jsFieldOfTable = O9Utils.GetFieldOfTable();
        int hC = 36;

        if (jsFieldOfTable != null && jsFieldOfTable.Count > 0)
        {
            JObject mapping = new();
            foreach (var jsValue in data)
            {
                int r = -1;
                int c = -1;
                bool isFind = false;

                for (r = 0; r < jsFieldOfTable.Count; r++)
                {
                    JArray jsArray = (JArray)jsFieldOfTable[r];
                    for (c = 0; c < jsArray.Count; c++)
                    {
                        if (((JValue)jsArray[c]).Value.Equals(jsValue.Key))
                        {
                            isFind = true;
                            break;
                        }
                    }
                    if (isFind)
                    {
                        break;
                    }
                }
                string row;
                if (r >= hC)
                {
                    int cv = (int)Math.Round((double)r / hC, 0);
                    row =
                        char.ConvertFromUtf32(ConvertCountToHashCode(cv) - 1)
                        + char.ConvertFromUtf32(ConvertCountToHashCode(r - cv * hC));
                }
                else
                {
                    row = char.ConvertFromUtf32(ConvertCountToHashCode(r));
                }

                if (isFind)
                {
                    if (jsValue.Value is JArray)
                    {
                        mapping.Add(
                            "#" + row + char.ConvertFromUtf32(ConvertCountToHashCode(c)),
                            jsValue.Value
                        );
                    }
                    else
                    {
                        mapping.Add(
                            "#" + row + char.ConvertFromUtf32(ConvertCountToHashCode(c)),
                            new JArray(jsValue.Value)
                        );
                    }
                }
                else
                {
                    if (!isMappingToArray)
                    {
                        if (jsValue.Value is JArray)
                        {
                            mapping.Add(jsValue.Key, jsValue.Value);
                        }
                        else
                        {
                            mapping.Add(jsValue.Key, new JArray(jsValue.Value));
                        }
                    }
                    else
                    {
                        mapping.Add(jsValue.Key, jsValue.Value);
                    }
                }
            }
            return mapping;
        }
        return null;
    }

    private static int ConvertCountToHashCode(int count)
    {
        if (count >= 0 && count <= 9)
        {
            count += 48;
        }
        else if (count >= 10 && count <= 35)
        {
            count += 55;
        }
        return count;
    }
}
