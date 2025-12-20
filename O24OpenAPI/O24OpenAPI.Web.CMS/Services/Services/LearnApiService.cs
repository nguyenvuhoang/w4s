using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.CMS.Constant;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
/// Ctor
/// </summary>
/// <param name="LearnApiRepository"></param>
public partial class LearnApiService(
    IRepository<LearnApi> LearnApiRepository,
    IStaticCacheManager staticCacheManager
) : ILearnApiService
{
    private readonly IRepository<LearnApi> _LearnApiRepository = LearnApiRepository;
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;LearnApi&gt;.</returns>
    public virtual async Task<LearnApi> GetById(int id)
    {
        return await _LearnApiRepository.GetById(id);
    }

    /// <summary>
    /// Gets GetByTxcodeAndApp
    /// </summary>
    /// <returns>Task&lt;LearnApiModel&gt;.</returns>
    public virtual async Task<LearnApiModel> GetByAppAndId(string app, string learnApiId)
    {
        try
        {
            var cacheKey = new CacheKey($"LearnApi-{app}-{learnApiId}");
            return await _staticCacheManager.GetOrSetAsync(
                cacheKey,
                async () =>
                {
                    var query = _LearnApiRepository.Table.Where(s =>
                        s.LearnApiId.Equals(learnApiId)
                    );

                    if (!Common.CommonLearnApis.Contains(learnApiId))
                    {
                        query = query.Where(s => s.App.Equals(app));
                    }

                    var getLearnApi = await query.FirstOrDefaultAsync();

                    if (getLearnApi == null)
                    {
                        return null;
                    }

                    return getLearnApi.ToModel<LearnApiModel>();
                }
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetByApp=Exception=getLearnApi=" + ex.StackTrace);
        }

        return null;
    }

    public virtual async Task<LearnApi> GetByLearnApiIdAndChannel(
        string learnApiId,
        string channelId
    )
    {
        try
        {
            var cacheKey = new CacheKey($"LearnApi:{channelId}:{learnApiId}");
            return await _staticCacheManager.GetOrSetAsync(
                cacheKey,
                async () =>
                {
                    var query = _LearnApiRepository.Table.Where(s =>
                        s.LearnApiId.Equals(learnApiId)
                    );

                    if (!Common.CommonLearnApis.Contains(learnApiId))
                    {
                        query = query.Where(s => s.App.Equals(channelId));
                    }

                    var getLearnApi = await query.FirstOrDefaultAsync();

                    if (getLearnApi == null)
                    {
                        return null;
                    }

                    return getLearnApi;
                }
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetByApp=Exception=getLearnApi=" + ex.StackTrace);
        }

        return null;
    }

    /// <summary>
    /// Gets SearchByApp
    /// </summary>
    /// <returns>Task&lt;LearnApi&gt;.</returns>
    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;LearnApi&gt;.</returns>
    public virtual async Task<LearnApi> Insert(LearnApi LearnApi)
    {
        var findForm = await _LearnApiRepository
            .Table.Where(s =>
                s.App.Equals(LearnApi.App) && s.LearnApiId.Equals(LearnApi.LearnApiId)
            )
            .FirstOrDefaultAsync();
        if (findForm == null)
        {
            await _LearnApiRepository.Insert(LearnApi);
        }

        return LearnApi;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public virtual async Task<List<LearnApiModel>> GetByApp(string app)
    {
        return await _LearnApiRepository
            .Table.Where(s => s.App.Equals(app.Trim()))
            .Select(s => s.ToModel<LearnApiModel>())
            .ToListAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<PagedListModel<LearnApi, LearnApiModel>> GetAll(
        SimpleSearchModel model
    )
    {
        var q = await _LearnApiRepository
            .Table.OrderBy(x => x.Id)
            .AsQueryable()
            .ToPagedList(model.PageIndex, model.PageSize);
        var result = q.ToPagedListModel<LearnApi, LearnApiModel>();
        return result;
    }

    ///
    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;LearnApi&gt;.</returns>
    public virtual async Task<LearnApi> Update(LearnApi LearnApi)
    {
        await _LearnApiRepository.Update(LearnApi);
        return LearnApi;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="app"></param>
    /// <param name="LearnApi"></param>
    /// <returns></returns>
    public virtual async Task<LearnApi> DeleteById(int id)
    {
        var learnAPi = await GetById(id);
        await _LearnApiRepository.Delete(learnAPi);
        return learnAPi;
    }
}
