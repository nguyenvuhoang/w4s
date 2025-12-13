using O24OpenAPI.Core;
using O24OpenAPI.Web.Framework.Domain;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.Web.Framework.Services.Configuration;

/// <summary>
/// The io 24 open api mapping service interface
/// </summary>
public interface IO24OpenAPIMappingService
{
    /// <summary>
    /// Gets a Neptune mapping by identifier
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<O24OpenAPIService> GetById(int id);

    /// <summary>
    /// Get all Neptune mapping
    /// </summary>
    /// <returns></returns>
    Task<IList<O24OpenAPIService>> GetAll();

    /// <summary>
    /// Adds the o 24 open api service
    /// </summary>
    /// <param name="o24OpenAPIService">The 24 open api service</param>
    /// <returns>A task containing the 24 open api service</returns>
    Task<O24OpenAPIService> AddAsync(O24OpenAPIService o24OpenAPIService);
    Task UpdateAsync(O24OpenAPIService o24OpenAPIService);
    Task DeleteAsync(O24OpenAPIService o24OpenAPIService);
    Task<IPagedList<O24OpenAPIService>> SimpleSearch(SimpleSearchModel model);
    /// <summary>
    /// Get Neptune service mapping by step code
    /// </summary>
    /// <param name="stepCode"></param>
    /// <returns></returns>
    Task<O24OpenAPIService> GetByStepCode(string stepCode);

    /// <summary>
    /// Get neptune service mappings by array of step codes
    /// </summary>
    /// <param name="stepCodes"></param>
    /// <returns></returns>
    Task<IList<O24OpenAPIService>> GetByStepCodes(string[] stepCodes);


}
