using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Services;

public static class ConditionService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public static bool CheckCondition(this WorkflowRequestModel workflow)
    {
        try
        {
            if (workflow.ObjectField?.TryGetValue("condition", out var conditionToken) != true)
            {
                return true;
            }

            var condition = conditionToken as JObject
                ?? JObject.Parse(conditionToken.ToString());

            if (condition["expression"] is not JObject expressionObj)
            {
                return true;
            }

            if (!EvaluateExpression(expressionObj, JObject.FromObject(workflow.ObjectField)))
            {
                var messageError = expressionObj["message_error"];
                if (messageError != null && !string.IsNullOrEmpty(messageError.ToString()))
                {
                    throw new InvalidOperationException(messageError.ToString());
                }
                return false;
            }

            return true;
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            Console.WriteLine(ex.StackTrace);
            throw new Exception($"Error in CheckCondition: {ex.Message}", ex);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="jsonString"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool CheckCondition(string jsonString, JObject data)
    {
        JObject condition = JObject.Parse(jsonString);
        return EvaluateExpression((JObject)condition["expression"], data);
    }

    private static bool EvaluateExpression(JObject expression, JObject data)
    {
        string func = expression["func"].ToString();
        JArray paras = (JArray)expression["paras"];

        switch (func)
        {
            case "&&":
                return paras.All(para => EvaluateExpression((JObject)para, data));
            case "||":
                return paras.Any(para => EvaluateExpression((JObject)para, data));
            case "IsStringEqual":
                return IsStringEqual(paras[0].ToString(), paras[1].ToString(), data);
            case "IsNotNull":
                return IsNotNull(paras[0].ToString(), data);
            case "IsNotNullNotEmpty":
                return IsNotNullNotEmpty(paras[0].ToString(), data);
            default:
                throw new Exception($"Unsupported function: {func}");
        }
    }


    private static bool IsStringEqual(string path1, string path2, JObject data)
    {
        string value1 = GetValue(path1, data);
        string value2 = GetValue(path2, data);

        return string.Equals(value1, value2);
    }

    private static string GetValue(string path, JObject data)
    {
        if (path.StartsWith("$"))
        {
            string jsonPath = path.StartsWith("$.") ? path.Substring(2) : path.Substring(1);

            JToken token = data.SelectToken(jsonPath);
            return token?.ToString();
        }
        else
        {
            return path;
        }
    }

    private static bool IsNotNull(string path, JObject data)
    {
        JToken token = data.SelectToken(path);
        return token != null && token.Type != JTokenType.Null;
    }

    private static bool IsNotNullNotEmpty(string path, JObject data)
    {
        JToken token = data.SelectToken(path);
        return token != null && token.Type != JTokenType.Null && !string.IsNullOrEmpty(token.ToString());
    }
}