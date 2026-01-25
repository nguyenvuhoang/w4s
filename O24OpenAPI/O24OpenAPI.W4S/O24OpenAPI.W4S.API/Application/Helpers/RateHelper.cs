using O24OpenAPI.W4S.API.Application.Models.Currency;

namespace O24OpenAPI.W4S.API.Application.Helpers;

/// <summary>
/// Defines the <see cref="RateHelper" />
/// </summary>
public static class RateHelper
{
    /// <summary>
    /// Build rate map from request.TransferRates.
    /// Expected: amountBase = amount * rate
    /// baseCurrency is always 1
    /// </summary>
    /// <param name="transferRates">The transferRates<see cref="IList{TransferRateResponseModel}?"/></param>
    /// <param name="baseCurrency">The baseCurrency<see cref="string"/></param>
    /// <returns>The <see cref="Dictionary{string, decimal}"/></returns>
    public static Dictionary<string, decimal> BuildRateMapFromRequest(
        IList<TransferRateResponseModel> transferRates,
        string baseCurrency
    )
    {
        Dictionary<string, decimal> map = new(StringComparer.OrdinalIgnoreCase)
        {
            [baseCurrency] = 1m
        };

        if (transferRates == null || transferRates.Count == 0)
            return map;

        foreach (TransferRateResponseModel r in transferRates)
        {
            if (r == null) continue;

            var ccy = r.CurrencyCode?.Trim();
            if (string.IsNullOrWhiteSpace(ccy)) continue;

            if (ccy.Equals(baseCurrency, StringComparison.OrdinalIgnoreCase))
            {
                map[ccy] = 1m;
                continue;
            }

            var rate = r.Transfer;
            if (!rate.HasValue || rate.Value <= 0m)
                continue;

            map[ccy] = rate.Value;
        }

        return map;
    }



    /// <summary>
    /// Find rate from request-provided map.
    /// </summary>
    public static decimal GetRateToBase(
        string fromCurrency,
        string baseCurrency,
        IReadOnlyDictionary<string, decimal> rateMap
    )
    {
        if (fromCurrency.Equals(baseCurrency, StringComparison.OrdinalIgnoreCase))
            return 1m;

        if (!rateMap.TryGetValue(fromCurrency, out var rate) || rate <= 0m)
            throw new InvalidOperationException($"Missing exchange rate for {fromCurrency}->{baseCurrency}");

        return rate;
    }
}
