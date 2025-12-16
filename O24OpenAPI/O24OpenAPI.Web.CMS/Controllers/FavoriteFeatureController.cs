using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.Web.CMS.Controllers;

public class FavoriteFeatureController: BaseController
{

    /// <summary>
    /// The web portal service
    /// </summary>
    private readonly IFavoriteFeatureService _favoriteFeatureService;
    private readonly IUserFavoriteFeatureService _userFavoriteFeatureService;



    /// <summary>
    ///
    /// </summary>
    /// <param name="countryService"></param>
    public FavoriteFeatureController(IFavoriteFeatureService favoriteFeatureService, IUserFavoriteFeatureService userFavoriteFeatureService)
    {
        _favoriteFeatureService = favoriteFeatureService;
        _userFavoriteFeatureService = userFavoriteFeatureService;
    }

    /// <summary>
    /// Posts the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the action result</returns>
    ///

    [HttpPost]
    public virtual async Task<ActionResult> GetAllFavoriteFeature()
    {
        var result = await _favoriteFeatureService.GetAll();
        return Ok(result);
    }

    [HttpPost]
    public virtual async Task<ActionResult> GetAllUserFavoriteFeature()
    {
        var result = await _userFavoriteFeatureService.GetAll();
        return Ok(result);
    }



}
