using System.Linq.Expressions;
using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Domain.Localization;
using O24OpenAPI.Data;
using O24OpenAPI.Web.Framework.Services.Logging;

namespace O24OpenAPI.Web.Framework.Localization;

/// <summary>Provides information about localization</summary>
public class LocalizationService : ILocalizationService
{
    /// <summary>
    /// The logger
    /// </summary>
    protected readonly ILogger _logger;

    /// <summary>
    /// The lsr repository
    /// </summary>
    protected readonly IRepository<LocaleStringResource> _lsrRepository;

    /// <summary>
    /// The work context
    /// </summary>
    protected readonly IWorkContext _workContext;

    /// <summary>
    /// The static cache manager
    /// </summary>
    protected readonly IStaticCacheManager _staticCacheManager;

    /// <summary>Constructor</summary>
    /// <param name="logger"></param>
    /// <param name="lsrRepository"></param>
    /// <param name="staticCacheManager"></param>
    /// <param name="workContext"></param>
    public LocalizationService(
        ILogger logger,
        IRepository<LocaleStringResource> lsrRepository,
        IStaticCacheManager staticCacheManager,
        IWorkContext workContext
    )
    {
        _logger = logger;
        _lsrRepository = lsrRepository;
        _staticCacheManager = staticCacheManager;
        _workContext = workContext;
    }

    /// <summary>Gets all resource</summary>
    /// <param name="resource"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<LocaleStringResource>> GetAll(
        string resource = "",
        int pageIndex = 0,
        int pageSize = 2147483647
    )
    {
        IPagedList<LocaleStringResource> localeStringResources = await _lsrRepository.GetAllPaged(
            query =>
            {
                if (!string.IsNullOrEmpty(resource))
                {
                    // ISSUE: reference to a compiler-generated field
                    query = query.Where(
                        (Expression<Func<LocaleStringResource, bool>>)(
                            l => l.ResourceName.Contains(resource)
                        )
                    );
                }

                query = query.OrderByDescending(
                    (Expression<Func<LocaleStringResource, string>>)(l => l.ResourceName)
                );
                return query;
            },
            pageIndex,
            pageSize
        );
        IPagedList<LocaleStringResource> all = localeStringResources;
        localeStringResources = null;
        return all;
    }

    /// <summary>Get a resource by identifier</summary>
    /// <param name="localeStringResourceId"></param>
    /// <returns></returns>
    public virtual async Task<LocaleStringResource> GetById(int localeStringResourceId)
    {
        LocaleStringResource byId = await _lsrRepository.GetById(new int?(localeStringResourceId));
        return byId;
    }

    /// <summary>Get a resource by name</summary>
    /// <param name="resourceName"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    public virtual async Task<LocaleStringResource> GetByName(string resourceName, string language)
    {
        var defaultLang = "en";
        var query = _lsrRepository
            .Table.OrderBy(lsr => lsr.ResourceName)
            .Where(lsr =>
                (lsr.Language == language || lsr.Language == defaultLang)
                && lsr.ResourceName == resourceName
            );
        var localeStringResourceList = await query.ToListAsync();
        if (localeStringResourceList is null)
        {
            await _logger.Warning(
                "Resource string (" + resourceName + ") not found. Language = " + language
            );
        }

        LocaleStringResource byName = localeStringResourceList.FirstOrDefault(s =>
            s.Language == language
        );
        if (byName is null)
        {
            byName = localeStringResourceList.FirstOrDefault(s => s.Language == defaultLang);
        }
        query = null;
        localeStringResourceList = null;
        return byName;
    }

    /// <summary>Gets a resource</summary>
    /// <param name="resourceKey"></param>
    /// <returns></returns>
    public virtual async Task<string> GetResource(string resourceKey)
    {
        Language workingLanguage = await _workContext.GetWorkingLanguage();
        if (workingLanguage == null)
        {
            return string.Empty;
        }

        string resource = await GetResource(resourceKey, workingLanguage.UniqueSeoCode, "");
        return resource;
    }

    /// <summary>Get locale resource value</summary>
    /// <param name="resourceKey"></param>
    /// <param name="language"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public virtual async Task<string> GetResource(
        string resourceKey,
        string language,
        string defaultValue = ""
    )
    {
        string result = string.Empty;
        if (resourceKey == null)
        {
            resourceKey = string.Empty;
        }

        resourceKey = resourceKey.Trim();
        CacheKey key = _staticCacheManager.PrepareKeyForDefaultCache(
            LocalizationDefaults.LocaleStringResourcesByNameCacheKey,
            language,
            resourceKey
        );
        IQueryable<string> query = _lsrRepository
            .Table.Where(
                (Expression<Func<LocaleStringResource, bool>>)(
                    l => l.ResourceName == resourceKey && l.Language == language
                )
            )
            .Select((Expression<Func<LocaleStringResource, string>>)(l => l.ResourceValue));
        string lsr = await _staticCacheManager.Get(
            key,
            async () =>
            {
                string resource = await query.FirstOrDefaultAsync();
                return resource;
            }
        );
        if (lsr != null)
        {
            result = lsr;
        }

        if (!string.IsNullOrEmpty(result))
        {
            return result;
        }

        result = string.IsNullOrEmpty(defaultValue) ? resourceKey : defaultValue;
        return result;
    }

    /// <summary>Add aa new locale resource</summary>
    /// <param name="resources"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    public virtual async Task AddLocaleResource(
        IDictionary<string, string> resources,
        string language = null
    )
    {
        await DeleteLocaleResources(resources.Keys.ToList(), language);
        List<LocaleStringResource> locales = resources
            .Select(resource => new LocaleStringResource()
            {
                Language = language,
                ResourceName = resource.Key,
                ResourceValue = resource.Value,
            })
            .ToList();
        await _lsrRepository.BulkInsert(locales);
        locales = null;
    }

    /// <summary>Deletes a locale resource</summary>
    /// <param name="resourceNames"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    public virtual async Task DeleteLocaleResources(
        IList<string> resourceNames,
        string language = null
    )
    {
        int num = await _lsrRepository.DeleteWhere(locale =>
            (!string.IsNullOrEmpty(language) || locale.Language == language)
            && resourceNames.Contains(
                locale.ResourceName,
                StringComparer.InvariantCultureIgnoreCase
            )
        );
    }
}
