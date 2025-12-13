using LinqToDB;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Localization;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_CURRENCY> _currencyRepository;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="currencyRepository"></param>
    public CurrencyService(
        ILocalizationService localizationService,
        IRepository<D_CURRENCY> currencyRepository
    )
    {
        _localizationService = localizationService;
        _currencyRepository = currencyRepository;
    }

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <param name="currencycode">The id</param>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_CURRENCY> GetByCurrencyCode(string currencycode)
    {
        return await _currencyRepository
            .Table.Where(s => s.CurrencyCode == currencycode)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_CURRENCY>> GetAll()
    {
        return await _currencyRepository.Table.Select(s => s).ToListAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_CURRENCY>> Search(SearchCurrencyModel model)
    {
        var listCurrency = await (
            from d in _currencyRepository.Table
            where
                (
                    (
                        !string.IsNullOrEmpty(model.CurrencyCode)
                        && model.CurrencyCode == d.CurrencyCode
                    ) || string.IsNullOrEmpty(model.CurrencyCode)
                )
                && (
                    (
                        !string.IsNullOrEmpty(model.CurrencyNumber)
                        && model.CurrencyNumber == d.CurrencyNumber
                    ) || string.IsNullOrEmpty(model.CurrencyNumber)
                )
                && (
                    (
                        !string.IsNullOrEmpty(model.CurrencyName)
                        && model.CurrencyName == d.CurrencyName
                    ) || string.IsNullOrEmpty(model.CurrencyName)
                )
                && (
                    (
                        !string.IsNullOrEmpty(model.MasterName)
                        && model.MasterName == d.MasterName
                    ) || string.IsNullOrEmpty(model.MasterName)
                )
            select d
        ).ToListAsync();

        return listCurrency;
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_CURRENCY> Insert(D_CURRENCY currency)
    {
        var findCurrency = await _currencyRepository
            .Table.Where(s => s.CurrencyCode.Equals(currency.CurrencyCode))
            .FirstOrDefaultAsync();
        if (findCurrency == null)
        {
            await _currencyRepository.Insert(currency);
        }
        else
        {
            throw new O24OpenAPIException(
                "InvalidCurrency",
                "The currency code already existing in system"
            );
        }

        return currency;
    }

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_CURRENCY> Update(D_CURRENCY currency)
    {
        var findCurrency = await _currencyRepository
            .Table.Where(s => s.CurrencyCode.Equals(currency.CurrencyCode))
            .FirstOrDefaultAsync();
        if (findCurrency == null)
        {
            throw new O24OpenAPIException(
                "InvalidCurrency",
                "The currency code does not exist in system"
            );
        }
        else
        {
            findCurrency.CurrencyName = currency.CurrencyName;
            findCurrency.CurrencyNumber = currency.CurrencyNumber;
            findCurrency.Order = currency.Order;
            findCurrency.DecimalDigits = currency.DecimalDigits;
            findCurrency.RoundingDigit = currency.RoundingDigit;
            findCurrency.Desc = currency.Desc;
            findCurrency.Status = currency.Status;
            await _currencyRepository.Update(findCurrency);
        }
        return currency;
    }

    /// <summary>
    ///Delete.
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_CURRENCY> DeleteById(string CurrencyCode)
    {
        var currency = await _currencyRepository
            .Table.Where(s => s.CurrencyCode.Equals(CurrencyCode))
            .FirstOrDefaultAsync();

        if (currency == null)
        {
            throw new O24OpenAPIException(
                "InvalidCurrency",
                "The currency code does not exist in system"
            );
        }
        await _currencyRepository.Delete(currency);
        return currency;
    }

    public async Task DeleteById(DeleteCurrencyByCurrencyCodeModel model)
    {
        model.ListCurrencyCode.Add(model.CurrencyCode);
        foreach (string curCode in model.ListCurrencyCode)
        {
            var currency = await _currencyRepository
                .Table.Where(s => s.CurrencyCode.Equals(curCode))
                .FirstOrDefaultAsync();

            if (currency != null)
            {
                await _currencyRepository.Delete(currency);
            }
        }
    }
}
