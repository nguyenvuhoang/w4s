using O24OpenAPI.Client.Infisical;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services.Queue;
using O24OpenAPI.Web.CMS.Models.Infisical;

namespace O24OpenAPI.Web.CMS.Queues;

public class InfisicalQueue : BaseQueue
{
    public async Task<WFScheme> GetSecret(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<GetSecretRequestModel>();
        return await Invoke<GetSecretRequestModel>(
            wfScheme,
            async () =>
            {
                var value = InfisicalManager.GetSecretByKey<string>(model.Key);
                var response = new GetSecretResponseModel { Value = value };
                return await Task.FromResult(response);
            }
        );
    }
}
