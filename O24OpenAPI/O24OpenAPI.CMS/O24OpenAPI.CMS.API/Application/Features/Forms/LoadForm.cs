using LinKit.Core.Cqrs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.CMS.API.Application.Utils;
using O24OpenAPI.CMS.Domain.AggregateModels.FormAggregate;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using System.Text.Json.Serialization;

namespace O24OpenAPI.CMS.API.Application.Features.Forms;

public class LoadFormCommand : BaseTransactionModel, ICommand<JToken>
{
    [JsonPropertyName("form_id")]
    public string FormId { get; set; }

    [JsonPropertyName("application_code")]
    public string ApplicationCode { get; set; }
}

[CqrsHandler]
public class LoadFormHandler(
    IFormService formService,
    IFormFieldDefinitionRepository formFieldDefinitionRepository,
    ICTHGrpcClientService cTHGrpcClientService,
    WorkContext workContext
) : ICommandHandler<LoadFormCommand, JToken>
{
    [LearnApi("CMS_LOAD_FORM")]
    public async Task<JToken> HandleAsync(
        LoadFormCommand request,
        CancellationToken cancellationToken = default
    )
    {
        if (request.FormId == null)
            return new JObject { ["success"] = false, ["error"] = "FormId is required" };

        string formId = request.FormId;

        FormModel configForm =
            await formService.GetByIdAndApp(formId, request.ApplicationCode)
            ?? throw await ErrorUtils.CreateException(
                "SYS_03_001",
                request.FormId,
                request.ApplicationCode
            );

        await ProcessLocalizedNames(configForm, formId, request.Language ?? "en");

        Dictionary<string, object> roleTask = await BuildRoleTaskWithListRole(
            workContext.CurrentChannel,
            formId,
            configForm,
            workContext.UserContext.UserCode
        );

        return new JObject
        {
            ["success"] = true,
            ["data"] = new JObject
            {
                ["loadRoleTask"] = JToken.FromObject(roleTask),
                ["form_design_detail"] = JToken.FromObject(configForm),
            },
        };
    }

    private async Task ProcessLocalizedNames(FormModel configForm, string formId, string language)
    {
        if (configForm == null)
        {
            return;
        }

        foreach (Dictionary<string, object> layout in configForm.ListLayout)
        {
            List<Dictionary<string, object>> views = JsonConvert.DeserializeObject<
                List<Dictionary<string, object>>
            >(layout.GetValueOrDefault("list_view")?.ToString() ?? "[]");

            foreach (Dictionary<string, object> view in views)
            {
                string viewCode = view.GetValueOrDefault("code")?.ToString();
                string viewName = view.GetValueOrDefault("name")?.ToString();
                if (!string.IsNullOrEmpty(viewCode))
                {
                    string localizedValue = await formFieldDefinitionRepository.GetFieldValueAsync(
                        language ?? "en",
                        formId: formId,
                        fieldName: viewCode
                    );

                    view["name"] = string.IsNullOrEmpty(localizedValue) ? viewName : localizedValue;
                }
                List<Dictionary<string, object>> components = JsonConvert.DeserializeObject<
                    List<Dictionary<string, object>>
                >(view.GetValueOrDefault("list_input")?.ToString() ?? "[]");

                foreach (Dictionary<string, object> component in components)
                {
                    object defaultObjRaw = component.GetValueOrDefault("default");
                    if (defaultObjRaw == null)
                    {
                        continue;
                    }

                    JObject defaultObj = JObject.FromObject(defaultObjRaw);

                    string code = defaultObj["code"]?.ToString();
                    string defaultName = defaultObj["name"]?.ToString();

                    if (!string.IsNullOrEmpty(code))
                    {
                        string localizedValue =
                            await formFieldDefinitionRepository.GetFieldValueAsync(
                                language ?? "en",
                                formId: formId,
                                fieldName: code
                            );

                        defaultObj["name"] = string.IsNullOrEmpty(localizedValue)
                            ? defaultName
                            : localizedValue;

                        component["default"] = defaultObj;
                    }
                }

                view["list_input"] = JToken.FromObject(components);
            }

            layout["list_view"] = JToken.FromObject(views);
        }
    }

    private async Task<Dictionary<string, object>> BuildRoleTaskWithListRole(
        string app,
        string formId,
        FormModel form_config,
        string userCode
    )
    {
        List<CTHUserInRoleModel> rolesOfUser =
            await cTHGrpcClientService.GetListRoleByUserCodeAsync(userCode);
        List<int> listRoleId = rolesOfUser.Select(s => s.RoleId).ToList();
        Dictionary<string, object> roleTask = [];
        roleTask.MergeDictionary(await BuildRoleTaskOfForm(listRoleId, app, formId, form_config));

        return roleTask;
    }

    private async Task<Dictionary<string, object>?> BuildRoleTaskOfForm(
    List<int> listRoleId,
    string app,
    string formCode,
    FormModel? formConfig
)
    {
        var result = new Dictionary<string, object>();

        formConfig ??= await formService.GetByIdAndApp(formCode, app);
        if (formConfig == null) return null;

        var listLayout = formConfig.ListLayout ?? new List<Dictionary<string, object>>();

        try
        {
            var menuByFormList = await cTHGrpcClientService.GetInfoFromFormCodeAsync(app, formCode);

            var commandIds = menuByFormList
                .Select(x => x?.CommandId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var menuRightAll = new List<CTHCommandIdInfoModel>();
            var parentRightAll = new List<CTHCommandIdInfoModel>();

            if (commandIds.Count > 0)
            {
                foreach (var cid in commandIds)
                {
                    var r1 = await cTHGrpcClientService.GetInfoFromCommandIdAsync(app, cid);
                    if (r1 != null && r1.Count > 0) menuRightAll.AddRange(r1);

                    var r2 = await cTHGrpcClientService.GetInfoFromParentIdAsync(app, cid);
                    if (r2 != null && r2.Count > 0) parentRightAll.AddRange(r2);
                }
            }

            var roleSet = new HashSet<int>(listRoleId);

            var menuRootByRole = menuRightAll
                .Where(x => x != null && roleSet.Contains(x.RoleId) && !string.IsNullOrWhiteSpace(x.CommandId))
                .GroupBy(x => x.RoleId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var menuChildByRole = parentRightAll
                .Where(x => x != null && roleSet.Contains(x.RoleId) && !string.IsNullOrWhiteSpace(x.CommandId))
                .GroupBy(x => x.RoleId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var baseRoleTask = BuildBaseRoleTaskFromLayout(listLayout, formCode);

            foreach (var roleId in listRoleId)
            {
                var roleTaskRole = (JObject)baseRoleTask.DeepClone();

                if (roleTaskRole[formCode] == null)
                    roleTaskRole[formCode] = new FormRoleModel().ToJToken();

                if (menuChildByRole.TryGetValue(roleId, out var childRights))
                {
                    foreach (var r in childRights)
                    {
                        var cid = r.CommandId;
                        if (string.IsNullOrWhiteSpace(cid)) continue;

                        roleTaskRole[cid] = new ComponentRoleModel
                        {
                            component = new RoleTaskModel { install = r.Invoke == 1 }
                        }.ToJToken();
                    }
                }

                bool acceptForm = false;

                if (menuRootByRole.TryGetValue(roleId, out var rootRights))
                {
                    foreach (var r in rootRights)
                    {
                        var cid = r.CommandId;
                        if (string.IsNullOrWhiteSpace(cid)) continue;

                        var allow = r.Invoke == 1;
                        acceptForm |= allow;

                        roleTaskRole[cid] = new ComponentRoleModel
                        {
                            component = new RoleTaskModel { install = allow }
                        }.ToJToken();
                    }
                }

                roleTaskRole[formCode] = new FormRoleModel
                {
                    form = new FormRoleInfoModel { accept = acceptForm }
                }.ToJToken();

                result[roleId.ToString()] = roleTaskRole;
            }
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
        }

        return result;
    }

    private static JObject BuildBaseRoleTaskFromLayout(
        List<Dictionary<string, object>> listLayout,
        string formCode
    )
    {
        var roleTaskRole = new JObject
        {
            [formCode] = new FormRoleModel().ToJToken()
        };

        foreach (var layout in listLayout)
        {
            var layoutCode = layout.GetValueOrDefault("codeHidden")?.ToString();
            if (!string.IsNullOrWhiteSpace(layoutCode))
                roleTaskRole[layoutCode] = new LayoutRoleModel().ToJToken();

            var viewsJson = layout.GetValueOrDefault("list_view")?.ToString();
            if (string.IsNullOrWhiteSpace(viewsJson)) continue;

            var views = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(viewsJson)
                        ?? [];

            foreach (var view in views)
            {
                var viewCode = view.GetValueOrDefault("codeHidden")?.ToString();
                if (!string.IsNullOrWhiteSpace(viewCode))
                    roleTaskRole[viewCode] = new ViewRoleModel().ToJToken();

                var inputsJson = view.GetValueOrDefault("list_input")?.ToString();
                if (string.IsNullOrWhiteSpace(inputsJson)) continue;

                var inputs = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(inputsJson)
                            ?? [];

                foreach (var component in inputs)
                {
                    var defObj = component.GetValueOrDefault("default");
                    if (defObj == null) continue;

                    var def = JObject.FromObject(defObj);
                    var commandId = def["codeHidden"]?.ToString();

                    if (string.IsNullOrWhiteSpace(commandId)) continue;

                    if (roleTaskRole[commandId] == null)
                        roleTaskRole[commandId] = new ComponentRoleModel().ToJToken();
                }
            }
        }

        return roleTaskRole;
    }

}
