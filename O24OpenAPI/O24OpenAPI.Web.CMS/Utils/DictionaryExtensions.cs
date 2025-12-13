namespace O24OpenAPI.Web.CMS.Utils;

public static class DictionaryExtensions
{
    public static bool HasValue(this Dictionary<string, object> dictionary)
    {
        return dictionary != null && dictionary.Count != 0;
    }
}
