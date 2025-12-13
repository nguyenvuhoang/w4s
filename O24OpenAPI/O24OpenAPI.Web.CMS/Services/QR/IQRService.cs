using O24OpenAPI.Web.CMS.Models.QR;

namespace O24OpenAPI.Web.CMS.Services.QR;

public interface IQRService
{
    Task<QRData> AddAsync(QRData qrData);
    Task<QRData> GetByKeyAsync(string key);
    Task UpdateAsync(QRData qrData);
    Task<GenQRResponse> GenQR(GenQRRequest request);
}
