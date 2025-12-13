using O24OpenAPI.Web.CMS.Models;
using CodeListResponseModel = O24OpenAPI.Web.CMS.Models.CodeListResponseModel;
using GetByGroupAndNameRequestModel = O24OpenAPI.Web.CMS.Models.GetByGroupAndNameRequestModel;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface ICodeListService
{
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Domain.C_CODELIST> GetById(int id);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<CodeListViewResponseModel>> GetAll(string lang);

    /// <summary>
    /// ViewById
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<CodeListViewResponseModel> ViewById(int id, string lang);

    Task<string> GetCaption(string codeId, string codename, string codegroup, string lang);

    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="codeList">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<Domain.C_CODELIST> Insert(Domain.C_CODELIST codeList);

    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="codeList">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<Domain.C_CODELIST> Update(Domain.C_CODELIST codeList);

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="codeId">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<Domain.C_CODELIST> DeleteById(int codeId);

    /// <summary>
    /// SimpleSearch
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<CodeListSearchResponseModel>> SimpleSearch(SimpleSearchModel model);

    /// <summary>
    /// AdvancedSearch
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<CodeListSearchResponseModel>> AdvancedSearch(
        CodeListAdvancedSearchRequestModel model
    );

    Task<List<Domain.C_CODELIST>> GetByApp(string app);

    Task<List<CodeListResponseModel>> GetByGroupAndName(GetByGroupAndNameRequestModel model);

    Task<CodeListResponseModel> GetInfoCodeList(CodeListWithPrimaryKeyModel model);
}
