using O24OpenAPI.O24ACT.Models;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public interface ICurrencyService
{
    Task<GetCurrencyResponse> GetCurrency(string currencyId);
    Task<GetCurrencyResponse> GetCurrencyByShortId(string shortCurrencyId);
}
