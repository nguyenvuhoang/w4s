using O24OpenAPI.Core.Domain.Localization;

namespace O24OpenAPI.Web.CMS.Utils;

public class MigrationUtils
{
    public static List<LocaleStringResource> PrepareDataResource(
        string resourceName,
        string module,
        Dictionary<string, string> langMessage
    )
    {
        var list = new List<LocaleStringResource>();
        foreach (KeyValuePair<string, string> entry in langMessage)
        {
            var item = new LocaleStringResource
            {
                Language = entry.Key,
                ResourceName = $"{module}.{resourceName}",
                ResourceValue = entry.Value,
                ResourceCode = "",
            };
            list.Add(item);
        }
        return list;
    }

    public static List<LocaleStringResource> PrepareDataResources(
        string module,
        Dictionary<string, Dictionary<string, string>> langMessages
    )
    {
        var list = new List<LocaleStringResource>();
        foreach (KeyValuePair<string, Dictionary<string, string>> entry in langMessages)
        {
            var resources = PrepareDataResource(entry.Key, module, entry.Value);
            list.AddRange(resources);
        }
        return list;
    }
}
