using O24OpenAPI.Web.CMS.Configuration;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.Request;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services.Queue;

namespace O24OpenAPI.Web.CMS.Queues;

public class SignatureQueue : BaseQueue
{
    private readonly IUserSessionsService _userSessionService =
        EngineContext.Current.Resolve<IUserSessionsService>();
    private readonly CMSSetting _cmsSetting = EngineContext.Current.Resolve<CMSSetting>();

    public async Task<WFScheme> SavePublicKeyAsync(WFScheme scheme)
    {
        var model = await scheme.ToModel<SignatureRequestModel>();
        return await Invoke<SignatureRequestModel>(
            scheme,
            async () =>
            {
                if (!_cmsSetting.ListChannelCheckSignature.Contains(model.ChannelId))
                {
                    throw new Exception("Channel not allowed to register signature");
                }
                if (string.IsNullOrEmpty(model.PublicKey))
                {
                    throw new Exception("Public key is empty");
                }
                await _userSessionService.UpdateSignatureKey(
                    token: model.Token,
                    signatureKey: model.PublicKey
                );
                return new StatusCompleteResponse();
            }
        );
    }
}
