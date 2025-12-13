using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Newtonsoft.Json;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Services;
using GetByGroupAndNameRequestModel = O24OpenAPI.Web.CMS.Models.GetByGroupAndNameRequestModel;
using ICodeListService = O24OpenAPI.Web.CMS.Services.Interfaces.ICodeListService;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class CodeListWorkflow(ICodeListService codeListService) : BaseQueueService
{
    private readonly ICodeListService _codeListService = codeListService;

    public async Task<WorkflowScheme> GetAll(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<BaseTransactionModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var listEntity = await _codeListService.GetAll(model.Language);
                return listEntity;
            }
        );
    }

    public async Task<WorkflowScheme> View(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ModelWithId>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _codeListService.ViewById(model.Id, model.Language);
                return result;
            }
        );
    }

    public async Task<WorkflowScheme> Insert(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<CodeListInsertModel>();
        return await Invoke<CodeListInsertModel>(
            workflow,
            async () =>
            {
                if (string.IsNullOrEmpty(model.CodeId))
                {
                    throw new O24OpenAPIException("InvalidCodeId", "Code id not null");
                }

                var entity = model.FromModel<C_CODELIST>();
                entity.MCaption = JsonConvert.SerializeObject(model.Mcaption);
                var result = await _codeListService.Insert(entity);
                return result;
            }
        );
    }

    public async Task<WorkflowScheme> Update(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<CodeListUpdateModel>();
        return await Invoke<CodeListUpdateModel>(
            workflow,
            async () =>
            {
                C_CODELIST entity =
                    await _codeListService.GetById(model.Id)
                    ?? throw new O24OpenAPIException("InvalidCodeId", "Code id does not exist");
                var mcaption = entity.MCaption;
                entity = model.ToEntity(entity);
                entity.MCaption = JsonConvert.SerializeObject(
                    new MultiCaption
                    {
                        Vietnamese = string.IsNullOrEmpty(model.Mcaption?.Vietnamese)
                            ? ""
                            : model.Mcaption.Vietnamese,
                        English = string.IsNullOrEmpty(model.Mcaption?.English)
                            ? ""
                            : model.Mcaption.English,
                        ThaiLand = string.IsNullOrEmpty(model.Mcaption?.ThaiLand)
                            ? ""
                            : model.Mcaption.ThaiLand,
                        Lao = string.IsNullOrEmpty(model.Mcaption?.Lao) ? "" : model.Mcaption.Lao,
                        Cambodia = string.IsNullOrEmpty(model.Mcaption?.Cambodia)
                            ? ""
                            : model.Mcaption.Cambodia,
                        Myanmar = string.IsNullOrEmpty(model.Mcaption?.Myanmar)
                            ? ""
                            : model.Mcaption.Myanmar,
                    }
                );
                var result = await _codeListService.Update(entity);
                return result;
            }
        );
    }

    public async Task<WorkflowScheme> DeleteById(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ModelWithId>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _codeListService.DeleteById(model.Id);
                return result;
            }
        );
    }

    public async Task<WorkflowScheme> SearchSimple(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<SimpleSearchModel>();
        return await Invoke<SimpleSearchModel>(
            workflow,
            async () =>
            {
                var result = await _codeListService.SimpleSearch(model);
                var items = result.ToPagedListModel<
                    CodeListSearchResponseModel,
                    CodeListSearchResponseModel
                >();
                return items;
            }
        );
    }

    public async Task<WorkflowScheme> SearchAdvance(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<CodeListAdvancedSearchRequestModel>();
        return await Invoke<CardServiceAdvancedSearchRequestModel>(
            workflow,
            async () =>
            {
                var result = await _codeListService.AdvancedSearch(model);
                var items = result.ToPagedListModel<
                    CodeListSearchResponseModel,
                    CodeListSearchResponseModel
                >();
                return items;
            }
        );
    }

    public async Task<WorkflowScheme> GetByGroupAndName(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetByGroupAndNameRequestModel>();
        return await Invoke<GetByGroupAndNameRequestModel>(
            workflow,
            async () =>
            {
                var result = await _codeListService.GetByGroupAndName(model);
                return result;
            }
        );
    }

    public async Task<WorkflowScheme> GetInfoCodeList(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<CodeListWithPrimaryKeyModel>();
        return await Invoke<CodeListWithPrimaryKeyModel>(
            workflow,
            async () =>
            {
                var result = await _codeListService.GetInfoCodeList(model);
                return result;
            }
        );
    }
}
