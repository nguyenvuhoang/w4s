using O24OpenAPI.CMS.API.Application.Models.QR;
using O24OpenAPI.CMS.Domain.AggregateModels;

namespace O24OpenAPI.CMS.API.Application.Features.QR;

public interface IQRService
{
    Task<QRData> AddAsync(QRData qrData);
    Task<QRData> GetByKeyAsync(string key);
    Task UpdateAsync(QRData qrData);
    Task<GenQRResponse> GenQR(GenQRRequest request);
}
