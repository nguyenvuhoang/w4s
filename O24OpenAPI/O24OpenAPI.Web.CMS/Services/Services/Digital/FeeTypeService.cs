using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Localization;

namespace O24OpenAPI.Web.CMS.Services.Services;

public partial class FeeTypeService : IFeeTypeService
{
    #region Fields

    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_FEE_TYPE> _DFeeTypeApiRepository;

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="learnApiRepository"></param>
    public FeeTypeService(
        ILocalizationService localizationService,
        IRepository<D_FEE_TYPE> learnApiRepository
    )
    {
        _localizationService = localizationService;
        _DFeeTypeApiRepository = learnApiRepository;
    }

    #endregion

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_FEE_TYPE> GetById(int id)
    {
        return await _DFeeTypeApiRepository.GetById(id);
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_FEE_TYPE> Insert(D_FEE_TYPE learnApi)
    {
        try
        {
            var findForm = await _DFeeTypeApiRepository
                .Table.Where(s => s.Id.Equals(learnApi.Id))
                .FirstOrDefaultAsync();
            if (findForm == null)
            {
                await _DFeeTypeApiRepository.Insert(learnApi);
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
    public virtual async Task<List<FeeTypeViewModel>> GetAll()
    {
        var result = await (
            from c in _DFeeTypeApiRepository.Table.DefaultIfEmpty()
            select new FeeTypeViewModel()
            {
                Id = c.Id,
                FeeType = c.FeeType,
                TypeName = c.TypeName,
            }
        ).ToListAsync();
        return result ?? new List<FeeTypeViewModel> { new FeeTypeViewModel { Id = 0 } };
    }

    ///
    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_FEE_TYPE> Update(D_FEE_TYPE learnApi)
    {
        try
        {
            await _DFeeTypeApiRepository.Update(learnApi);
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
    public virtual async Task<D_FEE_TYPE> DeleteById(int id)
    {
        var learnApi = await GetById(id);
        await _DFeeTypeApiRepository.Delete(learnApi);
        return learnApi;
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<FeeTypeViewModel> ViewById(int id)
    {
        try
        {
            var entity = await _DFeeTypeApiRepository.GetById(id);
            if (entity == null)
            {
                throw new O24OpenAPIException("Fee code does not exist");
            }

            var result = entity.ToModel<FeeTypeViewModel>();
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
    public virtual async Task<IPagedList<FeeTypeSearchSimpleResponseModel>> SimpleSearch(
        SimpleSearchModel model
    )
    {
        model.SearchText = !string.IsNullOrEmpty(model.SearchText)
            ? model.SearchText
            : string.Empty;
        var query = await (
            from c in _DFeeTypeApiRepository.Table.DefaultIfEmpty()
            where c.FeeType.Contains(model.SearchText) || c.TypeName.Contains(model.SearchText)
            select new FeeTypeSearchSimpleResponseModel()
            {
                Id = c.Id,
                FeeType = c.FeeType,
                TypeName = c.TypeName,
            }
        )
            .OrderBy(c => c.FeeType)
            .AsQueryable()
            .ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<FeeTypeSearchAdvanceResponseModel>> SearchAdvance(
        FeeTypeSearchAdvanceModel model
    )
    {
        var query = await (
            from c in _DFeeTypeApiRepository.Table.DefaultIfEmpty()
            where
                (
                    !string.IsNullOrEmpty(c.FeeType) && !string.IsNullOrEmpty(model.FeeType)
                        ? c.FeeType.Contains(model.FeeType)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.TypeName) && !string.IsNullOrEmpty(model.TypeName)
                        ? c.TypeName.Contains(model.TypeName)
                        : true
                )
            select new FeeTypeSearchAdvanceResponseModel()
            {
                Id = c.Id,
                FeeType = c.FeeType,
                TypeName = c.TypeName,
            }
        )
            .OrderBy(c => c.FeeType)
            .AsQueryable()
            .ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }
}
