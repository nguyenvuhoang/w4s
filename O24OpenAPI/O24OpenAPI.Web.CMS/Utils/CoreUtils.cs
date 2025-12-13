using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Commons;
using O24OpenAPI.Web.CMS.Configuration;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.Request;
using O24OpenAPI.Web.CMS.Services.Factory;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.CMS.Services.NeptuneService;

namespace O24OpenAPI.Web.CMS.Utils;

public class CoreUtils
{
    public static async Task LoginO9ForDigital(IServiceScope scope = null)
    {
        var authCoreService = EngineFactory.Resolve<ICoreAuthenticateService>(scope);
        var modelLoginForDigital = new AuthenJWTModel()
        {
            LoginName = GlobalVariable.Optimal9Settings.O9UserName,
            PassWord = GlobalVariable.Optimal9Settings.O9Password,
            IsEncrypted = !GlobalVariable.Optimal9Settings.O9Encrypt,
            IsDigital = true,
            DeviceId = "local_server",
        };
        await authCoreService.Login(modelLoginForDigital);
    }

    public static async Task LoginNeptuneCBS(IServiceScope scope = null)
    {
        var neptuneCBSService = EngineContext.Current.Resolve<INeptuneCBSService>(scope);
        var setting = EngineContext.Current.Resolve<CoreBankingSetting>(scope);
        if (
            setting == null
            || setting.UserNameLoginNeptune.NullOrEmpty()
            || setting.StaticTokenPortal.NullOrEmpty()
        )
        {
            return;
        }

        var portalToken = setting.StaticTokenPortal;
        var refid = Guid.NewGuid().ToString();
        var content = new JObject
        {
            { "lang", "en" },
            { "token", $"{portalToken}" },
            { "reference_id", refid },
            {
                "fields",
                new JObject
                {
                    { "username", setting.UserNameLoginNeptune },
                    { "password", setting.PasswordLoginNeptune },
                }
            },
        };
        var headers = new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {portalToken}" },
            { "Cache-Control", "no-cache" },
            { "Content-Type", "application/json" },
        };

        var model = new CallApiModel
        {
            WorkflowId = "UMG_LOGIN",
            Content = content.ToSerialize(),
            Header = headers,
        };
        var response = await neptuneCBSService.CallApiAsync(model, refid);

        if (response.Data.TryGetValue("token", out var token))
        {
            NeptuneSession.SetToken(token.ToString());
        }
    }
}
