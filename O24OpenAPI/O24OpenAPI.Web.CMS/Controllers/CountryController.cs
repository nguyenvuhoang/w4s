using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.GrpcContracts.GrpcClientServices.WFO;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Controllers;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;

namespace O24OpenAPI.Web.CMS.Controllers;

public class CountryController : BaseController
{
    /// <summary>
    /// The web portal service
    /// </summary>
    private readonly ICountryService _CountryService;

    /// <summary>
    ///
    /// </summary>
    /// <param name="countryService"></param>
    public CountryController(ICountryService countryService)
    {
        _CountryService = countryService;
    }

    /// <summary>
    /// Posts the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the action result</returns>
    [HttpPost]
    public virtual async Task<IActionResult> GetAll()
    {
        var result = await _CountryService.GetAll();
        return Ok(result);
    }

    /// <summary>
    /// Posts the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the action result</returns>
    [HttpPost]
    public virtual async Task<IActionResult> SearchSimple([FromBody] SimpleSearchModel model)
    {
        var result = await _CountryService.SimpleSearch(model);
        return Ok(result);
    }

    /// <summary>
    /// Posts the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the action result</returns>
    [HttpPost]
    public virtual async Task<IActionResult> SearchAdvance(
        [FromBody] CountrySearchAdvanceModel model
    )
    {
        var result = await _CountryService.SearchAdvance(model);
        return Ok(result);
    }

    /// <summary>
    /// Posts the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the action result</returns>
    [HttpPost]
    public virtual async Task<IActionResult> Insert([FromBody] CountryInsertModel model)
    {
        var result = await _CountryService.Insert(model.FromModel<D_COUNTRY>());
        return Ok(result);
    }

    /// <summary>
    /// Posts the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the action result</returns>
    [HttpPost]
    public virtual async Task<IActionResult> Update(CountryUpdateModel model)
    {
        var result = await _CountryService.Update(model.FromModel<D_COUNTRY>());
        return Ok(result);
    }

    /// <summary>
    /// Posts the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the action result</returns>
    [HttpPost]
    public virtual async Task<IActionResult> Delete(ModelWithId model)
    {
        var result = await _CountryService.DeleteById(model.Id);
        return Ok(result);
    }

    [HttpPost]
    public virtual async Task<IActionResult> TestGrpcStream(string name)
    {
        var wfoGrpcClient = EngineContext.Current.Resolve<IWFOGrpcClientService>();
        await wfoGrpcClient.SayHelloAsync(name);
        return Ok();
    }
}
