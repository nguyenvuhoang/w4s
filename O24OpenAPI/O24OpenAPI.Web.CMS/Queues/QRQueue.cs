using Newtonsoft.Json;
using O24OpenAPI.Web.CMS.Models.QR;
using O24OpenAPI.Web.CMS.Services.QR;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.Web.CMS.Queues;

public class QRQueue : BaseQueue
{
    private readonly IQRService _qrService = EngineContext.Current.Resolve<IQRService>();

    public async Task<WFScheme> GetQR(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<GetQRRequest>();
        return await Invoke<GetQRRequest>(
            wfScheme,
            async () =>
            {
                var result = await _qrService.GetByKeyAsync(model.Key);
                if (result == null)
                {
                    throw new O24OpenAPIException("Invalid QR code or QR code has expired.");
                }
                return new GetQRResponse
                {
                    Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                        result.Data
                    ),
                };
            }
        );
    }

    public async Task<WFScheme> GenQR(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<GenQRRequest>();
        return await Invoke<GenQRRequest>(
            wfScheme,
            async () =>
            {
                var result = await _qrService.GenQR(model);
                return result;
            }
        );
    }
}
