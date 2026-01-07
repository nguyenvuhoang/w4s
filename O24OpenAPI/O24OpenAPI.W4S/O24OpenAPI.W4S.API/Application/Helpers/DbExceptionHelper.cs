using System.Data.Common;

namespace O24OpenAPI.W4S.API.Application.Helpers;

public static class DbExceptionHelper
{
    public static bool IsUniqueConstraintViolation(Exception ex)
    {
        if (ex == null) return false;

        foreach (var e in Flatten(ex))
        {
            if (IsSqlServerUniqueViolation(e)) return true;

            if (IsOracleUniqueViolation(e)) return true;

            if (e is DbException dbx && LooksLikeUniqueByMessage(dbx.Message)) return true;
        }

        return false;
    }

    private static bool IsSqlServerUniqueViolation(Exception e)
    {
        var typeName = e.GetType().FullName ?? string.Empty;
        if (!typeName.EndsWith(".SqlException", StringComparison.Ordinal)) return false;


        var numberProp = e.GetType().GetProperty("Number");
        if (numberProp?.PropertyType == typeof(int))
        {
            var number = (int)numberProp.GetValue(e)!;
            return number is 2627 or 2601;
        }

        var errorsProp = e.GetType().GetProperty("Errors");
        var errorsObj = errorsProp?.GetValue(e);
        if (errorsObj is System.Collections.IEnumerable errors)
        {
            foreach (var err in errors)
            {
                var nProp = err?.GetType().GetProperty("Number");
                if (nProp?.PropertyType == typeof(int))
                {
                    var n = (int)nProp.GetValue(err)!;
                    if (n is 2627 or 2601) return true;
                }
            }
        }

        return false;
    }

    private static bool IsOracleUniqueViolation(Exception e)
    {

        var typeName = e.GetType().FullName ?? string.Empty;
        if (!typeName.EndsWith(".OracleException", StringComparison.Ordinal)) return false;


        var numberProp = e.GetType().GetProperty("Number");
        if (numberProp?.PropertyType == typeof(int))
        {
            var number = (int)numberProp.GetValue(e)!;
            return number is 1 or 2299;
        }

        var msgProp = e.GetType().GetProperty("Message");
        var msg = msgProp?.GetValue(e) as string;
        return LooksLikeUniqueByMessage(msg);
    }

    private static bool LooksLikeUniqueByMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message)) return false;
        var m = message.ToUpperInvariant();

        if (m.Contains("CANNOT INSERT DUPLICATE KEY", StringComparison.Ordinal)) return true;
        if (m.Contains("VIOLATION OF UNIQUE KEY", StringComparison.Ordinal)) return true;
        if (m.Contains("VIOLATION OF PRIMARY KEY", StringComparison.Ordinal)) return true;

        if (m.Contains("ORA-00001", StringComparison.Ordinal)) return true;
        if (m.Contains("UNIQUE CONSTRAINT", StringComparison.Ordinal)) return true;

        return false;
    }

    private static Exception[] Flatten(Exception ex)
    {
        if (ex is AggregateException ae)
            return ae.Flatten().InnerExceptions.SelectMany(Flatten).Prepend(ae).ToArray();

        if (ex is System.Reflection.TargetInvocationException tie && tie.InnerException != null)
            return new[] { ex }.Concat(Flatten(tie.InnerException)).ToArray();

        if (ex.InnerException != null)
            return new[] { ex }.Concat(Flatten(ex.InnerException)).ToArray();

        return [ex];
    }
}
