using O24OpenAPI.CMS.API.Application.Models.QR;

namespace O24OpenAPI.CMS.API.Application.Services.QR;

public interface IQRService
{
    Task<QRData> AddAsync(QRData qrData);
    Task<QRData> GetByKeyAsync(string key);
    Task UpdateAsync(QRData qrData);
    Task<GenQRResponse> GenQR(GenQRRequest request);
}
