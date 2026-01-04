using LinKit.Core.Cqrs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.CMS.API.Application.Utils;
using O24OpenAPI.CMS.Domain.AggregateModels.FormAggregate;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Attributes;
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
            return new JObject
            {
                ["success"] = false,
                ["error"] = "FormId is required"
            };

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
                ["form_design_detail"] = JToken.FromObject(configForm)
            }
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
            List<Dictionary<string, object>> views = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(
                layout.GetValueOrDefault("list_view")?.ToString() ?? "[]"
            );

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
                List<Dictionary<string, object>> components = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(
                    view.GetValueOrDefault("list_input")?.ToString() ?? "[]"
                );

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
        List<CTHUserInRoleModel> rolesOfUser = await cTHGrpcClientService.GetListRoleByUserCodeAsync(userCode);
        List<int> listRoleId = rolesOfUser.Select(s => s.RoleId).ToList();
        Dictionary<string, object> roleTask = [];
        roleTask.MergeDictionary(await BuildRoleTaskOfForm(listRoleId, app, formId, form_config));

        return roleTask;
    }

    private async Task<Dictionary<string, object>> BuildRoleTaskOfForm(
        List<int> listRoleId,
        string app,
        string formCode,
        FormModel formConfig
    )
    {
        Dictionary<string, object> result = [];
        formConfig ??= await formService.GetByIdAndApp(formCode, app);
        if (formConfig == null)
        {
            return null;
        }

        List<Dictionary<string, object>> listLayout = formConfig.ListLayout;
        try
        {
            CTHUserCommandModel getMenuByFormCode = (
                await cTHGrpcClientService.GetInfoFromFormCodeAsync(app, formCode)
            ).FirstOrDefault();
            List<CTHCommandIdInfoModel> getListMenuRight = [];
            List<CTHCommandIdInfoModel> getListParentID = [];
            if (getMenuByFormCode != null)
            {
                getListMenuRight = await cTHGrpcClientService.GetInfoFromCommandIdAsync(
                    app,
                    getMenuByFormCode.CommandId
                );
                getListParentID = await cTHGrpcClientService.GetInfoFromParentIdAsync(
                    app,
                    getMenuByFormCode.CommandId
                );
            }

            System.Console.WriteLine("getListMenuRight====" + getListMenuRight.ToSerialize());

            for (int i = 0; i < listRoleId.Count; i++)
            {
                JObject roleTaskRole = [];

                if (getMenuByFormCode != null)
                {
                    if (!string.IsNullOrEmpty(getMenuByFormCode.CommandId))
                    {
                        List<CTHCommandIdInfoModel> getListCommandRight = getListParentID.FindAll(s =>
                            s.RoleId == listRoleId[i]
                        );

                        foreach (CTHCommandIdInfoModel itemRight in getListCommandRight)
                        {
                            roleTaskRole[itemRight.CommandId] = new ComponentRoleModel()
                            {
                                component = new RoleTaskModel() { install = itemRight.Invoke == 1 },
                            }.ToJToken();
                        }

                        roleTaskRole[formCode] = new FormRoleModel { }.ToJToken();
                        System.Console.WriteLine("listRoleId[i]====" + listRoleId[i]);

                        CTHCommandIdInfoModel getRightForm = getListMenuRight.Find(s => s.RoleId == listRoleId[i]);
                        System.Console.WriteLine(
                            "getRightForm before====" + getRightForm.ToSerialize()
                        );

                        if (getRightForm != null)
                        {
                            roleTaskRole[formCode] = new FormRoleModel
                            {
                                form = new FormRoleInfoModel()
                                {
                                    accept = getRightForm.Invoke == 1,
                                },
                            }.ToJToken();

                            roleTaskRole[getMenuByFormCode.CommandId] = new ComponentRoleModel()
                            {
                                component = new RoleTaskModel()
                                {
                                    install = getRightForm.Invoke == 1,
                                },
                            }.ToJToken();
                        }

                        foreach (Dictionary<string, object> layout in listLayout)
                        {
                            roleTaskRole[layout["codeHidden"].ToString()] = new LayoutRoleModel()
                            { }.ToJToken();

                            foreach (
                                Dictionary<string, object> view in JsonConvert.DeserializeObject<
                                    List<Dictionary<string, object>>
                                >(layout.GetValueOrDefault("list_view").ToString())
                            )
                            {
                                roleTaskRole[view["codeHidden"].ToString()] = new ViewRoleModel()
                                { }.ToJToken();

                                foreach (
                                    Dictionary<string, object> component in JsonConvert.DeserializeObject<
                                        List<Dictionary<string, object>>
                                    >(view.GetValueOrDefault("list_input").ToString())
                                )
                                {
                                    JObject configDefaultCpn = JObject.FromObject(
                                        component.GetValueOrDefault("default")
                                    );
                                    string commandId = configDefaultCpn["codeHidden"].ToString();
                                    if (roleTaskRole[commandId] == null)
                                    {
                                        roleTaskRole[commandId] = new ComponentRoleModel()
                                        { }.ToJToken();
                                    }
                                }
                            }
                        }
                        System.Console.WriteLine(
                            "roleTaskRole after====" + roleTaskRole.ToSerialize()
                        );
                    }
                    else
                    {
                        roleTaskRole[formCode] = new FormRoleModel { }.ToJToken();

                        foreach (Dictionary<string, object> layout in listLayout)
                        {
                            roleTaskRole[layout["codeHidden"].ToString()] = new LayoutRoleModel()
                            { }.ToJToken();

                            foreach (
                                Dictionary<string, object> view in JsonConvert.DeserializeObject<
                                    List<Dictionary<string, object>>
                                >(layout.GetValueOrDefault("list_view").ToString())
                            )
                            {
                                roleTaskRole[view["codeHidden"].ToString()] = new ViewRoleModel()
                                { }.ToJToken();
                                foreach (
                                    Dictionary<string, object> component in JsonConvert.DeserializeObject<
                                        List<Dictionary<string, object>>
                                    >(view.GetValueOrDefault("list_input").ToString())
                                )
                                {
                                    JObject configDefaultCpn = JObject.FromObject(
                                        component.GetValueOrDefault("default")
                                    );

                                    string commandId = configDefaultCpn["codeHidden"].ToString();
                                    roleTaskRole[commandId] = new ComponentRoleModel()
                                    { }.ToJToken();
                                }
                            }
                        }
                    }
                }
                else
                {
                    roleTaskRole[formCode] = new FormRoleModel { }.ToJToken();

                    foreach (Dictionary<string, object> layout in listLayout)
                    {
                        roleTaskRole[layout["codeHidden"].ToString()] = new LayoutRoleModel()
                        { }.ToJToken();

                        foreach (
                            Dictionary<string, object> view in JsonConvert.DeserializeObject<
                                List<Dictionary<string, object>>
                            >(layout.GetValueOrDefault("list_view").ToString())
                        )
                        {
                            roleTaskRole[view["codeHidden"].ToString()] = new ViewRoleModel()
                            { }.ToJToken();
                            foreach (
                                Dictionary<string, object> component in JsonConvert.DeserializeObject<
                                    List<Dictionary<string, object>>
                                >(view.GetValueOrDefault("list_input").ToString())
                            )
                            {
                                JObject configDefaultCpn = JObject.FromObject(
                                    component.GetValueOrDefault("default")
                                );

                                string commandId = configDefaultCpn["codeHidden"].ToString();
                                roleTaskRole[commandId] = new ComponentRoleModel() { }.ToJToken();
                            }
                        }
                    }
                }

                result[listRoleId[i].ToString()] = roleTaskRole;
            }

            // processor.Complete();
            // await processor.Completion;
        }
        catch (System.Exception ex)
        {
            // TODO
            System.Console.WriteLine("ExceptionBuildRoleTask==" + ex.StackTrace);
        }
        return result;
    }
}
