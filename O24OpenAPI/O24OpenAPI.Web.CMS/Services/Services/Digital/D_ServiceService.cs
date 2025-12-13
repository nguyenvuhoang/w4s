using Microsoft.IdentityModel.Tokens;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Localization;

namespace O24OpenAPI.Web.CMS.Services.Services;

public partial class D_ServiceService : ID_ServiceService
{
    #region Fields

    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_SERVICE> _D_ServiceApiRepository;

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="learnApiRepository"></param>
    public D_ServiceService(
        ILocalizationService localizationService,
        IRepository<D_SERVICE> learnApiRepository
    )
    {
        _localizationService = localizationService;
        _D_ServiceApiRepository = learnApiRepository;
    }

    #endregion

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_SERVICE> GetById(int id)
    {
        return await _D_ServiceApiRepository.GetById(id);
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_SERVICE> Insert(D_SERVICE learnApi)
    {
        try
        {
            var findForm = await _D_ServiceApiRepository
                .Table.Where(s => s.Id.Equals(learnApi.Id))
                .FirstOrDefaultAsync();
            if (findForm == null)
            {
                await _D_ServiceApiRepository.Insert(learnApi);
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
    public virtual async Task<List<ServiceViewModel>> GetAll()
    {
        var result = await (
            from c in _D_ServiceApiRepository.Table.DefaultIfEmpty()
            select new ServiceViewModel()
            {
                Id = c.Id,
                ServiceID = c.ServiceID,
                ServiceName = c.ServiceName,
                Status = c.Status,
                BankService = c.BankService,
                CorpService = c.CorpService,
                checkuseronline = c.checkuseronline,
            }
        ).ToListAsync();
        return result ?? new List<ServiceViewModel> { new ServiceViewModel { Id = 0 } };
    }

    ///
    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_SERVICE> Update(D_SERVICE learnApi)
    {
        try
        {
            await _D_ServiceApiRepository.Update(learnApi);
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
    public virtual async Task<D_SERVICE> DeleteById(int id)
    {
        var learnApi = await GetById(id);
        await _D_ServiceApiRepository.Delete(learnApi);
        return learnApi;
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<ServiceViewModel> ViewById(int id)
    {
        try
        {
            var entity = await _D_ServiceApiRepository.GetById(id);
            if (entity == null)
            {
                throw new O24OpenAPIException("Country code does not exist");
            }

            var result = entity.ToModel<ServiceViewModel>();
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
    public virtual async Task<IPagedList<ServiceSearchSimpleResponseModel>> SimpleSearch(
        SimpleSearchModel model
    )
    {
        model.SearchText = !string.IsNullOrEmpty(model.SearchText)
            ? model.SearchText
            : string.Empty;
        var query = await (
            from c in _D_ServiceApiRepository.Table.DefaultIfEmpty()
            where
                c.ServiceID.Contains(model.SearchText)
                || c.ServiceName.Contains(model.SearchText)
                || c.Status.Contains(model.SearchText)
                || c.BankService.ToString().Contains(model.SearchText)
                || c.CorpService.ToString().Contains(model.SearchText)
                || c.checkuseronline.ToString().Contains(model.SearchText)
            select new ServiceSearchSimpleResponseModel()
            {
                Id = c.Id,
                ServiceID = c.ServiceID,
                ServiceName = c.ServiceName,
                Status = c.Status,
                BankService = c.BankService,
                CorpService = c.CorpService,
                checkuseronline = c.checkuseronline,
            }
        )
            .OrderBy(c => c.Id)
            .AsQueryable()
            .ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<ServiceSearchAdvanceResponseModel>> SearchAdvance(
        ServiceSearchAdvanceModel model
    )
    {
        var query =
            from c in _D_ServiceApiRepository.Table.DefaultIfEmpty()
            where
                (
                    !string.IsNullOrEmpty(c.ServiceID) && !string.IsNullOrEmpty(model.ServiceID)
                        ? c.ServiceID.Contains(model.ServiceID)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.ServiceName) && !string.IsNullOrEmpty(model.ServiceName)
                        ? c.ServiceName.Contains(model.ServiceName)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.Status) && !string.IsNullOrEmpty(model.Status)
                        ? c.Status.Contains(model.Status)
                        : true
                )

            select new ServiceSearchAdvanceResponseModel()
            {
                Id = c.Id,
                ServiceID = c.ServiceID,
                ServiceName = c.ServiceName,
                Status = c.Status,
                BankService = c.BankService,
                CorpService = c.CorpService,
                checkuseronline = c.checkuseronline,
            };
        if (model.BankService.ToString().IsNullOrEmpty())
        {
            query = query.Where(s => s.BankService == model.BankService);
        }
        if (model.CorpService.ToString().IsNullOrEmpty())
        {
            query = query.Where(s => s.CorpService == model.CorpService);
        }
        if (model.checkuseronline.ToString().IsNullOrEmpty())
        {
            query = query.Where(s => s.checkuseronline == model.checkuseronline);
        }
        return await query.AsQueryable().ToPagedList(model.PageIndex, model.PageSize);
        ;
    }
}
