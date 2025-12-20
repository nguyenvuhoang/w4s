namespace O24OpenAPI.CMS.API.Application.Utils;

public static class DictionaryExtensions
{
    public static bool HasValue(this Dictionary<string, object> dictionary)
    {
        return dictionary != null && dictionary.Count != 0;
    }
}
