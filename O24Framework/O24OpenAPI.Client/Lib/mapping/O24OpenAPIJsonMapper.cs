using System.Text.Json;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core;
using StackExchange.Redis;
using Z.Expressions;

namespace O24OpenAPI.Client.Lib.mapping;

/// <summary>
/// The 24 open api json mapper class
/// </summary>
public class O24OpenAPIJsonMapper
{
    /// <summary>
    /// The length
    /// </summary>
    private readonly Dictionary<EnumMappingFunction, string> returnTypeMapper = new Dictionary<
        EnumMappingFunction,
        string
    >
    {
        { EnumMappingFunction.mapfrominput, null },
        { EnumMappingFunction.simpleevaluate, null },
        { EnumMappingFunction.grpccall, null },
        { EnumMappingFunction.grpc, null },
        { EnumMappingFunction.sum, "number" },
        { EnumMappingFunction.multiply, "number" },
        { EnumMappingFunction.divide, "number" },
        { EnumMappingFunction.round, "number" },
        { EnumMappingFunction.pow, "number" },
        { EnumMappingFunction.sqrt, "number" },
        { EnumMappingFunction.pi, "number" },
        { EnumMappingFunction.log, "number" },
        { EnumMappingFunction.log10, "number" },
        { EnumMappingFunction.log2, "number" },
        { EnumMappingFunction.isequal, "boolean" },
        { EnumMappingFunction.isnumberequal, "boolean" },
        { EnumMappingFunction.isnotequal, "boolean" },
        { EnumMappingFunction.isgreaterthan, "boolean" },
        { EnumMappingFunction.isgreaterorequal, "boolean" },
        { EnumMappingFunction.islessthan, "boolean" },
        { EnumMappingFunction.islessthanorequal, "boolean" },
        { EnumMappingFunction.isnull, "boolean" },
        { EnumMappingFunction.isnotnull, "boolean" },
        { EnumMappingFunction.isempty, "boolean" },
        { EnumMappingFunction.isnotempty, "boolean" },
        { EnumMappingFunction.isnullorempty, "boolean" },
        { EnumMappingFunction.isnotnullnotempty, "boolean" },
        { EnumMappingFunction.isstringequal, "boolean" },
        { EnumMappingFunction.isbooleanequal, "boolean" },
        { EnumMappingFunction.isboolequal, "boolean" },
        { EnumMappingFunction.and, "boolean" },
        { EnumMappingFunction.or, "boolean" },
        { EnumMappingFunction.leftchars, "string" },
        { EnumMappingFunction.rightchars, "string" },
        { EnumMappingFunction.substring, "string" },
        { EnumMappingFunction.replace, "string" },
        { EnumMappingFunction.concat, "string" },
        { EnumMappingFunction.join, "string" },
        { EnumMappingFunction.toupper, "string" },
        { EnumMappingFunction.tolower, "string" },
        { EnumMappingFunction.length, "string" },
    };

    /// <summary>
    /// Gets or sets the value of the asembly
    /// </summary>
    public string? asembly { get; set; }

    /// <summary>
    /// Gets or sets the value of the func
    /// </summary>
    public string? func { get; set; }

    /// <summary>
    /// Gets or sets the value of the type
    /// </summary>
    public string? type { get; set; }

    /// <summary>
    /// Gets or sets the value of the paras
    /// </summary>
    public object[]? paras { get; set; }

    /// <summary>
    /// Gets or sets the value of the mapping dictionary
    /// </summary>
    public IDictionary<string, string> MappingDictionary { get; set; } =
        new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets the value of the redis server name
    /// </summary>
    public static string? RedisServerName { get; set; }

    /// <summary>
    /// Gets or sets the value of the redis server port
    /// </summary>
    public static int RedisServerPort { get; set; }

    /// <summary>
    /// Gets or sets the value of the redis user name
    /// </summary>
    public static string? RedisUserName { get; set; }

    /// <summary>
    /// Gets or sets the value of the redis user password
    /// </summary>
    public static string? RedisUserPassword { get; set; }

    /// <summary>
    /// Converts the template to mapped object using the specified template
    /// </summary>
    /// <param name="template">The template</param>
    /// <param name="pJsonObject">The json object</param>
    /// <param name="pMappingDictionary">The mapping dictionary</param>
    /// <returns>The object</returns>
    public static object convertTemplateToMappedObject(
        string template,
        JObject pJsonObject,
        IDictionary<string, string> pMappingDictionary
    )
    {
        return inspect(JsonDocument.Parse(template).RootElement, pJsonObject, pMappingDictionary);
    }

    /// <summary>
    /// Inspects the e
    /// </summary>
    /// <param name="e">The </param>
    /// <param name="pJsonObject">The json object</param>
    /// <param name="pMappingDictionary">The mapping dictionary</param>
    /// <returns>The result</returns>
    public static object inspect(
        JsonElement e,
        JObject pJsonObject,
        IDictionary<string, string> pMappingDictionary
    )
    {
        object result = null;
        switch (e.ValueKind)
        {
            case JsonValueKind.Object:
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (JsonProperty item in e.EnumerateObject())
                {
                    object value = inspect(item.Value, pJsonObject, pMappingDictionary);
                    dictionary.Add(item.Name, value);
                }
                result = dictionary;
                O24OpenAPIJsonMapper neptuneJsonMapper =
                    System.Text.Json.JsonSerializer.Deserialize<O24OpenAPIJsonMapper>(
                        System.Text.Json.JsonSerializer.Serialize(dictionary)
                    );
                neptuneJsonMapper.MappingDictionary = pMappingDictionary;
                if (neptuneJsonMapper.IsMapped())
                {
                    result = neptuneJsonMapper.DoMap(pJsonObject);
                }
                break;
            }
            case JsonValueKind.Array:
            {
                List<object> list = new List<object>();
                foreach (JsonElement item2 in e.EnumerateArray())
                {
                    list.Add(inspect(item2, pJsonObject, pMappingDictionary));
                }
                result = list;
                break;
            }
            case JsonValueKind.Number:
                result = e.GetDouble();
                break;
            case JsonValueKind.String:
                result = e.GetString();
                break;
            case JsonValueKind.True:
                result = true;
                break;
            case JsonValueKind.False:
                result = false;
                break;
            case JsonValueKind.Null:
                result = null;
                break;
        }
        return result;
    }

    /// <summary>
    /// Ises the mapped
    /// </summary>
    /// <returns>The bool</returns>
    public bool IsMapped()
    {
        if (func != null)
        {
            return paras != null;
        }
        return false;
    }

    /// <summary>
    /// Gets the value from token using the specified t
    /// </summary>
    /// <param name="t">The </param>
    /// <returns>The result</returns>
    private object getValueFromToken(JToken t)
    {
        if (t.Type == JTokenType.Null)
        {
            return null;
        }
        if (string.IsNullOrEmpty(type))
        {
            switch (t.Type)
            {
                case JTokenType.String:
                    type = "string";
                    break;
                case JTokenType.Integer:
                case JTokenType.Float:
                    type = "number";
                    break;
                case JTokenType.Date:
                    type = "date";
                    break;
                case JTokenType.Array:
                    type = "array";
                    break;
                case JTokenType.Object:
                    type = "object";
                    break;
                case JTokenType.Boolean:
                    type = "Boolean";
                    break;
            }
        }
        if (type.ToLower().Equals("number"))
        {
            return (decimal)t;
        }
        if (type.ToLower().Equals("array"))
        {
            if (t.Type == JTokenType.Array)
            {
                if (t.Any())
                {
                    return System.Text.Json.JsonSerializer.Deserialize<List<object>>(t.ToString());
                }
                return System.Text.Json.JsonSerializer.Deserialize<List<object>>("[]");
            }
            return null;
        }
        if (type.ToLower().Equals("object"))
        {
            return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(
                t.ToString()
            );
        }
        if (type.ToLower().Equals("boolean"))
        {
            return bool.Parse(t.ToString());
        }
        string result = (string)t;
        if (t.Type.ToString().ToLower().Equals("date"))
        {
            result = JsonConvert.SerializeObject(t);
            result = result.Substring(1, result.Length - 2);
        }
        return result;
    }

    /// <summary>
    /// Selects the token using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <param name="jpath">The jpath</param>
    /// <exception cref="O24OpenAPIException"></exception>
    /// <returns>The token</returns>
    private JToken SelectToken(JObject pExecutionJObject, string jpath)
    {
        return pExecutionJObject.SelectToken(jpath)
            ?? throw new O24OpenAPIException(
                "JsonPath ["
                    + jpath
                    + "] is invalid in mapping process. Please make sure it is configurated exactly. Notice that JsonPath is case sensitive"
            );
    }

    /// <summary>
    /// Converts the paras to double values using the specified p json object
    /// </summary>
    /// <param name="pJsonObject">The json object</param>
    /// <returns>The array</returns>
    private double[] ConvertParasToDoubleValues(JObject pJsonObject)
    {
        double[] array = new double[paras.Length];
        for (int i = 0; i < paras.Length; i++)
        {
            if (paras[i].ToString().StartsWith("$"))
            {
                string jpath = paras[i].ToString();
                JToken jToken = SelectToken(pJsonObject, jpath);
                array[i] = double.Parse(jToken.ToString());
            }
            else if (paras[i].ToString().StartsWith("-$"))
            {
                string text = paras[i].ToString();
                string jpath2 = text.Substring(1, text.Length - 1);
                JToken jToken = SelectToken(pJsonObject, jpath2);
                array[i] = 0.0 - double.Parse(jToken.ToString());
            }
            else
            {
                array[i] = double.Parse(paras[i].ToString());
            }
        }
        return array;
    }

    /// <summary>
    /// Converts the paras to decimal values using the specified p json object
    /// </summary>
    /// <param name="pJsonObject">The json object</param>
    /// <returns>The array</returns>
    private decimal[] ConvertParasToDecimalValues(JObject pJsonObject)
    {
        decimal[] array = new decimal[paras.Length];
        for (int i = 0; i < paras.Length; i++)
        {
            if (paras[i].ToString().StartsWith("$"))
            {
                string jpath = paras[i].ToString();
                JToken jToken = SelectToken(pJsonObject, jpath);
                array[i] = decimal.Parse(jToken.ToString());
            }
            else if (paras[i].ToString().StartsWith("-$"))
            {
                string jpath2 = paras[i].ToString().Substring(1);
                JToken jToken = SelectToken(pJsonObject, jpath2);
                array[i] = -decimal.Parse(jToken.ToString());
            }
            else
            {
                array[i] = decimal.Parse(paras[i].ToString());
            }
        }
        return array;
    }

    /// <summary>
    /// Converts the paras to boolean values using the specified p json object
    /// </summary>
    /// <param name="pJsonObject">The json object</param>
    /// <returns>The array</returns>
    private bool[] ConvertParasToBooleanValues(JObject pJsonObject)
    {
        bool[] array = new bool[paras.Length];
        for (int i = 0; i < paras.Length; i++)
        {
            if (paras[i].ToString().StartsWith("$"))
            {
                string jpath = paras[i].ToString();
                JToken jToken = SelectToken(pJsonObject, jpath);
                array[i] = bool.Parse(jToken.ToString());
            }
            else
            {
                array[i] = bool.Parse(paras[i].ToString());
            }
        }
        return array;
    }

    /// <summary>
    /// Converts the paras to string values using the specified p json object
    /// </summary>
    /// <param name="pJsonObject">The json object</param>
    /// <returns>The array</returns>
    private string[] ConvertParasToStringValues(JObject pJsonObject)
    {
        string[] array = new string[paras.Length];
        for (int i = 0; i < paras.Length; i++)
        {
            if (paras[i] != null)
            {
                if (paras[i].ToString().StartsWith("$"))
                {
                    string jpath = paras[i].ToString();
                    JToken jToken = SelectToken(pJsonObject, jpath);
                    array[i] = jToken.ToString();
                }
                else
                {
                    array[i] = paras[i]?.ToString();
                }
            }
            else
            {
                array[i] = string.Empty;
            }
        }
        return array;
    }

    /// <summary>
    /// Maps the from input using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The object</returns>
    private object __MapFromInput(JObject pExecutionJObject)
    {
        string jpath = paras[0].ToString();
        JToken t = SelectToken(pExecutionJObject, jpath);
        return getValueFromToken(t);
    }

    /// <summary>
    /// Creates the mapping to string using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The string</returns>
    private string __FromMappingToString(JObject pExecutionJObject)
    {
        string text = paras[0].ToString();
        string key = text;
        if (text.StartsWith("$"))
        {
            key = (string)SelectToken(pExecutionJObject, text);
        }
        try
        {
            if (MappingDictionary.ContainsKey(key))
            {
                return MappingDictionary[key];
            }
            if (paras.Length > 1)
            {
                object obj = paras[1];
                if (obj != null)
                {
                    string text2 = obj.ToString();
                    if (text2.StartsWith("$."))
                    {
                        return SelectToken(pExecutionJObject, text2).ToString();
                    }
                    return text2;
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Creates the mapping to number using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The object</returns>
    private object __FromMappingToNumber(JObject pExecutionJObject)
    {
        try
        {
            string text = __FromMappingToString(pExecutionJObject);
            if (text == null)
            {
                return null;
            }
            return decimal.Parse(text);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Tokenses the to text array using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The object</returns>
    private object __TokensToTextArray(JObject pExecutionJObject)
    {
        try
        {
            List<string> list = new List<string>();
            string path = paras[0].ToString();
            foreach (JToken item in pExecutionJObject.SelectTokens(path))
            {
                if (item != null)
                {
                    list.Add(item.ToString());
                }
                else
                {
                    list.Add(null);
                }
            }
            return list;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Tokenses the to number array using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The object</returns>
    private object __TokensToNumberArray(JObject pExecutionJObject)
    {
        try
        {
            List<object> list = new List<object>();
            string path = paras[0].ToString();
            foreach (JToken item in pExecutionJObject.SelectTokens(path))
            {
                if (item != null)
                {
                    list.Add(decimal.Parse(item.ToString()));
                }
                else
                {
                    list.Add(null);
                }
            }
            return list;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Lefts the chars using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The string</returns>
    private string __LeftChars(JObject pExecutionJObject)
    {
        string jpath = paras[0].ToString();
        int length = int.Parse(paras[1].ToString());
        return ((string)SelectToken(pExecutionJObject, jpath))?.Substring(0, length);
    }

    /// <summary>
    /// Rights the chars using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The string</returns>
    private string __RightChars(JObject pExecutionJObject)
    {
        string jpath = paras[0].ToString();
        int num = int.Parse(paras[1].ToString());
        string text = (string)SelectToken(pExecutionJObject, jpath);
        return text?.Substring(text.Length - num, num);
    }

    /// <summary>
    /// Subs the string using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The string</returns>
    private string __SubString(JObject pExecutionJObject)
    {
        string jpath = paras[0].ToString();
        int num = int.Parse(paras[1].ToString());
        int length = int.Parse(paras[2].ToString());
        return ((string)SelectToken(pExecutionJObject, jpath))?.Substring(num - 1, length);
    }

    /// <summary>
    /// Replaces the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The string</returns>
    private string __Replace(JObject pExecutionJObject)
    {
        string jpath = paras[0].ToString();
        string text = paras[1].ToString();
        string text2 = paras[2].ToString();
        if (text2 == null)
        {
            text2 = "";
        }
        string text3 = (string)SelectToken(pExecutionJObject, jpath);
        if (text3 != null && text != null)
        {
            return text3.Replace(text, text2);
        }
        return null;
    }

    /// <summary>
    /// Concats the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The string</returns>
    private string __Concat(JObject pExecutionJObject)
    {
        string[] value = ConvertParasToStringValues(pExecutionJObject);
        return string.Join("", value);
    }

    /// <summary>
    /// Joins the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The string</returns>
    private string __Join(JObject pExecutionJObject)
    {
        string[] array = ConvertParasToStringValues(pExecutionJObject);
        string[] array2 = new string[array.Length - 1];
        for (int i = 1; i < array.Length; i++)
        {
            array2[i - 1] = array[i];
        }
        return string.Join(array[0], array2);
    }

    /// <summary>
    /// Returns the upper using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The string</returns>
    private string __ToUpper(JObject pExecutionJObject)
    {
        string text = paras[0].ToString();
        string text2 = text;
        if (text.TrimStart().StartsWith("$"))
        {
            text2 = (string)SelectToken(pExecutionJObject, text);
        }
        return text2?.ToUpper();
    }

    /// <summary>
    /// Returns the lower using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The string</returns>
    private string __ToLower(JObject pExecutionJObject)
    {
        string text = paras[0].ToString();
        string text2 = text;
        if (text.TrimStart().StartsWith("$"))
        {
            text2 = (string)SelectToken(pExecutionJObject, text);
        }
        return text2?.ToLower();
    }

    /// <summary>
    /// Lengths the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The int</returns>
    private int __Length(JObject pExecutionJObject)
    {
        string text = paras[0].ToString();
        string text2 = text;
        if (text.TrimStart().StartsWith("$"))
        {
            text2 = (string)SelectToken(pExecutionJObject, text);
        }
        return text2?.Length ?? 0;
    }

    /// <summary>
    /// Sums the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The result</returns>
    private decimal __Sum(JObject pExecutionJObject)
    {
        new List<string>();
        decimal result = default(decimal);
        for (int i = 0; i < paras.Length; i++)
        {
            string text = paras[i].ToString();
            int num = 1;
            if (text.StartsWith("-$"))
            {
                num = -1;
                text = text.Substring(1);
            }
            decimal num2;
            if (text.StartsWith("$"))
            {
                JToken t = SelectToken(pExecutionJObject, text);
                num2 = (decimal)getValueFromToken(t);
            }
            else
            {
                num2 = decimal.Parse(text);
            }
            result += (decimal)num * num2;
        }
        return result;
    }

    /// <summary>
    /// Multiplies the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The result</returns>
    private decimal __Multiply(JObject pExecutionJObject)
    {
        decimal[] array = ConvertParasToDecimalValues(pExecutionJObject);
        decimal result = 1m;
        decimal[] array2 = array;
        foreach (decimal num in array2)
        {
            result *= num;
        }
        return result;
    }

    /// <summary>
    /// Divides the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The decimal</returns>
    private decimal __Divide(JObject pExecutionJObject)
    {
        decimal[] array = ConvertParasToDecimalValues(pExecutionJObject);
        int decimals = 8;
        if (array.Length == 3)
        {
            decimals = (int)array[2];
        }
        return Math.Round(array[0] / array[1], decimals);
    }

    /// <summary>
    /// Rounds the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The double</returns>
    private double __Round(JObject pExecutionJObject)
    {
        double[] array = ConvertParasToDoubleValues(pExecutionJObject);
        return Math.Round(array[0], (int)array[1]);
    }

    /// <summary>
    /// Pows the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The double</returns>
    private double __Pow(JObject pExecutionJObject)
    {
        double[] array = ConvertParasToDoubleValues(pExecutionJObject);
        return Math.Pow(array[0], array[1]);
    }

    /// <summary>
    /// Sqrts the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The double</returns>
    private double __Sqrt(JObject pExecutionJObject)
    {
        return Math.Sqrt(ConvertParasToDoubleValues(pExecutionJObject)[0]);
    }

    /// <summary>
    /// Logs the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The double</returns>
    private double __Log(JObject pExecutionJObject)
    {
        return Math.Log(ConvertParasToDoubleValues(pExecutionJObject)[0]);
    }

    /// <summary>
    /// Logs the 10 using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The double</returns>
    private double __Log10(JObject pExecutionJObject)
    {
        return Math.Log10(ConvertParasToDoubleValues(pExecutionJObject)[0]);
    }

    /// <summary>
    /// Logs the 2 using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The double</returns>
    private double __Log2(JObject pExecutionJObject)
    {
        return Math.Log2(ConvertParasToDoubleValues(pExecutionJObject)[0]);
    }

    /// <summary>
    /// Pis the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The double</returns>
    private double __Pi(JObject pExecutionJObject)
    {
        return Math.PI;
    }

    /// <summary>
    /// Simples the evaluate using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The object</returns>
    private object __SimpleEvaluate(JObject pExecutionJObject)
    {
        string format = paras[0].ToString();
        object[] array = new object[paras.Length - 1];
        for (int i = 1; i < paras.Length; i++)
        {
            string jpath = paras[i].ToString();
            JToken t = SelectToken(pExecutionJObject, jpath);
            object valueFromToken = getValueFromToken(t);
            if (type.ToLower() == "number")
            {
                array[i - 1] = double.Parse(valueFromToken.ToString());
            }
            if (type.ToLower() == "string")
            {
                array[i - 1] = valueFromToken.ToString();
            }
        }
        format = string.Format(format, array);
        if (type.ToLower() == "string")
        {
            return format;
        }
        return Math.Round(Eval.Execute<double>(format), 8);
    }

    /// <summary>
    /// Ises the null using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsNull(JObject pExecutionJObject)
    {
        string jpath = paras[0].ToString();
        return SelectToken(pExecutionJObject, jpath).Type == JTokenType.Null;
    }

    /// <summary>
    /// Ises the not null using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsNotNull(JObject pExecutionJObject)
    {
        return !__IsNull(pExecutionJObject);
    }

    /// <summary>
    /// Ises the empty using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsEmpty(JObject pExecutionJObject)
    {
        string jpath = paras[0].ToString();
        JToken jToken = SelectToken(pExecutionJObject, jpath);
        if (jToken.Type != JTokenType.Null)
        {
            return jToken.ToString().Equals("");
        }
        return false;
    }

    /// <summary>
    /// Ises the string equal using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsStringEqual(JObject pExecutionJObject)
    {
        string text = paras[0].ToString();
        string text2 = paras[1].ToString();
        if (text.StartsWith("$"))
        {
            text = (string)SelectToken(pExecutionJObject, text);
        }
        if (text2.StartsWith("$"))
        {
            text2 = (string)SelectToken(pExecutionJObject, text2);
        }
        return text.Equals(text2);
    }

    /// <summary>
    /// Ises the boolean equal using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsBooleanEqual(JObject pExecutionJObject)
    {
        string text = paras[0].ToString();
        string text2 = paras[1].ToString();
        bool flag = (
            (!text.StartsWith("$"))
                ? bool.Parse(text)
                : SelectToken(pExecutionJObject, text).Value<bool>()
        );
        bool flag2 = (
            (!text2.StartsWith("$"))
                ? bool.Parse(text2)
                : SelectToken(pExecutionJObject, text2).Value<bool>()
        );
        return flag == flag2;
    }

    /// <summary>
    /// Ands the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __And(JObject pExecutionJObject)
    {
        bool[] array = ConvertParasToBooleanValues(pExecutionJObject);
        for (int i = 0; i < array.Length; i++)
        {
            if (!array[i])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Ors the p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __Or(JObject pExecutionJObject)
    {
        bool[] array = ConvertParasToBooleanValues(pExecutionJObject);
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i])
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Ises the not empty using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsNotEmpty(JObject pExecutionJObject)
    {
        string jpath = paras[0].ToString();
        JToken t = SelectToken(pExecutionJObject, jpath);
        object valueFromToken = getValueFromToken(t);
        if (valueFromToken != null)
        {
            return !valueFromToken.ToString().Equals("");
        }
        return false;
    }

    /// <summary>
    /// Ises the null or empty using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsNullOrEmpty(JObject pExecutionJObject)
    {
        string jpath = paras[0].ToString();
        return string.IsNullOrEmpty((string)SelectToken(pExecutionJObject, jpath));
    }

    /// <summary>
    /// Ises the not null not empty using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsNotNullNotEmpty(JObject pExecutionJObject)
    {
        return !__IsNullOrEmpty(pExecutionJObject);
    }

    /// <summary>
    /// Ises the equals using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsEquals(JObject pExecutionJObject)
    {
        return __CompareNumber(pExecutionJObject, EnumMappingFunction.isequal);
    }

    /// <summary>
    /// Ises the not equals using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsNotEquals(JObject pExecutionJObject)
    {
        return __CompareNumber(pExecutionJObject, EnumMappingFunction.isnotequal);
    }

    /// <summary>
    /// Compares the number using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <param name="pMappingFunction">The mapping function</param>
    /// <returns>The bool</returns>
    private bool __CompareNumber(JObject pExecutionJObject, EnumMappingFunction pMappingFunction)
    {
        double[] array = ConvertParasToDoubleValues(pExecutionJObject);
        double num = array[0];
        double num2 = array[1];
        return pMappingFunction switch
        {
            EnumMappingFunction.isequal => num == num2,
            EnumMappingFunction.isnotequal => num != num2,
            EnumMappingFunction.isgreaterthan => num > num2,
            EnumMappingFunction.isgreaterorequal => num >= num2,
            EnumMappingFunction.islessthan => num < num2,
            EnumMappingFunction.islessthanorequal => num <= num2,
            _ => false,
        };
    }

    /// <summary>
    /// Ises the greater than using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsGreaterThan(JObject pExecutionJObject)
    {
        return __CompareNumber(pExecutionJObject, EnumMappingFunction.isgreaterthan);
    }

    /// <summary>
    /// Ises the greater than or equals using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsGreaterThanOrEquals(JObject pExecutionJObject)
    {
        return __CompareNumber(pExecutionJObject, EnumMappingFunction.isgreaterorequal);
    }

    /// <summary>
    /// Ises the less than using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsLessThan(JObject pExecutionJObject)
    {
        return __CompareNumber(pExecutionJObject, EnumMappingFunction.islessthan);
    }

    /// <summary>
    /// Javas the script eval using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The object</returns>
    private object __JavaScriptEval(JObject pExecutionJObject)
    {
        using V8ScriptEngine v8ScriptEngine = new V8ScriptEngine();
        try
        {
            string text = pExecutionJObject.ToString();
            v8ScriptEngine.Execute("var $ = " + text + ";");
            string text2 = paras[0].ToString();
            for (int i = 1; i < paras.Length; i++)
            {
                string value = paras[i].ToString();
                v8ScriptEngine.Execute($"var ${i} = {value};");
            }
            v8ScriptEngine.Execute("var result = eval('" + text2.Replace("'", "\\'") + "');");
            return v8ScriptEngine.Script.result;
        }
        catch (ScriptEngineException ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    /// Redises the get string using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The string</returns>
    private string __RedisGetString(JObject pExecutionJObject)
    {
        try
        {
            string configuration = $"{RedisServerName}:{RedisServerPort}";
            string text = paras[0].ToString();
            using ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(
                configuration,
                delegate(ConfigurationOptions opt)
                {
                    if (!string.IsNullOrEmpty(RedisUserName))
                    {
                        opt.User = RedisUserName;
                    }
                    if (!string.IsNullOrEmpty(RedisUserPassword))
                    {
                        opt.Password = RedisUserPassword;
                    }
                }
            );
            RedisValue redisValue = connectionMultiplexer.GetDatabase().StringGet(text);
            connectionMultiplexer.Close();
            if (paras.Length >= 2 && !redisValue.HasValue && paras[1] != null)
            {
                return paras[1].ToString();
            }
            return redisValue;
        }
        catch (ScriptEngineException ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    /// Redises the get number using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The object</returns>
    private object __RedisGetNumber(JObject pExecutionJObject)
    {
        try
        {
            return decimal.Parse(__RedisGetString(pExecutionJObject));
        }
        catch (ScriptEngineException ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Ises the less than or equals using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The bool</returns>
    private bool __IsLessThanOrEquals(JObject pExecutionJObject)
    {
        return __CompareNumber(pExecutionJObject, EnumMappingFunction.islessthanorequal);
    }

    /// <summary>
    /// Normalizes the func
    /// </summary>
    /// <returns>The enum mapping function</returns>
    private EnumMappingFunction __Normalize_Func()
    {
        string text = func.Trim()
            .Replace("_", "")
            .Replace("-", "")
            .Replace(".", "")
            .Replace(" ", "")
            .ToLower();
        switch (text)
        {
            case "<":
                text = EnumMappingFunction.islessthan.ToString();
                break;
            case "<=":
            case "=<":
                text = EnumMappingFunction.islessthanorequal.ToString();
                break;
            case ">":
                text = EnumMappingFunction.isgreaterthan.ToString();
                break;
            case "=>":
            case ">=":
                text = EnumMappingFunction.isgreaterorequal.ToString();
                break;
            case "==":
                text = EnumMappingFunction.isnumberequal.ToString();
                break;
            case "b==b":
            case "===":
                text = EnumMappingFunction.isbooleanequal.ToString();
                break;
            case "s==s":
                text = EnumMappingFunction.isstringequal.ToString();
                break;
            case "<>":
            case "!=":
                text = EnumMappingFunction.isnotequal.ToString();
                break;
            case "||":
                text = EnumMappingFunction.or.ToString();
                break;
            case "*":
                text = EnumMappingFunction.multiply.ToString();
                break;
            case "/":
                text = EnumMappingFunction.divide.ToString();
                break;
            case "^":
                text = EnumMappingFunction.pow.ToString();
                break;
            case "&&":
                text = EnumMappingFunction.and.ToString();
                break;
            case "len":
                text = EnumMappingFunction.length.ToString();
                break;
            case "input":
                text = EnumMappingFunction.mapfrominput.ToString();
                break;
            case "map":
                text = EnumMappingFunction.mappingtostring.ToString();
                break;
            case "jseval":
                text = EnumMappingFunction.javascripteval.ToString();
                break;
        }
        try
        {
            return Enum.Parse<EnumMappingFunction>(text);
        }
        catch
        {
            Console.Error.WriteLine(
                "An error occurs in DoMap(): ["
                    + text
                    + "] is not a valid built-in function in Neptune portal."
            );
            throw;
        }
    }

    /// <summary>
    /// Does the map using the specified p execution j object
    /// </summary>
    /// <param name="pExecutionJObject">The execution object</param>
    /// <returns>The object</returns>
    public object DoMap(JObject pExecutionJObject)
    {
        try
        {
            switch (__Normalize_Func())
            {
                case EnumMappingFunction.mapfrominput:
                    return __MapFromInput(pExecutionJObject);
                case EnumMappingFunction.mappingtostring:
                    return __FromMappingToString(pExecutionJObject);
                case EnumMappingFunction.mappingtonumber:
                    return __FromMappingToNumber(pExecutionJObject);
                case EnumMappingFunction.tokenstotextarray:
                    return __TokensToTextArray(pExecutionJObject);
                case EnumMappingFunction.tokenstonumberarray:
                    return __TokensToNumberArray(pExecutionJObject);
                case EnumMappingFunction.isnull:
                    return __IsNull(pExecutionJObject);
                case EnumMappingFunction.isnotnull:
                    return __IsNotNull(pExecutionJObject);
                case EnumMappingFunction.isempty:
                    return __IsEmpty(pExecutionJObject);
                case EnumMappingFunction.isnotempty:
                    return __IsNotEmpty(pExecutionJObject);
                case EnumMappingFunction.isnullorempty:
                    return __IsNullOrEmpty(pExecutionJObject);
                case EnumMappingFunction.isnotnullnotempty:
                    return __IsNotNullNotEmpty(pExecutionJObject);
                case EnumMappingFunction.leftchars:
                    return __LeftChars(pExecutionJObject);
                case EnumMappingFunction.rightchars:
                    return __RightChars(pExecutionJObject);
                case EnumMappingFunction.substring:
                    return __SubString(pExecutionJObject);
                case EnumMappingFunction.replace:
                    return __Replace(pExecutionJObject);
                case EnumMappingFunction.concat:
                    return __Concat(pExecutionJObject);
                case EnumMappingFunction.join:
                    return __Join(pExecutionJObject);
                case EnumMappingFunction.toupper:
                    return __ToUpper(pExecutionJObject);
                case EnumMappingFunction.tolower:
                    return __ToLower(pExecutionJObject);
                case EnumMappingFunction.length:
                    return __Length(pExecutionJObject);
                case EnumMappingFunction.isstringequal:
                    return __IsStringEqual(pExecutionJObject);
                case EnumMappingFunction.isbooleanequal:
                case EnumMappingFunction.isboolequal:
                    return __IsBooleanEqual(pExecutionJObject);
                case EnumMappingFunction.and:
                    return __And(pExecutionJObject);
                case EnumMappingFunction.or:
                    return __Or(pExecutionJObject);
                case EnumMappingFunction.simpleevaluate:
                    return __SimpleEvaluate(pExecutionJObject);
                case EnumMappingFunction.isequal:
                case EnumMappingFunction.isnumberequal:
                    return __IsEquals(pExecutionJObject);
                case EnumMappingFunction.isnotequal:
                    return __IsNotEquals(pExecutionJObject);
                case EnumMappingFunction.isgreaterthan:
                    return __IsGreaterThan(pExecutionJObject);
                case EnumMappingFunction.isgreaterorequal:
                    return __IsGreaterThanOrEquals(pExecutionJObject);
                case EnumMappingFunction.islessthan:
                    return __IsLessThan(pExecutionJObject);
                case EnumMappingFunction.islessthanorequal:
                    return __IsLessThanOrEquals(pExecutionJObject);
                case EnumMappingFunction.sum:
                    return __Sum(pExecutionJObject);
                case EnumMappingFunction.multiply:
                    return __Multiply(pExecutionJObject);
                case EnumMappingFunction.divide:
                    return __Divide(pExecutionJObject);
                case EnumMappingFunction.round:
                    return __Round(pExecutionJObject);
                case EnumMappingFunction.pow:
                    return __Pow(pExecutionJObject);
                case EnumMappingFunction.sqrt:
                    return __Sqrt(pExecutionJObject);
                case EnumMappingFunction.log:
                    return __Log(pExecutionJObject);
                case EnumMappingFunction.log10:
                    return __Log10(pExecutionJObject);
                case EnumMappingFunction.log2:
                    return __Log2(pExecutionJObject);
                case EnumMappingFunction.pi:
                    return __Pi(pExecutionJObject);
                case EnumMappingFunction.javascripteval:
                    return __JavaScriptEval(pExecutionJObject);
                case EnumMappingFunction.redisgetstring:
                    return __RedisGetString(pExecutionJObject);
                case EnumMappingFunction.redisgetnumber:
                    return __RedisGetNumber(pExecutionJObject);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Mapping built-in function meets exceptions: function=[{func}]; parameters=[{string.Join(",", paras)}]. Exception: {ex.Message}"
            );
        }
        return null;
    }
}
