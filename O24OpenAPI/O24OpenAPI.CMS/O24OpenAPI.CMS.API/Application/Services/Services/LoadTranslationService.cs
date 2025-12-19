using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.CMS.Infrastructure.Configurations;
using System.Text.Json;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

public class LoadTranslationService(TranslationSettings translationSetting) : ITranslationService
{
    private readonly TranslationSettings _translationSetting = translationSetting;

    public List<TranslationEntry> Load()
    {
        Dictionary<string, string> languages = new()
        {
            { "en", "English" },
            { "vi", "Vietnamese" },
            { "lo", "Lao" },
            { "zh", "Chinese" },
        };

        HashSet<string> keySet = new();
        Dictionary<string, Dictionary<string, string>> langData = new();

        foreach (var lang in languages.Keys)
        {
            try
            {
                var filePath = Path.Combine(_translationSetting.BasePath, $"{lang}.json");

                if (!File.Exists(filePath))
                {
                    langData[lang] = [];
                    continue;
                }

                var content = File.ReadAllText(filePath);
                JsonDocument jsonDoc = JsonDocument.Parse(content);
                var dict = FlattenJson(jsonDoc.RootElement);

                langData[lang] = dict ?? [];

                foreach (var key in langData[lang].Keys)
                {
                    keySet.Add(key);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error loading translation file for language '{lang}': {ex.Message}",
                    ex
                );
            }
        }

        return keySet
            .Select(key => new TranslationEntry
            {
                Key = key,
                English = langData["en"].GetValueOrDefault(key),
                Vietnamese = langData["vi"].GetValueOrDefault(key),
                Lao = langData["lo"].GetValueOrDefault(key),
                Chinese = langData["zh"].GetValueOrDefault(key),
            })
            .ToList();
    }

    //public byte[] ExportToExcel(List<TranslationEntry> translations)
    //{
    //    ExcelPackage.License.SetNonCommercialPersonal("BEN");

    //    try
    //    {
    //        using var package = new ExcelPackage();
    //        var ws = package.Workbook.Worksheets.Add("Translations");

    //        // Header
    //        ws.Cells[1, 1].Value = "Key";
    //        ws.Cells[1, 2].Value = "English";
    //        ws.Cells[1, 3].Value = "Vietnamese";
    //        ws.Cells[1, 4].Value = "Lao";
    //        ws.Cells[1, 5].Value = "Chinese";

    //        // Body
    //        for (int i = 0; i < translations.Count; i++)
    //        {
    //            var t = translations[i];

    //            // Optional: check for null
    //            ws.Cells[i + 2, 1].Value = t.Key ?? "";
    //            ws.Cells[i + 2, 2].Value = t.English ?? "";
    //            ws.Cells[i + 2, 3].Value = t.Vietnamese ?? "";
    //            ws.Cells[i + 2, 4].Value = t.Lao ?? "";
    //            ws.Cells[i + 2, 5].Value = t.Chinese ?? "";
    //        }

    //        return package.GetAsByteArray();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("ExportToExcel failed: " + ex.Message, ex);
    //    }
    //}

    private static Dictionary<string, string> FlattenJson(
        JsonElement jsonElement,
        string parentKey = ""
    )
    {
        Dictionary<string, string> result = new();

        foreach (var property in jsonElement.EnumerateObject())
        {
            string key = string.IsNullOrEmpty(parentKey)
                ? property.Name
                : $"{parentKey}.{property.Name}";

            if (property.Value.ValueKind == JsonValueKind.Object)
            {
                var nested = FlattenJson(property.Value, key);
                foreach (var item in nested)
                {
                    result[item.Key] = item.Value;
                }
            }
            else if (property.Value.ValueKind == JsonValueKind.String)
            {
                result[key] = property.Value.GetString();
            }
        }

        return result;
    }

    //public List<TranslationEntry> ImportFromExcel(Stream fileStream)
    //{
    //    ExcelPackage.License.SetNonCommercialPersonal("jits");
    //    using var package = new ExcelPackage(fileStream);
    //    var ws = package.Workbook.Worksheets[0];

    //    var entries = new List<TranslationEntry>();
    //    var rowCount = ws.Dimension.Rows;

    //    for (int row = 2; row <= rowCount; row++)
    //    {
    //        entries.Add(
    //            new TranslationEntry
    //            {
    //                Key = ws.Cells[row, 1].Text.Trim(),
    //                English = ws.Cells[row, 2].Text.Trim(),
    //                Vietnamese = ws.Cells[row, 3].Text.Trim(),
    //                Lao = ws.Cells[row, 4].Text.Trim(),
    //                Chinese = ws.Cells[row, 5].Text.Trim(),
    //            }
    //        );
    //    }

    //    return entries;
    //}

    public async Task SaveTranslationsToJsonAsync(List<TranslationEntry> entries)
    {
        Dictionary<string, string> en = new();
        Dictionary<string, string> vi = new();
        Dictionary<string, string> lo = new();
        Dictionary<string, string> zh = new();

        foreach (var e in entries)
        {
            if (!string.IsNullOrEmpty(e.Key))
            {
                en[e.Key] = e.English ?? "";
                vi[e.Key] = e.Vietnamese ?? "";
                lo[e.Key] = e.Lao ?? "";
                zh[e.Key] = e.Chinese ?? "";
            }
        }

        var basePath = _translationSetting.BasePath;
        JsonSerializerOptions options = new() { WriteIndented = true };

        await WriteUnflattenedJsonAsync(Path.Combine(basePath, "en.json"), en, options);
        await WriteUnflattenedJsonAsync(Path.Combine(basePath, "vi.json"), vi, options);
        await WriteUnflattenedJsonAsync(Path.Combine(basePath, "lo.json"), lo, options);
        await WriteUnflattenedJsonAsync(Path.Combine(basePath, "zh.json"), zh, options);
    }

    private static async Task WriteUnflattenedJsonAsync(
        string path,
        Dictionary<string, string> flat,
        JsonSerializerOptions options
    )
    {
        var nested = Unflatten(flat);
        var json = JsonSerializer.Serialize(nested, options);
        await File.WriteAllTextAsync(path, json);
    }

    public static JsonElement Unflatten(Dictionary<string, string> flat)
    {
        Dictionary<string, object> root = new();

        foreach (var kv in flat)
        {
            var parts = kv.Key.Split('.');
            var current = root;

            for (int i = 0; i < parts.Length - 1; i++)
            {
                var part = parts[i];

                if (!current.ContainsKey(part))
                {
                    current[part] = new Dictionary<string, object>();
                }

                current = (Dictionary<string, object>)current[part];
            }

            current[parts[^1]] = kv.Value;
        }

        var json = JsonSerializer.Serialize(
            root,
            new JsonSerializerOptions { WriteIndented = true }
        );
        return JsonDocument.Parse(json).RootElement;
    }
}
