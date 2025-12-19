using Newtonsoft.Json;
using O24OpenAPI.CMS.API.Application.Models.QR;
using O24OpenAPI.CMS.API.Application.Services.QR;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services.Queue;

namespace O24OpenAPI.CMS.API.Queues;

public class QRQueue : BaseQueue
{
    private readonly IQRService _qrService = EngineContext.Current.Resolve<IQRService>();

    public async Task<WFScheme> GetQR(WFScheme wfScheme)
    {
        GetQRRequest model = await wfScheme.ToModel<GetQRRequest>();
        return await Invoke<GetQRRequest>(
            wfScheme,
            async () =>
            {
                QRData result = await _qrService.GetByKeyAsync(model.Key);
                if (result == null)
                {
                    throw new O24OpenAPIException("Invalid QR code or QR code has expired.");
                }
                return new GetQRResponse
                {
                    Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Data),
                };
            }
        );
    }

    public async Task<WFScheme> GenQR(WFScheme wfScheme)
    {
        GenQRRequest model = await wfScheme.ToModel<GenQRRequest>();
        return await Invoke<GenQRRequest>(
            wfScheme,
            async () =>
            {
                GenQRResponse result = await _qrService.GenQR(model);
                return result;
            }
        );
    }
}
