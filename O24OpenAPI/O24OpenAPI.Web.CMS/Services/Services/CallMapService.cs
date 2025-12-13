using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using DateTimeFormat = O24OpenAPI.Web.CMS.Utils.DateTimeFormat;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class CallMapService : ICallMapService
{
    public JWebUIObjectContextModel Context { get; set; } =
        EngineContext.Current.Resolve<JWebUIObjectContextModel>();

    private readonly Dictionary<
        string,
        Func<string, string, string, JObject, JObject, string, string, string, Task<JToken>>
    > _mappingFunctions;

    private static readonly ConcurrentDictionary<
        Type,
        Dictionary<string, PropertyInfo>
    > _propertyCache = new();

    /// <summary>
    ///
    /// </summary>
    public CallMapService()
    {
        _mappingFunctions = new Dictionary<
            string,
            Func<string, string, string, JObject, JObject, string, string, string, Task<JToken>>
        >
        {
            { "getToken", GetTokenAsync },
            { "stringSplitArrayJson", StringSplitArrayJsonAsync },
            { "arrayJson", ArrayJsonAsync },
            { "mergeDataWithJson", MergeDataWithJsonAsync },
            { "arrayDynamic", ArrayDynamicAsync },
            { "arrayNumber", ArrayNumberAsync },
            { "dateYYYY-MM-DD", FormatDateAsync },
            { "date_YYYY-MM-DD", FormatDateAsync },
            { "dateDD-MM-YYYY", FormatDateAsync },
            { "dateDD-MM-YYYYNull", FormatDateNullAsync },
            { "date_dd-MM-yyyy", FormatCustomDateAsync },
            { "date_dd-MM-yyyy_Null", FormatCustomDateNullAsync },
            { "date_yyyy-MM-dd", FormatCustomDate2Async },
            { "date_yyyy-MM-dd_Null", FormatCustomDate2NullAsync },
            { "time_dd-MM-yyyy", GetTimeAsync },
            { "arrayString", ArrayStringAsync },
            { "dataSumColumn", DataSumColumnAsync },
            { "DataBoolean", GetDataBooleanAsync },
            { "dataNZero", GetDataNZeroAsync },
            { "dataNZeroSub1", GetDataNZeroSub1Async },
            { "dataIZeroSub1", GetDataIZeroSub1Async },
            { "PageSizeDataDefault", GetPageSizeDataDefaultAsync },
            { "dataSelectNull", GetDataSelectNullAsync },
            { "dataSelectOneNull", GetDataSelectOneNullAsync },
            { "dataSelectINull", GetDataSelectINullAsync },
            { "dataS", GetDataSAsync },
            { "dataSNull", GetDataSNullAsync },
            { "dataSTrim", GetDataSTrimAsync },
            { "dataSDefault", GetDataSDefaultAsync },
            { "dataN", GetDataNAsync },
            { "dataL", GetDataLAsync },
            { "dataI", GetDataIAsync },
            { "dataINull", GetDataINullAsync },
            { "dataIZero", GetDataIZeroAsync },
            { "dataNNull", GetDataNNullAsync },
            { "dataLongNumber", GetDataLongNumberAsync },
            { "dataJsonObject", GetDataJsonObjectAsync },
            { "LowerKeyObject", GetDataObjectLowerKeyAsync },
            { "dataJsonArray", GetDataJsonArrayAsync },
            { "addToArray", AddToArrayAsync },
            { "stringSplit", StringSplitAsync },
            { "infoRequest", GetInfoRequestAsync },
            { "dataSerializeJson", GetDataSerializeJsonAsync },
            { "SerializeJArray", GetSerializeJArrayAsync },
            { "ToSerialize", GetToSerializeAsync },
            { "stringObject", GetStringObjectAsync },
            { "userSession", GetUserSessionAsync },
            { "SessionCore", GetSessionCoreAsync },
            { "getQueryValue", GetQueryValueAsync },
            { "getDateTime", GetDateTimeByFormat },
            { "JArray", JArrayAsync },
            { "AESEncrypt", AESEncrypt },
            { "dataHeader", GetDataHeader },
            { "userInfoByToken", UserInfoByToken },
            { "dataSetting", GetSettingValueAsync },
        };
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<JObject> LoopConfigMapping(
        JObject obMapp,
        string appRequest,
        string uid,
        JObject dataMap,
        string learnApiData,
        bool isO9Mapping = false
    )
    {
        try
        {
            foreach (var itemObMap_ in obMapp)
            {
                if (itemObMap_.Key == "condition")
                {
                    continue;
                }

                if (itemObMap_.Value == null)
                {
                    continue;
                }

                string valueStr = itemObMap_.Value.ToString();

                if (Utils.Utils.IsValidJsonArray(valueStr))
                {
                    var obarray = JArray.FromObject(JsonConvert.DeserializeObject(valueStr));
                    obarray ??= new JArray();
                    obMapp[itemObMap_.Key] = await LoopConfigMappingArray(
                        obarray,
                        "",
                        "",
                        dataMap,
                        "",
                        isO9Mapping
                    );
                }
                else if (Utils.Utils.IsValidJsonObject(valueStr))
                {
                    JObject newObMap = JObject.FromObject(itemObMap_.Value);
                    obMapp[itemObMap_.Key] = await LoopConfigMapping(
                        newObMap,
                        appRequest,
                        uid,
                        dataMap,
                        learnApiData,
                        isO9Mapping
                    );
                }
                else
                {
                    string rs = valueStr;
                    if (!string.IsNullOrEmpty(rs))
                    {
                        rs = rs.Trim();
                        if (rs.Contains("MapS."))
                        {
                            rs = rs.ReplaceFirst("MapS.", "");
                            int iRs = await CallMapS(
                                obMapp,
                                itemObMap_.Key,
                                rs,
                                appRequest,
                                uid,
                                dataMap,
                                learnApiData,
                                isO9Mapping
                            );
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("LoopConfigMapping exception()==" + ex.StackTrace);
        }

        return obMapp;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<JArray> LoopConfigMappingArray(
        JArray obMapp,
        string appRequest,
        string uid,
        JObject dataMap,
        string learnApiData,
        bool isO9Mapping = false
    )
    {
        try
        {
            for (var i = 0; i < obMapp.Count; i++)
            {
                var itemObMapInstance = obMapp[i].ToJObject();
                foreach (var itemObMap in itemObMapInstance)
                {
                    if (itemObMap.Value == null)
                    {
                        continue;
                    }

                    string valueStr = itemObMap.Value.ToString();

                    if (Utils.Utils.IsValidJsonArray(valueStr)) { }
                    else if (Utils.Utils.IsValidJsonObject(valueStr))
                    {
                        JObject newObMap = JObject.FromObject(itemObMap.Value);
                        obMapp[itemObMap.Key] = await LoopConfigMapping(
                            newObMap,
                            appRequest,
                            uid,
                            dataMap,
                            learnApiData,
                            isO9Mapping
                        );
                    }
                    else
                    {
                        string rs = valueStr;
                        if (!string.IsNullOrEmpty(rs))
                        {
                            rs = rs.Trim();
                            if (rs.Contains("MapS."))
                            {
                                rs = rs.Replace("MapS.", "");
                                int iRs = await CallMapS(
                                    itemObMapInstance,
                                    itemObMap.Key,
                                    rs,
                                    appRequest,
                                    uid,
                                    dataMap,
                                    learnApiData,
                                    isO9Mapping
                                );
                                obMapp[i] = itemObMapInstance;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("LoopConfigMapping exception()==" + ex.StackTrace);
        }

        return obMapp;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected async Task<int> CallMapS(
        JObject obMapp,
        string mapKey,
        string para,
        string app,
        string uid,
        JObject dataMap,
        string learnApiData,
        bool isO9Mapping = false
    )
    {
        return await CallMapSAsync(
            obMapp,
            mapKey,
            para,
            app,
            uid,
            dataMap,
            learnApiData,
            isO9Mapping
        );
    }

    /// <summary>
    /// Describes whether is dot separated
    /// </summary>
    private static bool IsDotSeparated(string input)
    {
        int dotCount = 0;
        foreach (char c in input)
        {
            if (c == '.')
            {
                dotCount++;
                if (dotCount > 1)
                {
                    return false;
                }
            }
        }

        return dotCount == 1;
    }

    /// <summary>
    /// Describes whether is all upper
    /// </summary>
    private static bool IsAllUpper(string input)
    {
        input = new string(input.Where(char.IsLetter).ToArray());

        return !string.IsNullOrEmpty(input) && input.All(char.IsUpper);
    }

    /// <summary>
    /// Converts the to lower case using the specified input
    /// </summary>
    private static string ConvertToLowerCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        if (input.Contains('{') || input.Contains('['))
        {
            return input;
        }

        bool containsUppercase = false;
        Parallel.For(
            0,
            input.Length,
            (i, state) =>
            {
                if (char.IsUpper(input[i]))
                {
                    containsUppercase = true;
                    state.Stop();
                }
            }
        );

        if (!containsUppercase)
        {
            return input;
        }

        if (input.Length > 100)
        {
            return input;
        }

        if (IsAllUpper(input))
        {
            return input.ToLower();
        }

        if (IsDotSeparated(input))
        {
            int dotIndex = input.IndexOf('.');
            if (dotIndex >= 0 && dotIndex < input.Length - 1)
            {
                string result =
                    input.Substring(0, dotIndex + 1) + input.Substring(dotIndex + 1).ToLower();
                return result;
            }
        }

        return input;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected async Task<int> CallMapSAsync(
        JObject obMapp,
        string mapKey,
        string para,
        string app,
        string uid,
        JObject dataMap,
        string learnApiData,
        bool isO9Mapping = false
    )
    {
        var requestJson = Context.InfoRequest.GetRequestJson();
        int startIndexFunc = 0;
        string function = para[startIndexFunc..para.IndexOf("(")];

        int startIndexFunctionPara = para.IndexOf("(") + 1;
        string functionPara = para[startIndexFunctionPara..para.LastIndexOf(")")];

        if (startIndexFunctionPara == para.LastIndexOf(")"))
        {
            functionPara = learnApiData + "." + mapKey;
        }

        //if (isO9Mapping) functionPara = ConvertToLowerCase(functionPara);

        try
        {
            if (_mappingFunctions.TryGetValue(function, out var mappingFunction))
            {
                obMapp[mapKey] = await mappingFunction(
                    function,
                    functionPara,
                    mapKey,
                    dataMap,
                    obMapp,
                    app,
                    uid,
                    learnApiData
                );
            }
            else
            {
                Console.WriteLine($"Unknown function: {function}");
                obMapp[mapKey] = null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("MapS exception == " + ex.StackTrace);
            return -1;
        }

        return 1;
    }

    /// <summary>
    /// Gets the token using the specified function
    /// </summary>
    private async Task<JToken> GetTokenAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var userLogin = Context.InfoUser.GetUserLogin();
        return functionPara.Equals("")
            ? userLogin.PortalToken.ToString()
            : functionPara + userLogin.PortalToken;
    }

    /// <summary>
    /// Strings the split array json using the specified function
    /// </summary>
    private async Task<JToken> StringSplitArrayJsonAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        string[] para_stringSplitArrayJson = functionPara.Split(separator: "|");
        string para_0 = para_stringSplitArrayJson[0];
        string para_1 = para_stringSplitArrayJson[1];
        var arr_json = dataMap.SelectToken(para_0);
        if (arr_json != null)
        {
            try
            {
                string result = "";
                foreach (var item in arr_json.ToJArray())
                {
                    result += item.SelectToken(para_1).ToString() + ";";
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("stringSplitArrayJson Error ===" + ex.ToString());
                return null;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the data json for key of context using the specified data key
    /// </summary>
    private static JToken GetDataJsonForKeyOfContext(string data_key, JObject data)
    {
        if (data_key.Contains('.'))
        {
            string[] s_ = data_key.Split(separator: ".");
            if (s_.Length > 0)
            {
                var ob_loop = data;
                for (int i = 0; i < s_.Length; i++)
                {
                    if (i == (s_.Length - 1))
                    {
                        var a = s_[i];
                        return ob_loop.GetValue(s_[i]);
                    }
                    else if (ob_loop.ContainsKey(s_[i]))
                    {
                        ob_loop = ob_loop.GetValue(s_[i]).ToJObject();
                    }
                }
            }
        }
        else
        {
            if (data.ContainsKey(data_key))
            {
                return data.GetValue(data_key);
            }
        }

        return null;
    }

    /// <summary>
    /// Arrays the json using the specified function
    /// </summary>
    private async Task<JToken> ArrayJsonAsync(
        string function,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp,
        string app,
        string uid,
        string learnApiData
    )
    {
        string[] paraConvert = functionPara.Split(separator: "|");
        string paraData = paraConvert[0];
        string paraArr = paraConvert[1];

        var data_arr = GetDataJsonForKeyOfContext(paraData, dataMap);
        if (data_arr != null)
        {
            if (data_arr is JArray)
            {
                JArray data_arr_is_array = data_arr.ToJArray();
                JArray data_result = [];
                for (int i = 0; i < data_arr_is_array.Count; i++)
                {
                    var data_elm_arr = data_arr_is_array[i];
                    var json_config_elm_arr = JToken.Parse(paraArr).DeepClone().ToJObject();
                    json_config_elm_arr = await LoopConfigMapping(
                        json_config_elm_arr,
                        app,
                        uid,
                        data_elm_arr.ToJObject(),
                        learnApiData
                    );
                    data_result.Add(json_config_elm_arr);
                }

                return data_result;
            }
        }

        return new JArray();
    }

    /// <summary>
    /// Merges the data with json using the specified function
    /// </summary>
    private async Task<JToken> MergeDataWithJsonAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        string[] para_json = functionPara.Split(separator: "|");
        string json1Key = para_json[0];
        string json2Config = para_json[1];

        var json1 = GetDataJsonForKeyOfContext(json1Key, dataMap);
        var json2_config = JToken.Parse(json2Config).DeepClone().ToJObject();
        var json2 = await LoopConfigMapping(json2_config, app_, uid, dataMap, learnApiData);
        var new_json = new JObject();
        if (json1 != null && json2.ToSerialize() != json2Config)
        {
            new_json = JObject.FromObject(
                Utils.Utils.MergeDictionary(json1.ToDictionary(), json2.ToDictionary())
            );
        }

        return new_json;
    }

    /// <summary>
    /// Arrays the dynamic using the specified function
    /// </summary>
    private async Task<JToken> ArrayDynamicAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        return dataMap.SelectToken(functionPara);
    }

    /// <summary>
    /// Arrays the number using the specified function
    /// </summary>
    private async Task<JToken> ArrayNumberAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        string[] para_convert_number = functionPara.Split(separator: "|");
        string para_data_number = para_convert_number[0];
        string para_arr_number = para_convert_number[1];

        var data_arr_number = GetDataJsonForKeyOfContext(para_data_number, dataMap);
        if (data_arr_number != null)
        {
            if (data_arr_number is JArray)
            {
                JArray data_result = new JArray();
                var data_arr_is_array = data_arr_number.ToJArray();
                for (int i = 0; i < data_arr_is_array.Count; i++)
                {
                    var data_elm_arr = data_arr_is_array[i];
                    if (data_elm_arr.ToJObject() != null)
                    {
                        data_result.Add(data_elm_arr.SelectToken(para_arr_number));
                    }
                    else
                    {
                        data_result.Add(data_elm_arr);
                    }
                }

                return data_result;
            }
        }

        return new JArray();
    }

    /// <summary>
    /// Formats the date using the specified function
    /// </summary>
    private async Task<JToken> FormatDateAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (!string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            long date1 = long.Parse(dataMap.SelectToken(functionPara).ToString());
            string format = "yyyy-MM-dd"; // Default format

            if (function_ == "date_YYYY-MM-DD")
            {
                format = "yyyy/MM/dd";
            }
            else if (function_ == "dateDD-MM-YYYY")
            {
                format = "dd-MM-yyyy";
            }
            // Add more conditions for other date formats if needed
            var date = Utils.Utils.FormatUnixTimestamp(date1, format, true);
            return date;
        }

        return null;
    }

    /// <summary>
    /// Formats the date null using the specified function
    /// </summary>
    private async Task<JToken> FormatDateNullAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (!string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            long date1 = long.Parse(dataMap.SelectToken(functionPara).ToString());
            return Utils
                .Utils.ConvertFromUnixTimestampMillisecond(date1, true)
                .ToString("dd-MM-yyyy");
        }

        return null;
    }

    /// <summary>
    /// Formats the custom date using the specified function
    /// </summary>
    private async Task<JToken> FormatCustomDateAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (!string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            string dataDate = dataMap
                .SelectToken(functionPara)
                .ToString(Formatting.Indented)
                .Trim('"');
            DateTime date1;
            if (dataDate.Contains('|'))
            {
                var dataArray = dataDate.Split('|');
                date1 = DateTime.ParseExact(
                    dataArray[0],
                    dataArray[1],
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
                );
            }
            else
            {
                date1 = DateTime.ParseExact(
                    dataDate.ToString(),
                    DateTimeFormat.DateFormats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
                );
            }

            return date1.ToString("dd/MM/yyyy");
        }

        return null;
    }

    /// <summary>
    /// Formats the custom date null using the specified function
    /// </summary>
    private async Task<JToken> FormatCustomDateNullAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (!string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            string dataDate = dataMap
                .SelectToken(functionPara)
                .ToString(Formatting.Indented)
                .Trim('"');
            DateTime date1;
            if (dataDate.Contains('|'))
            {
                var dataArray = dataDate.Split('|');
                date1 = DateTime.ParseExact(
                    dataArray[0],
                    dataArray[1],
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
                );
            }
            else
            {
                date1 = DateTime.ParseExact(
                    dataDate.ToString(),
                    DateTimeFormat.DateFormats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
                );
            }

            return date1.ToString("dd/MM/yyyy");
        }

        return null;
    }

    /// <summary>
    /// Formats the custom date 2 using the specified function
    /// </summary>
    private async Task<JToken> FormatCustomDate2Async(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (!string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            string dataDate = dataMap
                .SelectToken(functionPara)
                .ToString(Formatting.Indented)
                .Trim('"');
            DateTime date1;
            if (dataDate.Contains('|'))
            {
                var dataArray = dataDate.Split('|');
                date1 = DateTime.ParseExact(
                    dataArray[0],
                    dataArray[1],
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
                );
            }
            else
            {
                date1 = DateTime.ParseExact(
                    dataDate.ToString(),
                    DateTimeFormat.DateFormats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
                );
            }

            return date1.ToString("yyyy/MM/dd");
        }

        return null;
    }

    /// <summary>
    /// Formats the custom date 2 null using the specified function
    /// </summary>
    private async Task<JToken> FormatCustomDate2NullAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (!string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            string dataDate = dataMap
                .SelectToken(functionPara)
                .ToString(Formatting.Indented)
                .Trim('"');
            DateTime date1;
            if (dataDate.Contains('|'))
            {
                var dataArray = dataDate.Split('|');
                date1 = DateTime.ParseExact(
                    dataArray[0],
                    dataArray[1],
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
                );
            }
            else
            {
                date1 = DateTime.ParseExact(
                    dataDate.ToString(),
                    DateTimeFormat.DateFormats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
                );
            }

            return date1.ToString("yyyy/MM/dd");
        }

        return null;
    }

    /// <summary>
    /// Gets the time using the specified function
    /// </summary>
    private async Task<JToken> GetTimeAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (!string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            var date1 = O9Utils.ConvertDateStringToLong(
                dataMap.SelectToken(functionPara).ToString()
            );
            return date1;
        }

        return null;
    }

    /// <summary>
    ///
    /// </summary>
    private async Task<JToken> GetDateTimeByFormat(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var dataArray = functionPara.Split('|');
        if (!string.IsNullOrEmpty(dataMap.SelectToken(dataArray[0])?.ToString()))
        {
            string dataDate = dataMap
                .SelectToken(dataArray[0])
                .ToString(Formatting.Indented)
                .Trim('"');
            var date1 = DateTime.ParseExact(
                dataDate,
                dataArray[1],
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
            );
            return date1.ToString(dataArray[2]);
        }

        return null;
    }

    /// <summary>
    /// Arrays the string using the specified function
    /// </summary>
    private async Task<JToken> ArrayStringAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        string[] para_convert_string = functionPara.Split(separator: "|");
        string para_data_string = para_convert_string[0];
        string para_arr_string = para_convert_string[1];

        var data_arr_string = dataMap.SelectToken(para_data_string);
        if (data_arr_string != null)
        {
            if (data_arr_string is JArray)
            {
                JArray data_result = [];
                var data_arr_is_array = data_arr_string.ToJArray();
                for (int i = 0; i < data_arr_is_array.Count; i++)
                {
                    var data_elm_arr = data_arr_is_array[i];
                    if (Utils.Utils.IsValidJsonObject(data_elm_arr.ToSerialize()))
                    {
                        data_result.Add(data_elm_arr.SelectToken(para_arr_string));
                    }
                    else
                    {
                        data_result.Add(data_elm_arr);
                    }
                }

                return data_result;
            }
            else
            {
                JArray data_result = new JArray();
                if (Utils.Utils.IsValidJsonObject(data_arr_string.ToSerialize()))
                {
                    data_result.Add(data_arr_string.SelectToken(para_arr_string));
                }

                return data_result;
            }
        }

        return new JArray();
    }

    /// <summary>
    /// Data s the sum column using the specified function
    /// </summary>
    private async Task<JToken> DataSumColumnAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        string[] para_convert_num_sum = functionPara.Split("|");
        string para_data_sum = para_convert_num_sum[0];
        string para_arr_sum = para_convert_num_sum[1];

        var data_arr_sum = dataMap.SelectToken(para_data_sum);
        try
        {
            if (data_arr_sum != null)
            {
                if (data_arr_sum is JArray)
                {
                    double data_result = 0;
                    var data_arr_is_array = data_arr_sum.ToJArray();
                    for (int i = 0; i < data_arr_is_array.Count; i++)
                    {
                        var data_elm_arr = data_arr_is_array[i];
                        Console.WriteLine(
                            "=====data_elm_arr=====" + JsonConvert.SerializeObject(data_elm_arr)
                        );
                        if (data_elm_arr.ToJObject() != null)
                        {
                            data_result += double.Parse(
                                data_elm_arr.SelectToken(para_arr_sum).ToString()
                            );
                        }
                        else
                        {
                            data_result += double.Parse(data_elm_arr.ToString());
                        }
                    }

                    return data_result;
                }
                else
                {
                    double data_result = 0;
                    if (Utils.Utils.IsValidJsonObject(data_arr_sum.ToSerialize()))
                    {
                        data_result += double.Parse(
                            data_arr_sum.SelectToken(para_arr_sum).ToString()
                        );
                    }

                    return data_result;
                }
            }

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("dataSumColumn err ==" + ex.StackTrace);
            return null;
        }
    }

    /// <summary>
    /// Gets the data boolean using the specified function
    /// </summary>
    private async Task<JToken> GetDataBooleanAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        return dataMap.SelectToken(functionPara) != null && (bool)dataMap.SelectToken(functionPara);
    }

    private async Task<JToken> GetDataNZeroAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        string rsNZero = "";
        try
        {
            var data_ = dataMap.SelectToken(functionPara);
            if (data_ != null && !data_.ToString().Equals(""))
            {
                rsNZero = data_.ToString();
            }

            if (!rsNZero.Equals(""))
            {
                return double.Parse(rsNZero, CultureInfo.InvariantCulture);
            }

            return 0;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /// <summary>
    /// Gets the data n zero sub 1 using the specified function
    /// </summary>
    private async Task<JToken> GetDataNZeroSub1Async(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        string rsNZeroSub = "";
        if (!dataMap.SelectToken(functionPara).ToString().Equals(""))
        {
            rsNZeroSub = dataMap.SelectToken(functionPara).ToString();
        }

        if (!rsNZeroSub.Equals(""))
        {
            return double.Parse(rsNZeroSub, CultureInfo.InvariantCulture) - 1;
        }

        return 0.0;
    }

    /// <summary>
    /// Gets the data i zero sub 1 using the specified function
    /// </summary>
    private async Task<JToken> GetDataIZeroSub1Async(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        string rsIZeroSub = "";
        if (!dataMap.SelectToken(functionPara).ToString().Equals(""))
        {
            rsIZeroSub = dataMap.SelectToken(functionPara).ToString();
        }

        if (!rsIZeroSub.Equals(""))
        {
            return int.Parse(rsIZeroSub) - 1;
        }

        return 0;
    }

    /// <summary>
    /// Gets the page size data default using the specified function
    /// </summary>
    private async Task<JToken> GetPageSizeDataDefaultAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        string rsPageSize = "";
        if (!dataMap.SelectToken(functionPara).ToString().Equals(""))
        {
            rsPageSize = dataMap.SelectToken(functionPara).ToString();
        }

        if (!rsPageSize.Equals(""))
        {
            return int.Parse(rsPageSize);
        }

        return 5;
    }

    /// <summary>
    /// Gets the data select null using the specified function
    /// </summary>
    private async Task<JToken> GetDataSelectNullAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var dataSelect = dataMap.SelectToken(functionPara).ToString();
        if (dataSelect.Equals("select_null") || dataSelect.Equals("All") || dataSelect.Equals("-"))
        {
            return "";
        }

        return dataSelect;
    }

    /// <summary>
    /// Gets the data select one null using the specified function
    /// </summary>
    private async Task<JToken> GetDataSelectOneNullAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        try
        {
            if (string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
            {
                return null;
            }

            return dataMap.SelectToken(functionPara);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the data select i null using the specified function
    /// </summary>
    private async Task<JToken> GetDataSelectINullAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var dataSelectINull = dataMap.SelectToken(functionPara).ToString();
        if (dataSelectINull.Equals("select_null"))
        {
            return null;
        }

        return int.Parse(dataSelectINull);
    }

    /// <summary>
    /// Gets the data s using the specified function
    /// </summary>
    private async Task<JToken> GetDataSAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            return string.Empty;
        }

        return dataMap.SelectToken(functionPara);
    }

    /// <summary>
    /// Gets the data s null using the specified function
    /// </summary>
    private async Task<JToken> GetDataSNullAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            return null;
        }

        return dataMap.SelectToken(functionPara);
    }

    /// <summary>
    /// Gets the data s trim using the specified function
    /// </summary>
    private async Task<JToken> GetDataSTrimAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var selectedToken = dataMap.SelectToken(functionPara)?.ToString()?.Trim();
        if (string.IsNullOrEmpty(selectedToken))
        {
            return "";
        }

        return selectedToken;
    }

    /// <summary>
    /// Gets the data s default using the specified function
    /// </summary>
    private Task<JToken> GetDataSDefaultAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        var param = functionPara.Split('|');
        var selectedToken = dataMap.SelectToken(param[0])?.ToString()?.Trim();

        return Task.FromResult(
            selectedToken.HasValue() ? selectedToken : JToken.FromObject(param[1])
        );
    }

    private async Task<JToken> AESEncrypt(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var selectedToken = dataMap.SelectToken(functionPara)?.ToString()?.Trim();
        if (string.IsNullOrEmpty(selectedToken))
        {
            return "";
        }

        return O9Encrypt.AESEncrypt(selectedToken);
    }

    /// <summary>
    /// Gets the data n using the specified function
    /// </summary>
    private async Task<JToken> GetDataNAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            return 0;
        }
        else
        {
            var tokenMap = dataMap.SelectToken(functionPara);
            // var stringMap = tokenMap?.ToString();
            var valueMap = Convert.ToDouble(tokenMap);
            return valueMap;
        }
    }

    /// <summary>
    /// Gets the data l using the specified function
    /// </summary>
    private async Task<JToken> GetDataLAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            return 0;
        }

        return long.Parse(dataMap.SelectToken(functionPara)?.ToString());
    }

    /// <summary>
    /// Gets the data i using the specified function
    /// </summary>
    private async Task<JToken> GetDataIAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            return 0;
        }

        return int.Parse(dataMap.SelectToken(functionPara)?.ToString());
    }

    /// <summary>
    /// Gets the data i null using the specified function
    /// </summary>
    private async Task<JToken> GetDataINullAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            return null;
        }

        return int.Parse(dataMap.SelectToken(functionPara)?.ToString());
    }

    /// <summary>
    /// Gets the data i zero using the specified function
    /// </summary>
    private async Task<JToken> GetDataIZeroAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        try
        {
            if (string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
            {
                return 0;
            }

            return int.Parse(dataMap.SelectToken(functionPara)?.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return 0;
        }
    }

    /// <summary>
    /// Gets the data n null using the specified function
    /// </summary>
    private async Task<JToken> GetDataNNullAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        if (string.IsNullOrEmpty(dataMap.SelectToken(functionPara)?.ToString()))
        {
            return null;
        }

        return double.Parse(
            dataMap.SelectToken(functionPara)?.ToString(),
            CultureInfo.InvariantCulture
        );
    }

    /// <summary>
    /// Gets the data long number using the specified function
    /// </summary>
    private async Task<JToken> GetDataLongNumberAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var stringValue = dataMap.SelectToken(functionPara)?.ToString();
        Console.WriteLine("dataLongNumber====" + stringValue);
        if (string.IsNullOrEmpty(stringValue))
        {
            return 0;
        }

        if (decimal.TryParse(stringValue, out decimal decimalValue))
        {
            Console.WriteLine("dataLongNumber====decimalValue====" + decimalValue);
            return decimalValue;
        }

        return 0;
    }

    /// <summary>
    /// Gets the data json object using the specified function
    /// </summary>
    private async Task<JToken> GetDataJsonObjectAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var dataObj = dataMap.SelectToken(functionPara);
        if (dataObj != null)
        {
            return JObject.FromObject(dataObj);
        }

        return new JObject();
    }

    private async Task<JToken> GetDataObjectLowerKeyAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var dataObj = dataMap.SelectToken(functionPara);
        if (dataObj != null)
        {
            return JObject.FromObject(dataObj).ConvertToJObject();
        }

        return new JObject();
    }

    /// <summary>
    /// Gets the data json array using the specified function
    /// </summary>
    private async Task<JToken> GetDataJsonArrayAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var dataObjArray = dataMap.SelectToken(functionPara);
        if (dataObjArray != null)
        {
            return JArray.FromObject(dataObjArray);
        }

        return new JArray();
    }

    /// <summary>
    /// Adds the to array using the specified function
    /// </summary>
    private async Task<JToken> AddToArrayAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var value = dataMap.SelectToken(functionPara);

        if (
            obMapp_.TryGetValue(mapKey, out JToken existingValue)
            && existingValue is JArray existingArray
        )
        {
            existingArray.Add(value);
            return existingArray;
        }

        return new JArray(value);
    }

    /// <summary>
    /// Strings the split using the specified function
    /// </summary>
    private async Task<JToken> StringSplitAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var dataStringSplit = dataMap.SelectToken(functionPara).ToJArray();
        if (dataStringSplit != null)
        {
            string result = "";
            foreach (var item in dataStringSplit)
            {
                result += item.ToString() + ";";
            }

            return result;
        }

        return "";
    }

    /// <summary>
    /// Gets the info request using the specified function
    /// </summary>
    private async Task<JToken> GetInfoRequestAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        var userLogin = Context.InfoUser.GetUserLogin();
        await Task.CompletedTask;
        if (userLogin != null)
        {
            var dataKey = JObject.FromObject(userLogin).SelectToken(functionPara);
            if (dataKey != null)
            {
                return dataKey;
            }

            return JObject
                .FromObject(userLogin)
                .SelectToken(Utils.StringExtensions.ToTitleCase(functionPara));
        }

        return null;
    }

    /// <summary>
    /// Gets the data serialize json using the specified function
    /// </summary>
    private async Task<JToken> GetDataSerializeJsonAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var dataSerializeJson = dataMap.SelectToken(functionPara);
        if (dataSerializeJson != null)
        {
            return JObject.Parse(dataSerializeJson.ToString());
        }

        return new JObject();
    }

    /// <summary>
    /// Gets the serialize j array using the specified function
    /// </summary>
    private async Task<JToken> GetSerializeJArrayAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var dataSerializeJArray = dataMap.SelectToken(functionPara);
        if (dataSerializeJArray != null)
        {
            return JArray.Parse(dataSerializeJArray.ToString());
        }

        return new JArray();
    }

    /// <summary>
    /// Gets the user session using the specified function
    /// </summary>
    private async Task<JToken> GetUserSessionAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        var requestHeader = Context?.InfoUser.GetUserLogin();

        var user =
            SessionUtils.GetUserSession(Context)
            ?? await EngineContext
                .Current.Resolve<IStaticCacheManager>()
                .Get<UserSessions>(new CacheKey(requestHeader.Token));

        if (user != null)
        {
            var userType = user.GetType();
            var propertyMap = _propertyCache.GetOrAdd(
                userType,
                type => type.GetProperties().ToDictionary(p => p.Name.ToLower(), p => p)
            );

            if (propertyMap.TryGetValue(functionPara.ToLower(), out var property))
            {
                return property.GetValue(user).ToJToken();
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the user session using the specified function
    /// </summary>
    private async Task<JToken> GetSessionCoreAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var sessions = O9Client.CoreBankingSession.UUID;
        if (sessions != null)
        {
            return sessions;
        }
        return null;
    }

    /// <summary>
    /// Gets the user session using the specified function
    /// </summary>
    private async Task<JToken> GetDataHeader(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var requestHeader = Context?.InfoUser.GetUserLogin().ToJObject();
        var data = requestHeader.SelectToken(functionPara);
        if (data != null)
        {
            return data.ToString();
        }

        return null;
    }

    private async Task<JToken> UserInfoByToken(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        try
        {
            var token = Context?.InfoUser.GetUserLogin().Token;
            var output = token.Split('.')[1].Replace('-', '+').Replace('_', '/');
            switch (output.Length % 4)
            {
                case 2:
                    output += "==";
                    break;
                case 3:
                    output += "=";
                    break;
            }
            var encode = Encoding.UTF8.GetString(Convert.FromBase64String(output));
            var data = JObject.Parse(encode);
            if (!data.ContainsKey(functionPara))
            {
                return data;
            }
            return data.SelectToken(functionPara);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the query value using the specified function
    /// </summary>
    private async Task<JToken> GetQueryValueAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        var _baseService = EngineContext.Current.Resolve<IBaseService>();
        var listKey = functionPara.Split('|').ToList();
        var queryKey = listKey[0];
        listKey.RemoveAt(0);
        var listParam = new List<string>();
        foreach (var key in listKey)
        {
            var values = dataMap.SelectToken(key);
            if (values != null)
            {
                listParam.Add(values.ToString());
            }
        }

        var resultQuery = await _baseService.GetInfoByQuery<string>(queryKey, listParam);

        if (resultQuery.HasValue())
        {
            return resultQuery;
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the string object using the specified function
    /// </summary>
    private async Task<JToken> GetStringObjectAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var arr = functionPara.Split(".");
        if (!string.IsNullOrEmpty(arr[0]) && !string.IsNullOrEmpty(arr[1]))
        {
            var obj = dataMap.SelectToken(arr[0]);
            if (obj != null)
            {
                if (obj.IsNullOrEmpty())
                {
                    return string.Empty;
                }

                var jData = JObject.Parse(obj.ToString()).ConvertToJObject();

                return jData.SelectToken(arr[1]);
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the to serialize using the specified function
    /// </summary>
    private async Task<JToken> GetToSerializeAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var ToSerialize = dataMap.SelectToken(functionPara);
        if (ToSerialize != null)
        {
            return ToSerialize.ToSerialize();
        }
        else
        {
            return new JObject().ToSerialize();
        }
    }

    private async Task<JToken> JArrayAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        await Task.CompletedTask;
        var functions = functionPara.Split("|");
        var arrayData = dataMap.SelectToken(functions[0]);

        var result = new JArray();
        foreach (JObject item in arrayData.Cast<JObject>())
        {
            if (item[functions[1]] != null)
            {
                result.Add(item[functions[1]]);
            }
        }

        return await Task.FromResult<JToken>(result);
    }

    private async Task<JToken> GetSettingValueAsync(
        string function_,
        string functionPara,
        string mapKey,
        JObject dataMap,
        JObject obMapp_,
        string app_,
        string uid,
        string learnApiData
    )
    {
        var settingService = EngineContext.Current.Resolve<ICMSSettingService>();
        var setting = await settingService.GetStringValue(functionPara);

        return setting;
    }
}
