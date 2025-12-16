using LinqToDB;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class CityService : ICityService
{
    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_CITY> _cityRepository;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="cityRepository"></param>
    public CityService(ILocalizationService localizationService, IRepository<D_CITY> cityRepository)
    {
        _localizationService = localizationService;
        _cityRepository = cityRepository;
    }

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <param name="RegionName">The id</param>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_CITY> GetByCityID(int CityID)
    {
        return await _cityRepository.Table.Where(s => s.Id == CityID).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <param name="CityName">The id</param>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_CITY> GetByCityName(string CityName)
    {
        return await _cityRepository.Table.Where(s => s.CityName == CityName).FirstOrDefaultAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_CITY>> GetAll()
    {
        return await _cityRepository.Table.Select(s => s).ToListAsync();
    }

    /// <summary>
    /// s
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_CITY>> Search(SearchCityModel model)
    {
        var listCity = await (
            from d in _cityRepository.Table
            where
                (
                    (!string.IsNullOrEmpty(model.CityCode) && model.CityCode == d.CityCode)
                    || string.IsNullOrEmpty(model.CityCode)
                )
                && (
                    (!string.IsNullOrEmpty(model.CityName) && model.CityName == d.CityName)
                    || string.IsNullOrEmpty(model.CityName)
                )
                && (
                    (!string.IsNullOrEmpty(model.Description) && model.Description == d.Description)
                    || string.IsNullOrEmpty(model.Description)
                )

            select d
        ).ToListAsync();

        return listCity;
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_CITY> Insert(D_CITY city)
    {
        var findCity = await _cityRepository
            .Table.Where(s => s.CityCode.Equals(city.CityCode))
            .FirstOrDefaultAsync();
        if (findCity == null)
        {
            await _cityRepository.Insert(city);
        }
        else
        {
            throw new O24OpenAPIException(
                "InvalidCity",
                "The city code already existing in system"
            );
        }

        return city;
    }

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_CITY> Update(D_CITY city)
    {
        var checkCityCode = await _cityRepository
            .Table.Where(s => s.Id != city.Id && s.CityCode == city.CityCode)
            .FirstOrDefaultAsync();

        if (checkCityCode != null)
        {
            throw new O24OpenAPIException("InvalidCityCode", "The city code is unique");
        }

        await _cityRepository.Update(city);
        return city;
    }

    /// <summary>
    ///Delete.
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_CITY> DeleteById(int CityID)
    {
        var city = await _cityRepository.Table.Where(s => s.Id == CityID).FirstOrDefaultAsync();

        if (city == null)
        {
            throw new O24OpenAPIException("Invalidcity", "The city does not exist in system");
        }
        await _cityRepository.Delete(city);
        return city;
    }
}
