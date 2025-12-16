using Microsoft.IdentityModel.Tokens;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public partial class FeeService : IFeeService
{
    #region Fields

    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_FEE> _DfeeApiRepository;

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="learnApiRepository"></param>
    public FeeService(
        ILocalizationService localizationService,
        IRepository<D_FEE> learnApiRepository
    )
    {
        _localizationService = localizationService;
        _DfeeApiRepository = learnApiRepository;
    }

    #endregion

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_FEE> GetById(int id)
    {
        return await _DfeeApiRepository.GetById(id);
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_FEE> Insert(D_FEE learnApi)
    {
        try
        {
            var findForm = await _DfeeApiRepository
                .Table.Where(s => s.Id.Equals(learnApi.Id))
                .FirstOrDefaultAsync();
            if (findForm == null)
            {
                await _DfeeApiRepository.Insert(learnApi);
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
    public virtual async Task<List<FeeViewModel>> GetAll()
    {
        var result = await (
            from c in _DfeeApiRepository.Table.DefaultIfEmpty()
            select new FeeViewModel()
            {
                Id = c.Id,
                FeeID = c.FeeID,
                FeeName = c.FeeName,
                FeeType = c.FeeType,
                CCYID = c.CCYID,
                UserCreated = c.UserCreated,
                UserModified = c.UserModified,
                ChargeLater = c.ChargeLater,
                IsRegionFee = c.IsRegionFee,
                FixAmount = c.FixAmount,
                IsLadder = c.IsLadder,
                IsBillPaymentFee = c.IsBillPaymentFee,
                DateCreated = c.DateCreated,
                DateModified = c.DateModified,
            }
        ).ToListAsync();
        return result ?? new List<FeeViewModel> { new FeeViewModel { Id = 0 } };
    }

    ///
    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_FEE> Update(D_FEE learnApi)
    {
        try
        {
            await _DfeeApiRepository.Update(learnApi);
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
    public virtual async Task<D_FEE> DeleteById(int id)
    {
        var learnApi = await GetById(id);
        await _DfeeApiRepository.Delete(learnApi);
        return learnApi;
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<FeeViewModel> ViewById(int id)
    {
        try
        {
            var entity = await _DfeeApiRepository.GetById(id);
            if (entity == null)
            {
                throw new O24OpenAPIException("Fee code does not exist");
            }

            var result = entity.ToModel<FeeViewModel>();
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
    public virtual async Task<IPagedList<FeeSearchSimpleResponseModel>> SimpleSearch(
        SimpleSearchModel model
    )
    {
        model.SearchText = !string.IsNullOrEmpty(model.SearchText)
            ? model.SearchText
            : string.Empty;
        var query = await (
            from c in _DfeeApiRepository.Table.DefaultIfEmpty()
            where
                c.FeeID.Contains(model.SearchText)
                || c.FeeName.Contains(model.SearchText)
                || c.FeeType.Contains(model.SearchText)
                || c.CCYID.Contains(model.SearchText)
                || c.UserCreated.Contains(model.SearchText)
                || c.UserModified.Contains(model.SearchText)
                || c.ChargeLater.Contains(model.SearchText)
                || c.IsRegionFee.ToString().Contains(model.SearchText)
                || c.FixAmount.ToString().Contains(model.SearchText)
                || c.IsLadder.ToString().Contains(model.SearchText)
                || c.IsBillPaymentFee.ToString().Contains(model.SearchText)
            select new FeeSearchSimpleResponseModel()
            {
                Id = c.Id,
                FeeID = c.FeeID,
                FeeName = c.FeeName,
                FeeType = c.FeeType,
                CCYID = c.CCYID,
                UserCreated = c.UserCreated,
                UserModified = c.UserModified,
                ChargeLater = c.ChargeLater,
                IsRegionFee = c.IsRegionFee,
                FixAmount = c.FixAmount,
                IsLadder = c.IsLadder,
                IsBillPaymentFee = c.IsBillPaymentFee,
                DateCreated = c.DateCreated,
                DateModified = c.DateModified,
            }
        ).OrderBy(c => c.FeeID).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<FeeSearchAdvanceResponseModel>> SearchAdvance(
        FeeSearchAdvanceModel model
    )
    {
        var query =
            from c in _DfeeApiRepository.Table.DefaultIfEmpty()
            where
                (
                    !string.IsNullOrEmpty(c.FeeID) && !string.IsNullOrEmpty(model.FeeID)
                        ? c.FeeID.Contains(model.FeeID)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.FeeName) && !string.IsNullOrEmpty(model.FeeName)
                        ? c.FeeName.Contains(model.FeeName)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.FeeType) && !string.IsNullOrEmpty(model.FeeType)
                        ? c.FeeType.Contains(model.FeeType)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.CCYID) && !string.IsNullOrEmpty(model.CCYID)
                        ? c.CCYID.Contains(model.CCYID)
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
                && (
                    !string.IsNullOrEmpty(c.ChargeLater) && !string.IsNullOrEmpty(model.ChargeLater)
                        ? c.ChargeLater.Contains(model.ChargeLater)
                        : true
                )
            select new FeeSearchAdvanceResponseModel()
            {
                Id = c.Id,
                FeeID = c.FeeID,
                FeeName = c.FeeName,
                FeeType = c.FeeType,
                CCYID = c.CCYID,
                UserCreated = c.UserCreated,
                UserModified = c.UserModified,
                ChargeLater = c.ChargeLater,
                IsRegionFee = c.IsRegionFee,
                FixAmount = c.FixAmount,
                IsLadder = c.IsLadder,
                IsBillPaymentFee = c.IsBillPaymentFee,
                DateCreated = c.DateCreated,
                DateModified = c.DateModified,
            };

        if (model.IsRegionFee.ToString().IsNullOrEmpty())
        {
            query = query.Where(s => s.IsRegionFee == model.IsRegionFee);
        }
        if (model.IsLadder.ToString().IsNullOrEmpty())
        {
            query = query.Where(s => s.IsLadder == model.IsLadder);
        }
        if (model.IsBillPaymentFee.ToString().IsNullOrEmpty())
        {
            query = query.Where(s => s.IsBillPaymentFee == model.IsBillPaymentFee);
        }
        if (model.FixAmount.ToString().IsNullOrEmpty())
        {
            query = query.Where(s => s.FixAmount == model.FixAmount);
        }
        if (DateTime.TryParse(model.DateCreated, out DateTime temp1))
        {
            query = query.Where(s => s.DateCreated.Value.Date == temp1.Date);
        }
        if (DateTime.TryParse(model.DateModified, out DateTime temp2))
        {
            query = query.Where(s => s.DateModified.Value.Date == temp2.Date);
        }
        return await query.AsQueryable().ToPagedList(model.PageIndex, model.PageSize);
    }
}
