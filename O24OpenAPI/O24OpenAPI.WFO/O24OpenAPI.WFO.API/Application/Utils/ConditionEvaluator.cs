using O24OpenAPI.WFO.API.Application.Models;
using System.Text.Json;

namespace O24OpenAPI.WFO.API.Application.Utils;

public static class ConditionEvaluator
{
    public static bool Evaluate(Dictionary<string, object> request, Condition condition)
    {
        if (!string.IsNullOrEmpty(condition.Logic))
        {
            List<bool> subResults = condition.Conditions.Select(c => Evaluate(request, c)).ToList();
            if (condition.Logic.Equals("AND", StringComparison.CurrentCultureIgnoreCase))
            {
                return subResults.All(r => r);
            }
            else if (condition.Logic.Equals("OR", StringComparison.CurrentCultureIgnoreCase))
            {
                return subResults.Any(r => r);
            }
            throw new ArgumentException($"Unsupported logic: {condition.Logic}");
        }

        object fieldValue = GetValueFromDictionary(request, condition.Field);

        object conditionValue =
            condition.Value is string str && str.StartsWith("$.")
                ? GetValueFromDictionary(request, str)
                : condition.Value;

        if (conditionValue is JsonElement element)
        {
            conditionValue = element.GetPrimitiveValue();
        }

        return Compare(fieldValue, condition.Operator, conditionValue);
    }

    private static object GetValueFromDictionary(Dictionary<string, object> dict, string field)
    {
        if (string.IsNullOrEmpty(field) || !field.StartsWith("$."))
        {
            throw new ArgumentException("Field must start with '$.'");
        }

        string key = field.Substring(2); // Bỏ "$."
        if (dict.TryGetValue(key, out object value))
        {
            return value;
        }
        return null;
    }

    private static object GetPrimitiveValue(this JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt64(out var l) ? l : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => throw new NotSupportedException($"Unsupported ValueKind: {element.ValueKind}"),
        };
    }

    private static bool Compare(object fieldValue, string operatorType, object conditionValue)
    {
        if (operatorType == "IsNullOrEmpty")
        {
            return fieldValue == null || (fieldValue is string str && string.IsNullOrEmpty(str));
        }
        if (operatorType == "NotNullOrEmpty")
        {
            return fieldValue != null && fieldValue is string str && !string.IsNullOrEmpty(str);
        }

        if (fieldValue == null)
        {
            return false;
        }

        if (fieldValue is string strValue && conditionValue is string strCondition)
        {
            return operatorType switch
            {
                "equals" or "==" => strValue == strCondition,
                ">" => string.Compare(strValue, strCondition) > 0,
                "<" => string.Compare(strValue, strCondition) < 0,
                ">=" => string.Compare(strValue, strCondition) >= 0,
                "<=" => string.Compare(strValue, strCondition) <= 0,
                "contains" => strValue.Contains(strCondition, StringComparison.OrdinalIgnoreCase),
                _ => throw new ArgumentException(
                    $"Unsupported operator for string: {operatorType}"
                ),
            };
        }
        else if (fieldValue is IComparable comparableValue && conditionValue != null)
        {
            var convertedValue = Convert.ChangeType(conditionValue, fieldValue.GetType());
            return operatorType switch
            {
                "equals" or "==" => comparableValue.CompareTo(convertedValue) == 0,
                ">" => comparableValue.CompareTo(convertedValue) > 0,
                "<" => comparableValue.CompareTo(convertedValue) < 0,
                ">=" => comparableValue.CompareTo(convertedValue) >= 0,
                "<=" => comparableValue.CompareTo(convertedValue) <= 0,
                _ => throw new ArgumentException($"Unsupported operator: {operatorType}"),
            };
        }

        throw new ArgumentException(
            $"Cannot compare types: {fieldValue?.GetType()} and {conditionValue?.GetType()}"
        );
    }
}
