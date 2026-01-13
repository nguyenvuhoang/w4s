using LinqToDB;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Services;

public class CurrencyService(IRepository<Currency> currencyRepository) : ICurrencyService
{
    private readonly IRepository<Currency> _currencyRepository = currencyRepository;

    public async Task<GetCurrencyResponse> GetCurrency(string currencyId)
    {
        var currency = await _currencyRepository.Table.Where(c => c.CurrencyId.Equals(currencyId) && c.StatusOfCurrency.Equals("A")).FirstOrDefaultAsync();
        if (currency == null)
        {
            var nullResponse = new GetCurrencyResponse
            {
                Id = 0
            };
            return nullResponse;
        }
        var response = new GetCurrencyResponse
        {
            Id = currency.Id,
            CurrencyId = currency.CurrencyId,
            ShortCurrencyId = currency.ShortCurrencyId,
            StatusOfCurrency = currency.StatusOfCurrency,
            RoundingDigits = currency.RoundingDigits,
            DecimalDigits = currency.DecimalDigits,
            CurrencyNumber = currency.CurrencyNumber,
        };
        return response;
    }

    public async Task<GetCurrencyResponse> GetCurrencyByShortId(string shortCurrencyId)
    {
        var currency = await _currencyRepository.Table.Where(c => c.ShortCurrencyId.Equals(shortCurrencyId) && c.StatusOfCurrency.Equals("A")).FirstOrDefaultAsync();
        if (currency == null)
        {
            var nullResponse = new GetCurrencyResponse
            {
                Id = 0
            };
            return nullResponse;
        }
        var response = new GetCurrencyResponse
        {
            Id = currency.Id,
            CurrencyId = currency.CurrencyId,
            ShortCurrencyId = currency.ShortCurrencyId,
            StatusOfCurrency = currency.StatusOfCurrency,
            RoundingDigits = currency.RoundingDigits,
            DecimalDigits = currency.DecimalDigits,
            DisplayOrder = currency.DisplayOrder
        };
        return response;
    }
}
