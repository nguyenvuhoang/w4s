using Stimulsoft.Report;

namespace O24OpenAPI.Web.Report.Utils;

public static class TypeUtils
{
    /// <summary>
    /// Chuyển đổi một chuỗi thành kiểu dữ liệu C# tương ứng.
    /// </summary>
    public static Type GetVariableType(string typeString)
    {
        return typeString?.Trim().ToLower() switch
        {
            "int" => typeof(int),
            "decimal" => typeof(decimal),
            "double" => typeof(double),
            "float" => typeof(float),
            "bool" => typeof(bool),
            "datetime" => typeof(DateTime),
            "datetime?" => typeof(DateTime?),
            "guid" => typeof(Guid),
            "stringlist" => typeof(StringList),
            _ => typeof(string) // Mặc định là string nếu không khớp
        };
    }
}
