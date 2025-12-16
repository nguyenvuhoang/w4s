using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public partial class ReasonsDefinitionService : IReasonsDefinitionService
{
    #region Fields

    private readonly ILocalizationService _localizationService;

    private readonly IRepository<C_REASONS_DEFINITION> _DreasonsDefinitionApiRepository;

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="learnApiRepository"></param>
    public ReasonsDefinitionService(
        ILocalizationService localizationService,
        IRepository<C_REASONS_DEFINITION> reasonsDefinitionApiRepository
    )
    {
        _localizationService = localizationService;
        _DreasonsDefinitionApiRepository = reasonsDefinitionApiRepository;
    }

    #endregion

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<C_REASONS_DEFINITION> GetById(int id)
    {
        return await _DreasonsDefinitionApiRepository.GetById(id);
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<C_REASONS_DEFINITION> Insert(C_REASONS_DEFINITION learnApi)
    {
        try
        {
            var findForm = await _DreasonsDefinitionApiRepository
                .Table.Where(s => s.Id.Equals(learnApi.Id))
                .FirstOrDefaultAsync();
            if (findForm == null)
            {
                await _DreasonsDefinitionApiRepository.Insert(learnApi);
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
    public virtual async Task<List<ReasonsDefinitionViewModel>> GetAll()
    {
        var result = await (
            from c in _DreasonsDefinitionApiRepository.Table.DefaultIfEmpty()
            select new ReasonsDefinitionViewModel()
            {
                Id = c.Id,
                ReasonID = c.ReasonID,
                ReasonCode = c.ReasonCode,
                ReasonName = c.ReasonName,
                ReasonType = c.ReasonType,
                ReasonEvent = c.ReasonEvent,
                ReasonAction = c.ReasonAction,
                Description = c.Description,
                Status = c.Status,
            }
        ).ToListAsync();
        return result
            ?? new List<ReasonsDefinitionViewModel> { new ReasonsDefinitionViewModel { Id = 0 } };
    }

    ///
    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<C_REASONS_DEFINITION> Update(C_REASONS_DEFINITION learnApi)
    {
        try
        {
            await _DreasonsDefinitionApiRepository.Update(learnApi);
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
    public virtual async Task<C_REASONS_DEFINITION> DeleteById(int id)
    {
        var learnApi = await GetById(id);
        await _DreasonsDefinitionApiRepository.Delete(learnApi);
        return learnApi;
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<ReasonsDefinitionViewModel> ViewById(int id)
    {
        try
        {
            var entity = await _DreasonsDefinitionApiRepository.GetById(id);
            if (entity == null)
            {
                throw new O24OpenAPIException("Fee code does not exist");
            }

            var result = entity.ToModel<ReasonsDefinitionViewModel>();
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
    public virtual async Task<IPagedList<ReasonsDefinitionSearchSimpleResponseModel>> SimpleSearch(
        SimpleSearchModel model
    )
    {
        model.SearchText = !string.IsNullOrEmpty(model.SearchText)
            ? model.SearchText
            : string.Empty;
        var query = await (
            from c in _DreasonsDefinitionApiRepository.Table.DefaultIfEmpty()
            where
                c.ReasonID.ToString().Contains(model.SearchText)
                || c.ReasonCode.Contains(model.SearchText)
                || c.ReasonName.Contains(model.SearchText)
                || c.ReasonType.Contains(model.SearchText)
                || c.ReasonEvent.Contains(model.SearchText)
                || c.ReasonAction.Contains(model.SearchText)
                || c.Description.Contains(model.SearchText)
                || c.Status.Contains(model.SearchText)
            select new ReasonsDefinitionSearchSimpleResponseModel()
            {
                Id = c.Id,
                ReasonID = c.ReasonID,
                ReasonCode = c.ReasonCode,
                ReasonName = c.ReasonName,
                ReasonType = c.ReasonType,
                ReasonEvent = c.ReasonEvent,
                ReasonAction = c.ReasonAction,
                Description = c.Description,
                Status = c.Status,
            }
        ).OrderBy(c => c.ReasonCode).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<
        IPagedList<ReasonsDefinitionSearchAdvanceResponseModel>
    > SearchAdvance(ReasonsDefinitionSearchAdvanceModel model)
    {
        var query = await (
            from c in _DreasonsDefinitionApiRepository.Table.DefaultIfEmpty()
            where
                (
                    !string.IsNullOrEmpty(c.ReasonCode) && !string.IsNullOrEmpty(model.ReasonCode)
                        ? c.ReasonCode.Contains(model.ReasonCode)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.ReasonName) && !string.IsNullOrEmpty(model.ReasonName)
                        ? c.ReasonName.Contains(model.ReasonName)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.ReasonType) && !string.IsNullOrEmpty(model.ReasonType)
                        ? c.ReasonType.Contains(model.ReasonType)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.ReasonEvent) && !string.IsNullOrEmpty(model.ReasonEvent)
                        ? c.ReasonEvent.Contains(model.ReasonEvent)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.ReasonAction)
                    && !string.IsNullOrEmpty(model.ReasonAction)
                        ? c.ReasonAction.Contains(model.ReasonAction)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.Description) && !string.IsNullOrEmpty(model.Description)
                        ? c.Description.Contains(model.Description)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.Status) && !string.IsNullOrEmpty(model.Status)
                        ? c.Status.Contains(model.Status)
                        : true
                )
            select new ReasonsDefinitionSearchAdvanceResponseModel()
            {
                Id = c.Id,
                ReasonID = c.ReasonID,
                ReasonCode = c.ReasonCode,
                ReasonName = c.ReasonName,
                ReasonType = c.ReasonType,
                ReasonEvent = c.ReasonEvent,
                ReasonAction = c.ReasonAction,
                Description = c.Description,
                Status = c.Status,
            }
        ).OrderBy(c => c.ReasonCode).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }
}
