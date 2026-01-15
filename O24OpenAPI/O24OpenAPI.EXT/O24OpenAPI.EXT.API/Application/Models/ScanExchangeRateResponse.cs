using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.EXT.API.Application.Models
{
    public class ScanExchangeRateResponse : BaseO24OpenAPIModel
    {
        public int Total { get; init; }
        public int Inserted { get; init; } = 0;
        public int Updated { get; init; }
        public DateTime? RateDateUtc { get; init; }
        public ScanExchangeRateResponse() { }
    }
}
