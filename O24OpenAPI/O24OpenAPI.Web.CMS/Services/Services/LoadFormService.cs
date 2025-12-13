using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services;

public class LoadFormService : ILoadFormService
{
    private readonly IFormService _formService = EngineContext.Current.Resolve<IFormService>();
    private readonly IUserInRoleService _userInRoleService =
        EngineContext.Current.Resolve<IUserInRoleService>();
    private readonly IUserCommandService _userCommandService =
        EngineContext.Current.Resolve<IUserCommandService>();
    private JWebUIObjectContextModel _context =
        EngineContext.Current.Resolve<JWebUIObjectContextModel>();

    private readonly ICTHGrpcClientService _cthGrpcService =
        EngineContext.Current.Resolve<ICTHGrpcClientService>();
    private readonly IFormFieldDefinitionService _formFieldDefinitionService =
        EngineContext.Current.Resolve<IFormFieldDefinitionService>();

    public async Task<JToken> LoadFormAndRoleTask(FormModelRequest model)
    {
        var formId = model.FormId;

        var configForm =
            await _formService.GetByIdAndApp(formId, model.ApplicationCode)
            ?? throw await ErrorUtils.CreateException(
                "SYS_03_001",
                model.FormId,
                model.ApplicationCode
            );

        await ProcessLocalizedNames(configForm, formId, model.Language ?? "en");

        var roleTask = await BuildRoleTaskWithListRole(
            _context.InfoApp.GetApp(),
            formId,
            configForm
        );
        if (roleTask != null)
        {
            _context.Bo.AddPackFo("loadRoleTask", roleTask);

            _context.Bo.AddPackFo("form_design_detail", configForm);

            return "true".ToObjectSuccess();
        }

        return "false".ToObjectSuccess();
    }

    public async Task<Dictionary<string, object>> BuildRoleTaskWithListRole(
        string app,
        string formId,
        FormModel form_config
    )
    {
        var rolesOfUser = await _cthGrpcService.GetListRoleByUserCodeAsync(
            _context.InfoUser.UserSession.UserCode
        );
        var listRoleId = rolesOfUser.Select(s => s.RoleId).ToList();
        Dictionary<string, object> roleTask = [];
        roleTask.MergeDictionary(await BuildRoleTaskOfForm(listRoleId, app, formId, form_config));

        return roleTask;
    }

    /// <summary>
    /// BuildRoleTaskOfForm tinh tuy la day
    /// </summary>
    /// <param name="listRoleId"></param>
    /// <param name="app"></param>
    /// <param name="formCode"></param>
    /// <param name="formConfig"></param>
    /// <returns></returns>
    public async Task<Dictionary<string, object>> BuildRoleTaskOfForm(
        List<int> listRoleId,
        string app,
        string formCode,
        FormModel formConfig
    )
    {
        Dictionary<string, object> result = [];
        formConfig ??= await _formService.GetByIdAndApp(formCode, app);
        if (formConfig == null)
        {
            return null;
        }

        var listLayout = formConfig.ListLayout;
        try
        {
            var getMenuByFormCode = (
                await _cthGrpcService.GetInfoFromFormCodeAsync(app, formCode)
            ).FirstOrDefault();
            var getListMenuRight = new List<CTHCommandIdInfoModel>();
            var getListParentID = new List<CTHCommandIdInfoModel>();
            if (getMenuByFormCode != null)
            {
                getListMenuRight = await _cthGrpcService.GetInfoFromCommandIdAsync(
                    app,
                    getMenuByFormCode.CommandId
                );
                getListParentID = await _cthGrpcService.GetInfoFromParentIdAsync(
                    app,
                    getMenuByFormCode.CommandId
                );
            }

            System.Console.WriteLine("getListMenuRight====" + getListMenuRight.ToSerialize());

            for (var i = 0; i < listRoleId.Count; i++)
            {
                JObject roleTaskRole = [];

                if (getMenuByFormCode != null)
                {
                    if (!string.IsNullOrEmpty(getMenuByFormCode.CommandId))
                    {
                        var getListCommandRight = getListParentID.FindAll(s =>
                            s.RoleId == listRoleId[i]
                        );

                        foreach (var itemRight in getListCommandRight)
                        {
                            roleTaskRole[itemRight.CommandId] = new ComponentRoleModel()
                            {
                                component = new RoleTaskModel() { install = itemRight.Invoke == 1 },
                            }.ToJToken();
                        }

                        roleTaskRole[formCode] = new FormRoleModel { }.ToJToken();
                        System.Console.WriteLine("listRoleId[i]====" + listRoleId[i]);

                        var getRightForm = getListMenuRight.Find(s => s.RoleId == listRoleId[i]);
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

                        foreach (var layout in listLayout)
                        {
                            roleTaskRole[layout["codeHidden"].ToString()] = new LayoutRoleModel()
                            { }.ToJToken();

                            foreach (
                                var view in JsonConvert.DeserializeObject<
                                    List<Dictionary<string, object>>
                                >(layout.GetValueOrDefault("list_view").ToString())
                            )
                            {
                                roleTaskRole[view["codeHidden"].ToString()] = new ViewRoleModel()
                                { }.ToJToken();

                                foreach (
                                    var component in JsonConvert.DeserializeObject<
                                        List<Dictionary<string, object>>
                                    >(view.GetValueOrDefault("list_input").ToString())
                                )
                                {
                                    var configDefaultCpn = JObject.FromObject(
                                        component.GetValueOrDefault("default")
                                    );
                                    var commandId = configDefaultCpn["codeHidden"].ToString();
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

                        foreach (var layout in listLayout)
                        {
                            roleTaskRole[layout["codeHidden"].ToString()] = new LayoutRoleModel()
                            { }.ToJToken();

                            foreach (
                                var view in JsonConvert.DeserializeObject<
                                    List<Dictionary<string, object>>
                                >(layout.GetValueOrDefault("list_view").ToString())
                            )
                            {
                                roleTaskRole[view["codeHidden"].ToString()] = new ViewRoleModel()
                                { }.ToJToken();
                                foreach (
                                    var component in JsonConvert.DeserializeObject<
                                        List<Dictionary<string, object>>
                                    >(view.GetValueOrDefault("list_input").ToString())
                                )
                                {
                                    var configDefaultCpn = JObject.FromObject(
                                        component.GetValueOrDefault("default")
                                    );

                                    var commandId = configDefaultCpn["codeHidden"].ToString();
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

                    foreach (var layout in listLayout)
                    {
                        roleTaskRole[layout["codeHidden"].ToString()] = new LayoutRoleModel()
                        { }.ToJToken();

                        foreach (
                            var view in JsonConvert.DeserializeObject<
                                List<Dictionary<string, object>>
                            >(layout.GetValueOrDefault("list_view").ToString())
                        )
                        {
                            roleTaskRole[view["codeHidden"].ToString()] = new ViewRoleModel()
                            { }.ToJToken();
                            foreach (
                                var component in JsonConvert.DeserializeObject<
                                    List<Dictionary<string, object>>
                                >(view.GetValueOrDefault("list_input").ToString())
                            )
                            {
                                var configDefaultCpn = JObject.FromObject(
                                    component.GetValueOrDefault("default")
                                );

                                var commandId = configDefaultCpn["codeHidden"].ToString();
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

    /// <summary>
    /// Process Localized Names
    /// </summary>
    /// <param name="configForm"></param>
    /// <param name="formId"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    public async Task ProcessLocalizedNames(FormModel configForm, string formId, string language)
    {
        if (configForm == null)
        {
            return;
        }

        foreach (var layout in configForm.ListLayout)
        {
            var views = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(
                layout.GetValueOrDefault("list_view")?.ToString() ?? "[]"
            );

            foreach (var view in views)
            {
                var viewCode = view.GetValueOrDefault("code")?.ToString();
                var viewName = view.GetValueOrDefault("name")?.ToString();
                if (!string.IsNullOrEmpty(viewCode))
                {
                    var localizedValue = await _formFieldDefinitionService.GetFieldValueAsync(
                        language ?? "en",
                        formId: formId,
                        fieldName: viewCode
                    );

                    view["name"] = string.IsNullOrEmpty(localizedValue) ? viewName : localizedValue;
                }
                var components = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(
                    view.GetValueOrDefault("list_input")?.ToString() ?? "[]"
                );

                foreach (var component in components)
                {
                    var defaultObjRaw = component.GetValueOrDefault("default");
                    if (defaultObjRaw == null)
                    {
                        continue;
                    }

                    var defaultObj = JObject.FromObject(defaultObjRaw);

                    var code = defaultObj["code"]?.ToString();
                    var defaultName = defaultObj["name"]?.ToString();

                    if (!string.IsNullOrEmpty(code))
                    {
                        var localizedValue = await _formFieldDefinitionService.GetFieldValueAsync(
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
}
