using LinqToDB;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using System.Text.Json;

namespace O24OpenAPI.Web.CMS.Services.Services.Digital;

public class AppLanguageConfigService(
    IRepository<TranslationLanguages> translationLanguageRepository
) : IAppLanguageConfigService
{
    private readonly IRepository<TranslationLanguages> _translationLanguageRepository = translationLanguageRepository;

    public async Task<List<AppLanguageConfigResponseModel>> LoadAppLanguageAsync(AppLanguageConfigRequestModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var channelId = !string.IsNullOrWhiteSpace(model.RequestChannel)
            ? model.RequestChannel!
            : model.ChannelId;

        var result = await _translationLanguageRepository.Table
            .Where(s => s.ChannelId == channelId)
            .Select(s => new AppLanguageConfigResponseModel
            {
                JSONContent = s.JSONContent,
                Language = s.Language
            })
            .ToListAsync();

        return result;
    }

    public async Task<PagedResult<AppLanguageConfigResponseModel>> LoadAppLanguagePageAsync(
         AppLanguageConfigRequestModel model,
         CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(model);

        bool doPaging = model.PageIndex.HasValue && model.PageSize.HasValue &&
                        model.PageIndex.Value >= 0 && model.PageSize.Value > 0;

        var channelId = !string.IsNullOrWhiteSpace(model.RequestChannel)
            ? model.RequestChannel!
            : model.ChannelId;

        var allLangs = await _translationLanguageRepository.Table
            .Where(s => s.ChannelId == channelId)
            .Select(s => new AppLanguageConfigResponseModel
            {
                Language = s.Language,
                JSONContent = s.JSONContent
            })
            .ToListAsync(ct);

        if (model.Languages is { Length: > 0 })
        {
            var order = model.Languages;
            var set = new HashSet<string>(order.Select(x => x.ToLowerInvariant()));
            allLangs = [.. allLangs
                .Where(x => set.Contains((x.Language ?? "").ToLowerInvariant()))
                .OrderBy(x => Array.IndexOf(order, x.Language))];
        }

        if (allLangs.Count == 0)
        {
            return new PagedResult<AppLanguageConfigResponseModel>
            {
                Items = [],
                TotalItems = 0,
                PageIndex = doPaging ? model.PageIndex!.Value : 0,
                PageSize = doPaging ? model.PageSize!.Value : 0
            };
        }

        var langMaps = allLangs.ToDictionary(
            x => x.Language.ToLowerInvariant(),
            x => SafeFlatten(x.JSONContent),
            StringComparer.OrdinalIgnoreCase);

        var allKeys = new HashSet<string>(langMaps.SelectMany(m => m.Value.Keys), StringComparer.Ordinal);

        var q = (model.Search ?? "").Trim();
        if (!string.IsNullOrEmpty(q))
        {
            var qLower = q.ToLowerInvariant();
            allKeys = allKeys
                .Where(k =>
                    k.ToLowerInvariant().Contains(qLower) ||
                    langMaps.Values.Any(m => m.TryGetValue(k, out var v) && (v ?? "").ToLowerInvariant().Contains(qLower)))
                .ToHashSet(StringComparer.Ordinal);
        }

        var orderedKeys = allKeys.OrderBy(k => k, StringComparer.Ordinal).ToList();

        int total = orderedKeys.Count;
        int pageIndex = doPaging ? model.PageIndex!.Value : 0;
        int pageSize = doPaging ? model.PageSize!.Value : total;

        var pageKeys = doPaging
            ? orderedKeys.Skip(pageIndex * pageSize).Take(pageSize).ToList()
            : orderedKeys;

        var pageItems = new List<AppLanguageConfigResponseModel>(allLangs.Count);
        foreach (var lang in allLangs)
        {
            var l = lang.Language.ToLowerInvariant();
            if (!langMaps.TryGetValue(l, out var dict))
            {
                pageItems.Add(new AppLanguageConfigResponseModel { Language = lang.Language, JSONContent = "{}" });
                continue;
            }

            var filtered = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var k in pageKeys)
            {
                if (dict.TryGetValue(k, out var v))
                {
                    filtered[k] = v ?? "";
                }
            }

            var jsonSubset = JsonSerializer.Serialize(filtered);

            pageItems.Add(new AppLanguageConfigResponseModel
            {
                Language = lang.Language,
                JSONContent = jsonSubset
            });
        }

        return new PagedResult<AppLanguageConfigResponseModel>
        {
            Items = pageItems,
            TotalItems = total,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<PagedResult<AppLanguageVersionResponseModel>> LoadAppLanguageVersionAsync(
         AppLanguageConfigRequestModel model,
         CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(model);

        var channelId = !string.IsNullOrWhiteSpace(model.RequestChannel)
            ? model.RequestChannel!
            : model.ChannelId;

        var allLangs = await _translationLanguageRepository.Table
            .Where(s => s.ChannelId == channelId)
            .Select(s => new AppLanguageVersionResponseModel
            {
                language = s.Language,
                version = s.Version,
            })
            .ToListAsync(ct);

        int total = allLangs.Count;
        int pageIndex = 0;
        int pageSize = total;

        var pageItems = new List<AppLanguageVersionResponseModel>(allLangs.Count);
        foreach (var lang in allLangs)
        {
            pageItems.Add(new AppLanguageVersionResponseModel
            {
                language = lang.language,
                version = lang.version,
            });
        }

        return new PagedResult<AppLanguageVersionResponseModel>
        {
            Items = pageItems,
            TotalItems = total,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }


    private static Dictionary<string, string> SafeFlatten(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new Dictionary<string, string>(StringComparer.Ordinal);
        }

        try
        {
            using var doc = JsonDocument.Parse(json);
            var dict = new Dictionary<string, string>(StringComparer.Ordinal);
            FlattenJson(doc.RootElement, dict, "");
            return dict;
        }
        catch
        {
            return new Dictionary<string, string>(StringComparer.Ordinal);
        }
    }


    private static void FlattenJson(JsonElement el, Dictionary<string, string> dst, string prefix)
    {
        switch (el.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var p in el.EnumerateObject())
                {
                    var key = string.IsNullOrEmpty(prefix) ? p.Name : $"{prefix}.{p.Name}";
                    FlattenJson(p.Value, dst, key);
                }
                break;
            case JsonValueKind.Array:
                int i = 0;
                foreach (var item in el.EnumerateArray())
                {
                    var key = $"{prefix}[{i++}]";
                    FlattenJson(item, dst, key);
                }
                break;
            case JsonValueKind.String:
                dst[prefix] = el.GetString() ?? "";
                break;
            case JsonValueKind.Number:
            case JsonValueKind.True:
            case JsonValueKind.False:
                dst[prefix] = el.ToString();
                break;
            default:
                dst[prefix] = "";
                break;
        }
    }
}
