using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Localization;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.Services;

public partial class CountryService : ICountryService
{
    #region Fields

    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_COUNTRY> _DcountryApiRepository;

    private readonly IStoredCommandService _storedCommandService;
    private readonly IMappingService _mappingService;
    private readonly IO9ClientService _o9ClientService;
    private readonly IWorkflowStepService _workflowStepService;

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="learnApiRepository"></param>
    public CountryService(
        ILocalizationService localizationService,
        IRepository<D_COUNTRY> learnApiRepository,
        IStoredCommandService storedCommandService,
        IO9ClientService o9ClientService,
        IWorkflowStepService workflowStepService,
        IMappingService mappingService
    )
    {
        this._localizationService = localizationService;
        this._DcountryApiRepository = learnApiRepository;
        this._storedCommandService = storedCommandService;
        this._o9ClientService = o9ClientService;
        this._workflowStepService = workflowStepService;
        this._mappingService = mappingService;
    }

    #endregion

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_COUNTRY> GetById(int id)
    {
        return await _DcountryApiRepository.GetById(id);
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_COUNTRY> Insert(D_COUNTRY learnApi)
    {
        try
        {
            var findForm = await _DcountryApiRepository
                .Table.Where(s => s.Id.Equals(learnApi.Id))
                .FirstOrDefaultAsync();
            if (findForm == null)
            {
                await _DcountryApiRepository.Insert(learnApi);
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
    public virtual async Task<List<CountryViewModel>> GetAll()
    {
        var result = await (
            from c in _DcountryApiRepository.Table.DefaultIfEmpty()
            select new CountryViewModel()
            {
                Id = c.Id,
                CountryID = c.CountryID,
                CountryCode = c.CountryCode,
                CountryName = c.CountryName,
                MCountryName = c.MCountryName,
                CapitalName = c.CapitalName,
                CurrencyID = c.CurrencyID,
                Language = c.Language,
                Status = c.Status,
                Order = c.Order,
                Description = c.Description,
                UserCreated = c.UserCreated,
                DateCreated = c.DateCreated,
                UserModified = c.UserModified,
                LastModified = c.LastModified,
            }
        ).ToListAsync();
        return result ?? new List<CountryViewModel> { new CountryViewModel { Id = 0 } };
    }

    ///
    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_COUNTRY> Update(D_COUNTRY learnApi)
    {
        try
        {
            await _DcountryApiRepository.Update(learnApi);
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
    public virtual async Task<D_COUNTRY> DeleteById(int id)
    {
        var learnApi = await GetById(id);
        await _DcountryApiRepository.Delete(learnApi);
        return learnApi;
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<CountryViewModel> ViewById(int id)
    {
        try
        {
            var entity = await _DcountryApiRepository.GetById(id);
            if (entity == null)
            {
                throw new O24OpenAPIException("Country code does not exist");
            }

            var result = entity.ToModel<CountryViewModel>();
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
    public virtual async Task<IPagedList<SearchSimpleResponseModel>> SimpleSearch(
        SimpleSearchModel model
    )
    {
        model.SearchText = !string.IsNullOrEmpty(model.SearchText)
            ? model.SearchText
            : string.Empty;
        var query = await (
            from c in _DcountryApiRepository.Table.DefaultIfEmpty()
            where
                c.CountryID.Contains(model.SearchText)
                || c.CountryCode.Contains(model.SearchText)
                || c.CountryName.Contains(model.SearchText)
                || c.MCountryName.Contains(model.SearchText)
                || c.CapitalName.Contains(model.SearchText)
                || c.CurrencyID.Contains(model.SearchText)
                || c.Language.Contains(model.SearchText)
                || c.Status.Contains(model.SearchText)
                || c.Order.ToString().Contains(model.SearchText)
                || c.Description.Contains(model.SearchText)
                || c.UserCreated.Contains(model.SearchText)
                || c.UserApproved.Contains(model.SearchText)
                || c.CountryCode.Contains(model.SearchText)
                || c.UserModified.Contains(model.SearchText)
                || c.TimeZone.Contains(model.SearchText)
                || c.PhoneCountryCode.Contains(model.SearchText)
            select new SearchSimpleResponseModel()
            {
                Id = c.Id,
                CountryID = c.CountryID,
                CountryCode = c.CountryCode,
                CountryName = c.CountryName,
                MCountryName = c.MCountryName,
                CapitalName = c.CapitalName,
                CurrencyID = c.CurrencyID,
                Language = c.Language,
                Status = c.Status,
                Order = c.Order,
                Description = c.Description,
                UserCreated = c.UserCreated,
                DateCreated = c.DateCreated,
                UserModified = c.UserModified,
                LastModified = c.LastModified,
                UserApproved = c.UserApproved,
                DateApproved = c.DateApproved,
                TimeZone = c.TimeZone,
                PhoneCountryCode = c.PhoneCountryCode,
            }
        )
            .OrderBy(c => c.CountryID)
            .AsQueryable()
            .ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<SearchAdvanceResponseModel>> SearchAdvance(
        CountrySearchAdvanceModel model
    )
    {
        var query = await (
            from c in _DcountryApiRepository.Table.DefaultIfEmpty()
            where
                (
                    !string.IsNullOrEmpty(c.CountryID) && !string.IsNullOrEmpty(model.CountryID)
                        ? c.CountryID.Contains(model.CountryID)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.CountryCode) && !string.IsNullOrEmpty(model.CountryCode)
                        ? c.CountryCode.Contains(model.CountryCode)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.CountryName) && !string.IsNullOrEmpty(model.CountryName)
                        ? c.CountryName.Contains(model.CountryName)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.PhoneCountryCode)
                    && !string.IsNullOrEmpty(model.PhoneCountryCode)
                        ? c.PhoneCountryCode.Contains(model.PhoneCountryCode)
                        : true
                )
            select new SearchAdvanceResponseModel()
            {
                Id = c.Id,
                CountryID = c.CountryID,
                CountryCode = c.CountryCode,
                CountryName = c.CountryName,
                MCountryName = c.MCountryName,
                CapitalName = c.CapitalName,
                CurrencyID = c.CurrencyID,
                Language = c.Language,
                Status = c.Status,
                Order = c.Order,
                Description = c.Description,
                UserCreated = c.UserCreated,
                DateCreated = c.DateCreated,
                UserModified = c.UserModified,
                LastModified = c.LastModified,
                UserApproved = c.UserApproved,
                DateApproved = c.DateApproved,
                TimeZone = c.TimeZone,
                PhoneCountryCode = c.PhoneCountryCode,
            }
        )
            .OrderBy(c => c.CountryID)
            .AsQueryable()
            .ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    public async Task DeleteByListID(DeleteCountryModel model)
    {
        model.ListID.Add(model.ID);
        foreach (string id in model.ListID)
        {
            var learnApi = await GetById(id.ToInt());
            await _DcountryApiRepository.Delete(learnApi);
        }
    }

    public async Task<JToken> GetCountryName(string countryCode)
    {
        var storedCommand = await _storedCommandService.GetByName("GetCountryName");
        if (storedCommand == null)
        {
            throw new Exception("Stored Command GetCountryCodeList not found");
        }
        string sql = storedCommand.Query;
        sql = string.Format(sql, countryCode);
        var result = await _o9ClientService.SearchAsync(sql);

        if (result["data"] is JArray dataArray && dataArray.Count > 0)
        {
            return result;
        }

        return null;
    }

    public async Task<JToken> GetCountryList()
    {
        var storedCommand = await _storedCommandService.GetByName("GetCountryList");
        if (storedCommand == null)
        {
            throw new Exception("Stored Command GetCountryCodeList not found");
        }
        string sql = storedCommand.Query;
        sql = string.Format(sql);
        var result = await _o9ClientService.SearchAsync(sql);

        if (result["data"] is JArray dataArray && dataArray.Count > 0)
        {
            return result;
        }

        return null;
    }
}
