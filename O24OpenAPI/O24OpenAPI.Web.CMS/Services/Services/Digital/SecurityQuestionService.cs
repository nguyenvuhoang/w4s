using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public partial class SecurityQuestionService : ISecurityQuestionService
{
    #region Fields

    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_SECURITY_QUESTION> _DsecurityQuestionApiRepository;

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="learnApiRepository"></param>
    public SecurityQuestionService(
        ILocalizationService localizationService,
        IRepository<D_SECURITY_QUESTION> learnApiRepository
    )
    {
        _localizationService = localizationService;
        _DsecurityQuestionApiRepository = learnApiRepository;
    }

    #endregion

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_SECURITY_QUESTION> GetById(int id)
    {
        return await _DsecurityQuestionApiRepository.GetById(id);
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_SECURITY_QUESTION> Insert(D_SECURITY_QUESTION learnApi)
    {
        try
        {
            var findForm = await _DsecurityQuestionApiRepository
                .Table.Where(s => s.Id.Equals(learnApi.Id))
                .FirstOrDefaultAsync();
            if (findForm == null)
            {
                await _DsecurityQuestionApiRepository.Insert(learnApi);
            }
            else
            {
                throw new O24OpenAPIException(
                    $"Security question with ID {learnApi.Id} already exist"
                );
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
    public virtual async Task<List<SecurityQuestionViewModel>> GetAll()
    {
        var result = await (
            from c in _DsecurityQuestionApiRepository.Table.DefaultIfEmpty()
            select new SecurityQuestionViewModel()
            {
                Id = c.Id,
                Question = c.Question,
                LangID = c.LangID,
                Status = c.Status,
                UserCreated = c.UserCreated,
                DateCreated = c.DateCreated,
                UserModified = c.UserModified,
            }
        ).ToListAsync();
        return result
            ?? new List<SecurityQuestionViewModel> { new SecurityQuestionViewModel { Id = 0 } };
    }

    ///
    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_SECURITY_QUESTION> Update(D_SECURITY_QUESTION learnApi)
    {
        try
        {
            var entity = await GetById(learnApi.Id);
            if (entity == null)
            {
                throw new O24OpenAPIException($"Security question with ID {learnApi.Id} not exist");
            }

            await _DsecurityQuestionApiRepository.Update(learnApi);
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
    public virtual async Task<D_SECURITY_QUESTION> DeleteById(int id)
    {
        try
        {
            var learnApi = await GetById(id);
            if (learnApi == null)
            {
                throw new O24OpenAPIException($"Security question with ID {id} not exist");
            }

            await _DsecurityQuestionApiRepository.Delete(learnApi);
            return learnApi;
        }
        catch (Exception ex)
        {
            throw new O24OpenAPIException(ex.Message, ex);
        }
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<SecurityQuestionViewModel> ViewById(int id)
    {
        try
        {
            var entity = await _DsecurityQuestionApiRepository.GetById(id);
            if (entity == null)
            {
                throw new O24OpenAPIException($"Security question with ID {id} not exist");
            }

            var result = entity.ToModel<SecurityQuestionViewModel>();
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
    public virtual async Task<IPagedList<SecurityQuestionSearchSimpleResponseModel>> SimpleSearch(
        SimpleSearchModel model
    )
    {
        try
        {
            model.SearchText = !string.IsNullOrEmpty(model.SearchText)
                ? model.SearchText
                : string.Empty;
            var query = await (
                from c in _DsecurityQuestionApiRepository.Table.DefaultIfEmpty()
                where
                    c.Question.Contains(model.SearchText)
                    || c.LangID.Contains(model.SearchText)
                    || c.Status.Contains(model.SearchText)
                    || c.UserCreated.Contains(model.SearchText)
                    || c.UserModified.Contains(model.SearchText)
                    || c.DateCreated.ToString().Contains(model.SearchText)
                select new SecurityQuestionSearchSimpleResponseModel()
                {
                    Id = c.Id,
                    Question = c.Question,
                    LangID = c.LangID,
                    Status = c.Status,
                    UserCreated = c.UserCreated,
                    DateCreated = c.DateCreated,
                    UserModified = c.UserModified,
                }
            ).OrderBy(c => c.Id).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);
            if (query == null || !query.Any())
            {
                throw new O24OpenAPIException("Data not found");
            }
            return query;
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
    public virtual async Task<IPagedList<SecurityQuestionSearchAdvanceResponseModel>> SearchAdvance(
        SecurityQuestionSearchAdvanceModel model
    )
    {
        var query =
            from c in _DsecurityQuestionApiRepository.Table.DefaultIfEmpty()
            where
                (
                    !string.IsNullOrEmpty(c.Question) && !string.IsNullOrEmpty(model.Question)
                        ? c.Question.Contains(model.Question)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.LangID) && !string.IsNullOrEmpty(model.LangID)
                        ? c.LangID.Contains(model.LangID)
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
            select new SecurityQuestionSearchAdvanceResponseModel()
            {
                Id = c.Id,
                Question = c.Question,
                LangID = c.LangID,
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
    }
}
