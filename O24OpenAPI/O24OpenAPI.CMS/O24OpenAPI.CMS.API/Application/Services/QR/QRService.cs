using LinqToDB;
using O24OpenAPI.CMS.API.Application.Models.QR;
using O24OpenAPI.Core.Extensions;

namespace O24OpenAPI.CMS.API.Application.Services.QR;

public class QRService(IRepository<QRData> repo) : IQRService
{
    private readonly IRepository<QRData> _repo = repo;

    public async Task<QRData> AddAsync(QRData qrData)
    {
        return await _repo.InsertAsync(qrData);
    }

    public async Task<QRData> GetByKeyAsync(string key)
    {
        string hashedId = key.Hash();
        return await _repo.Table.Where(s => s.HashedId == hashedId).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(QRData qrData)
    {
        await _repo.Update(qrData);
    }

    public async Task<GenQRResponse> GenQR(GenQRRequest request)
    {
        var key = Guid.NewGuid().ToString();
        var qrData = new QRData
        {
            ChannelId = request.ChannelId,
            HashedId = key.Hash(),
            Data = request.ToSerialize(),
        };
        await AddAsync(qrData);
        return new GenQRResponse { Key = key };
    }
}
