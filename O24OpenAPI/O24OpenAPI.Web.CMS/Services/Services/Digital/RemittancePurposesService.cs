using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public partial class RemittancePurposesService : IRemittancePurposesService
{
    #region Fields

    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_REMITTANCE_PURPOSES> _DremittancePurposesApiRepository;

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="learnApiRepository"></param>
    public RemittancePurposesService(
        ILocalizationService localizationService,
        IRepository<D_REMITTANCE_PURPOSES> learnApiRepository
    )
    {
        _localizationService = localizationService;
        _DremittancePurposesApiRepository = learnApiRepository;
    }

    #endregion

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_REMITTANCE_PURPOSES> GetById(int id)
    {
        return await _DremittancePurposesApiRepository.GetById(id);
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_REMITTANCE_PURPOSES> Insert(D_REMITTANCE_PURPOSES learnApi)
    {
        try
        {
            var findForm = await _DremittancePurposesApiRepository
                .Table.Where(s => s.Id.Equals(learnApi.Id))
                .FirstOrDefaultAsync();
            if (findForm == null)
            {
                await _DremittancePurposesApiRepository.Insert(learnApi);
            }

            return learnApi;
        }
        catch (Exception ex)
        {
            throw new O24OpenAPIException(ex.Message, ex);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<RemittancePurposesViewModel>> GetAll()
    {
        var result = await (
            from c in _DremittancePurposesApiRepository.Table.DefaultIfEmpty()
            select new RemittancePurposesViewModel()
            {
                Id = c.Id,
                RemittancePurposes = c.RemittancePurposes,
                LangID = c.LangID,
                Status = c.Status,
                UserCreated = c.UserCreated,
                DateCreated = c.DateCreated,
                UserModified = c.UserModified,
                IMGLINK = c.IMGLINK,
            }
        ).ToListAsync();
        //await _DremittancePurposesApiRepository.Table.Select(s => s).ToListAsync();
        return result
            ?? new List<RemittancePurposesViewModel> { new RemittancePurposesViewModel { Id = 0 } };
    }

    ///
    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_REMITTANCE_PURPOSES> Update(D_REMITTANCE_PURPOSES learnApi)
    {
        try
        {
            await _DremittancePurposesApiRepository.Update(learnApi);
            return learnApi;
        }
        catch (Exception ex)
        {
            throw new O24OpenAPIException(ex.Message, ex);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="app"></param>
    /// <param name="learnApi"></param>
    /// <returns></returns>
    public virtual async Task<D_REMITTANCE_PURPOSES> DeleteById(int id)
    {
        var learnApi = await GetById(id);
        await _DremittancePurposesApiRepository.Delete(learnApi);
        return learnApi;
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<RemittancePurposesViewModel> ViewById(int id)
    {
        try
        {
            var entity = await _DremittancePurposesApiRepository.GetById(id);
            if (entity == null)
            {
                throw new O24OpenAPIException("Country code does not exist");
            }

            var result = entity.ToModel<RemittancePurposesViewModel>();
            return result;
        }
        catch (Exception ex)
        {
            throw new O24OpenAPIException(ex.Message, ex);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<RemittancePurposesSearchSimpleResponseModel>> SimpleSearch(
        SimpleSearchModel model
    )
    {
        model.SearchText = !string.IsNullOrEmpty(model.SearchText)
            ? model.SearchText
            : string.Empty;
        var query = await (
            from c in _DremittancePurposesApiRepository.Table.DefaultIfEmpty()
            where
                c.RemittancePurposes.Contains(model.SearchText)
                || c.LangID.Contains(model.SearchText)
                || c.Status.Contains(model.SearchText)
                || c.UserCreated.Contains(model.SearchText)
                || c.UserModified.Contains(model.SearchText)
                || c.DateCreated.ToString().Contains(model.SearchText)
                || c.IMGLINK.Contains(model.SearchText)
            select new RemittancePurposesSearchSimpleResponseModel()
            {
                Id = c.Id,
                RemittancePurposes = c.RemittancePurposes,
                LangID = c.LangID,
                Status = c.Status,
                UserCreated = c.UserCreated,
                DateCreated = c.DateCreated,
                UserModified = c.UserModified,
                IMGLINK = c.IMGLINK,
            }
        ).OrderBy(c => c.Id).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<
        IPagedList<RemittancePurposesSearchAdvanceResponseModel>
    > SearchAdvance(RemittancePurposesSearchAdvanceModel model)
    {
        var query =
            from c in _DremittancePurposesApiRepository.Table.DefaultIfEmpty()
            where
                (
                    !string.IsNullOrEmpty(c.RemittancePurposes)
                    && !string.IsNullOrEmpty(model.RemittancePurposes)
                        ? c.RemittancePurposes.Contains(model.RemittancePurposes)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.LangID) && !string.IsNullOrEmpty(model.LangID)
                        ? c.LangID.Contains(model.LangID)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.IMGLINK) && !string.IsNullOrEmpty(model.IMGLINK)
                        ? c.IMGLINK.Contains(model.IMGLINK)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.Status) && !string.IsNullOrEmpty(model.Status)
                        ? c.Status.Contains(model.Status)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.UserCreated) && !string.IsNullOrEmpty(model.UserCreated)
                        ? c.UserCreated.Contains(model.UserCreated)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.UserModified)
                    && !string.IsNullOrEmpty(model.UserModified)
                        ? c.UserModified.Contains(model.UserModified)
                        : true
                )
            select new RemittancePurposesSearchAdvanceResponseModel()
            {
                Id = c.Id,
                RemittancePurposes = c.RemittancePurposes,
                LangID = c.LangID,
                IMGLINK = c.IMGLINK,
                Status = c.Status,
                UserCreated = c.UserCreated,
                DateCreated = c.DateCreated,
                UserModified = c.UserModified,
            };
        if (DateTime.TryParse(model.DateCreated, out DateTime temp1))
        {
            query = query.Where(s => s.DateCreated.Value.Date == temp1.Date);
        }
        return await query.AsQueryable().ToPagedList(model.PageIndex, model.PageSize);
        ;
    }
}
