using Apache.NMS.ActiveMQ.Util.Synchronization;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class CodeListService(
    IRepository<Domain.C_CODELIST> codeListRepository,
    IStaticCacheManager distributedCacheManager
) : ICodeListService
{
    private readonly IRepository<Domain.C_CODELIST> _codeListRepository = codeListRepository;
    private readonly IStaticCacheManager _distributedCacheManager = distributedCacheManager;

    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<Domain.C_CODELIST> GetById(int id)
    {
        var entity =
            await _codeListRepository.GetById(id)
            ?? throw new O24OpenAPIException("InvalidCodeId", "Code id does not exist");
        return entity;
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <param name="lang"></param>
    /// <returns></returns>
    public virtual async Task<List<CodeListViewResponseModel>> GetAll(string lang = "en")
    {
        var query = await (
            from c in _codeListRepository.Table.DefaultIfEmpty()
            select new CodeListViewResponseModel()
            {
                Id = c.Id,
                CodeId = c.CodeId,
                CodeName = c.CodeName,
                Caption = c.Caption,
                MCaption = c.MCaption.GetLangValue(lang),
                CodeGroup = c.CodeGroup,
                CodeIndex = c.CodeIndex,
                CodeValue = c.CodeValue,
                Ftag = c.Ftag,
                Visible = c.Visible,
            }
        ).ToListAsync();
        return query;
    }

    public virtual async Task<string> GetCaption(
        string codeId,
        string codename,
        string codegroup,
        string lang = "en"
    )
    {
        var entity = await _codeListRepository
            .Table.Where(s =>
                s.CodeId == codeId && s.CodeName == codename && s.CodeGroup == codegroup
            )
            .FirstOrDefaultAsync();
        return entity.MCaption.GetLangValue(lang);
    }

    /// <summary>
    /// ViewById
    /// </summary>
    /// <param name="id"></param>
    /// <param name="lang"></param>
    /// <returns></returns>
    public virtual async Task<CodeListViewResponseModel> ViewById(int id, string lang = "en")
    {
        var entity =
            await _codeListRepository.GetById(id)
            ?? throw new O24OpenAPIException("InvalidCodeId", "Code id does not exist");
        var response = new CodeListViewResponseModel()
        {
            Id = entity.Id,
            CodeId = entity.CodeId,
            CodeName = entity.CodeName,
            Caption = entity.Caption,
            MCaption = entity.MCaption.GetLangValue(lang),
            CodeGroup = entity.CodeGroup,
            CodeIndex = entity.CodeIndex,
            CodeValue = entity.CodeValue,
            Ftag = entity.Ftag,
            Visible = entity.Visible,
        };
        return response;
    }

    /// <summary>
    /// Insert
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public virtual async Task<Domain.C_CODELIST> Insert(Domain.C_CODELIST codeList)
    {
        await _codeListRepository.Insert(codeList);
        return codeList;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public virtual async Task<Domain.C_CODELIST> Update(Domain.C_CODELIST codeList)
    {
        await _codeListRepository.Update(codeList);
        return codeList;
    }

    /// <summary>
    /// Delete By Id
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public virtual async Task<Domain.C_CODELIST> DeleteById(int codeListId)
    {
        var entity = await _codeListRepository.GetById(codeListId);
        if (entity == null)
        {
            throw new O24OpenAPIException("InvalidCodeId", "Code id does not exist");
        }

        await _codeListRepository.Delete(entity);
        return entity;
    }

    /// <summary>
    /// Simple search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual async Task<IPagedList<CodeListSearchResponseModel>> SimpleSearch(
        SimpleSearchModel model
    )
    {
        model.SearchText = !string.IsNullOrEmpty(model.SearchText)
            ? model.SearchText
            : string.Empty;
        var query = await (
            from p in _codeListRepository.Table.DefaultIfEmpty()
            where
                p.CodeId.Contains(model.SearchText)
                || p.CodeName.Contains(model.SearchText)
                || p.Caption.Contains(model.SearchText)
                || p.Ftag.Contains(model.SearchText)
                || p.CodeIndex.ToString().Contains(model.SearchText)
            select new CodeListSearchResponseModel()
            {
                Id = p.Id,
                CodeId = p.CodeId,
                CodeName = p.CodeName,
                Caption = p.Caption,
                CodeGroup = p.CodeGroup,
                CodeIndex = p.CodeIndex,
                CodeValue = p.CodeValue,
                Ftag = p.Ftag,
                Visible = p.Visible,
            }
        ).OrderBy(c => c.CodeId).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<CodeListSearchResponseModel>> AdvancedSearch(
        CodeListAdvancedSearchRequestModel model
    )
    {
        var query = await (
            from c in _codeListRepository.Table.DefaultIfEmpty()
            where
                (
                    !string.IsNullOrEmpty(c.CodeId) && !string.IsNullOrEmpty(model.CodeId)
                        ? c.CodeId.Contains(model.CodeId)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.CodeName) && !string.IsNullOrEmpty(model.CodeName)
                        ? c.CodeName.Contains(model.CodeName)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.Caption) && !string.IsNullOrEmpty(model.Caption)
                        ? c.Caption.Contains(model.Caption)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.Ftag) && !string.IsNullOrEmpty(model.Ftag)
                        ? c.Ftag.Contains(model.Ftag)
                        : true
                )
            select new CodeListSearchResponseModel()
            {
                Id = c.Id,
                CodeId = c.CodeId,
                CodeName = c.CodeName,
                Caption = c.Caption,
                CodeGroup = c.CodeGroup,
                CodeIndex = c.CodeIndex,
                CodeValue = c.CodeValue,
                Ftag = c.Ftag,
                Visible = c.Visible,
            }
        ).OrderBy(c => c.CodeId).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    public virtual async Task<List<Domain.C_CODELIST>> GetByApp(string app)
    {
        var key = new CacheKey(CachingKey.EntityTemplate, new object[] { nameof(Domain.C_CODELIST), app })
        {
            IsForever = true,
        };
        return await _distributedCacheManager.GetOrSetAsync(
            key,
            () => _codeListRepository.Table.Where(c => c.App == app).ToListAsync()
        );
    }

    public virtual async Task<List<Models.CodeListResponseModel>> GetByGroupAndName(
        GetByGroupAndNameRequestModel model
    )
    {
        var query = await _codeListRepository
            .Table.Where(c => c.CodeGroup == model.CodeGroup && c.CodeName == model.CodeName)
            .Select(s => new Models.CodeListResponseModel()
            {
                Id = s.Id,
                CodeId = s.CodeId,
                CodeName = s.CodeName,
                Caption = s.Caption,
                LanguageCaption = s.MCaption.GetLangValue(null),
                CodeGroup = s.CodeGroup,
                CodeIndex = s.CodeIndex,
                CodeValue = s.CodeValue,
                Ftag = s.Ftag,
                Visible = s.Visible,
            })
            .ToListAsync();
        return query;
    }

    public virtual async Task<Models.CodeListResponseModel> GetInfoCodeList(
        CodeListWithPrimaryKeyModel model
    )
    {
        var query = await _codeListRepository
            .Table.Where(c => c.CodeGroup == model.CodeGroup && c.CodeName == model.CodeName)
            .Select(s => new Models.CodeListResponseModel()
            {
                Id = s.Id,
                CodeId = s.CodeId,
                CodeName = s.CodeName,
                Caption = s.Caption,
                LanguageCaption = s.MCaption.GetLangValue(null),
                CodeGroup = s.CodeGroup,
                CodeIndex = s.CodeIndex,
                CodeValue = s.CodeValue,
                Ftag = s.Ftag,
                Visible = s.Visible,
            })
            .FirstOrDefaultAsync();
        return query;
    }
}

public static class CaptionExtension
{
    public static string GetCaption(this string value, string codename, string codegroup)
    {
        var codelistService = EngineContext.Current.Resolve<ICodeListService>();
        var lang = Utils.Utils.GetRequestLanguage();
        var result = codelistService.GetCaption(value, codename, codegroup, lang).GetAsyncResult();

        return result;
    }
}
