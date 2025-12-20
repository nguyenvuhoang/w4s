using System.Text.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

public class ValidationService : IValidationService
{
    private readonly IRaiseErrorService _raiseErrorService;

    private class GetValueResponse
    {
        public string Path { get; set; }
        public JToken Value { get; set; }
    }

    public ValidationService(IRaiseErrorService raiseErrorService)
    {
        _raiseErrorService = raiseErrorService;
    }

    public bool EvaluateCondition(JsonElement config, JsonElement data)
    {
        return EvaluateExpression(config, data);
    }

    private bool EvaluateExpression(JsonElement expression, JsonElement data)
    {
        if (
            expression.ValueKind != JsonValueKind.Object
            || !expression.TryGetProperty("func", out JsonElement function)
        )
        {
            return true;
        }

        if (function.ValueKind == JsonValueKind.Undefined)
        {
            return true;
        }

        JsonElement parameters = expression.GetProperty("paras");

        switch (function.GetString())
        {
            case "&&":
                return EvaluateAnd(parameters, data);
            case "||":
                return EvaluateOr(parameters, data);
            case "IsStringEqual":
                return EvaluateStringEqual(parameters, data);
            case "IsBoolEqual":
                return EvaluateBoolEqual(parameters, data);
            case "IsGreaterThan":
                return EvaluateGreaterThan(parameters, data);
            default:
                return false;
        }
    }

    private bool EvaluateAnd(JsonElement parameters, JsonElement data)
    {
        foreach (var param in parameters.EnumerateArray())
        {
            if (!EvaluateExpression(param, data))
            {
                return false;
            }
        }

        return true;
    }

    private bool EvaluateOr(JsonElement parameters, JsonElement data)
    {
        foreach (var param in parameters.EnumerateArray())
        {
            if (EvaluateExpression(param, data))
            {
                return true;
            }
        }

        return false;
    }

    private static bool EvaluateStringEqual(JsonElement parameters, JsonElement data)
    {
        string leftValue = GetValueFromPath<string>(parameters[0].GetString(), data);
        string rightValue = parameters[1].GetString();
        return leftValue == rightValue;
    }

    private static bool EvaluateGreaterThan(JsonElement parameters, JsonElement data)
    {
        double leftValue = GetValueFromPath<double>(parameters[0].GetString(), data);

        double rightValue = parameters[1].GetDouble();
        return leftValue > rightValue;
    }

    private static bool EvaluateBoolEqual(JsonElement parameters, JsonElement data)
    {
        var leftValue = GetValueFromPath<bool>(parameters[0].GetString(), data);
        bool rightValue = parameters[1].GetBoolean();
        return leftValue == rightValue;
    }

    private static T GetValueFromPath<T>(string path, JsonElement data)
    {
        var token = path.Trim();
        JsonElement current = data;
        current = current.GetProperty(token);

        if (typeof(T) == typeof(bool))
        {
            return (T)(object)current.GetBoolean();
        }

        if (typeof(T) == typeof(double))
        {
            return (T)(object)current.GetDouble();
        }

        if (typeof(T) == typeof(string))
        {
            return (T)(object)current.GetString();
        }

        throw new InvalidOperationException("Unsupported type");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jsonString"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task<bool> EvaluateCondition(string config, JObject data)
    {
        JObject condition = JObject.Parse(config);
        return await EvaluateExpression(condition, data);
    }

    private async Task<bool> EvaluateExpression(JObject expression, JObject data)
    {
        var func = expression["func"]?.ToString();
        var paras = (JArray)expression["paras"];
        var isSkip = expression["isskip"] == null || bool.Parse(expression["isskip"].ToString());
        switch (func)
        {
            case "&&":
                var andResults = await Task.WhenAll(
                    paras.Select(async para => await EvaluateExpression((JObject)para, data))
                );
                return andResults.All(result => result);
            case "||":
                var orResults = await Task.WhenAll(
                    paras.Select(async para => await EvaluateExpression((JObject)para, data))
                );
                return orResults.Any(result => result);
            case "IsStringEqual":
                return await IsStringEqual(paras[0].ToString(), paras[1].ToString(), data, isSkip);
            case "IsNotNull":
                return await IsNotNull(paras[0].ToString(), data, isSkip);
            case "IsNotNullNotEmpty":
                return await IsNotNullNotEmpty(paras[0].ToString(), data, isSkip);
            case "LessThan":
                return await EvaluateLessThan(paras, data, isSkip);
            case "GreaterThan":
                return await EvaluateGreaterThan(paras, data, isSkip);
            default:
                return true;
        }
    }

    private async Task<bool> IsStringEqual(
        string path1,
        string path2,
        JObject data,
        bool isSkip = true
    )
    {
        var value1 = GetValue(path1, data).Value;
        var value2 = GetValue(path2, data).Value;

        var result = string.Equals(value1, value2);
        if (!result && !isSkip)
        {
            throw await _raiseErrorService.RaiseErrorWithKeyResource(
                "Common.Value.NotStringEqual",
                value1.Path,
                value1.Path
            );
        }

        return result;
    }

    private static GetValueResponse GetValue(string path, JObject data)
    {
        if (path.StartsWith("$"))
        {
            string jsonPath = path.StartsWith("$.") ? path.Substring(2) : path.Substring(1);

            JToken token = data.SelectToken(jsonPath);
            return new GetValueResponse() { Value = token, Path = jsonPath };
        }
        else
        {
            return new GetValueResponse() { Value = path, Path = path };
        }
    }

    private async Task<bool> IsNotNull(string path, JObject data, bool isSkip = true)
    {
        var token = GetValue(path, data);
        var result = token != null && token.Value.Type != JTokenType.Null;
        if (!result && !isSkip)
        {
            throw await _raiseErrorService.RaiseErrorWithKeyResource(
                "Common.Value.NotStringNull",
                token?.Path
            );
        }

        return result;
    }

    private async Task<bool> IsNotNullNotEmpty(string path, JObject data, bool isSkip = true)
    {
        var token = GetValue(path, data);
        var result =
            token != null
            && token.Value?.Type != JTokenType.Null
            && !string.IsNullOrEmpty(token.Value?.ToString());
        if (!result && !isSkip)
        {
            throw await _raiseErrorService.RaiseErrorWithKeyResource(
                "Common.Value.NotStringNullEmpty",
                token?.Path
            );
        }

        return result;
    }

    private async Task<bool> EvaluateGreaterThan(
        JArray parameters,
        JObject data,
        bool isSkip = true
    )
    {
        var leftValue = GetValue(parameters[0].ToString(), data);
        double rightValue = (double)parameters[1];
        var result = (double)leftValue.Value > rightValue;
        if (!result && !isSkip)
        {
            throw await _raiseErrorService.RaiseErrorWithKeyResource(
                "Common.Value.GreaterThan",
                leftValue?.Path,
                rightValue
            );
        }

        return result;
    }

    private async Task<bool> EvaluateLessThan(JArray parameters, JObject data, bool isSkip = true)
    {
        var leftValue = GetValue(parameters[0].ToString(), data);
        double rightValue = (double)parameters[1];
        var result = (double)leftValue.Value < rightValue;
        if (!result && !isSkip)
        {
            throw await _raiseErrorService.RaiseErrorWithKeyResource(
                "Common.Value.LessThan",
                leftValue?.Path,
                rightValue
            );
        }

        return result;
    }
}
