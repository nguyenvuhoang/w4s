using O24OpenAPI.CMS.API.Application.Models.OpenAPI;
using O24OpenAPI.CMS.API.Application.Models.Request;
using O24OpenAPI.CMS.API.Application.Models.Response;
using O24OpenAPI.CMS.Domain.AggregateModels;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public interface ICoreAPIService
{
    /// <summary>
    /// Get all API keys with pagination support.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<CoreApiKeys> GetById(int id);

    /// <summary>
    /// Get client by client ID
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    Task<CoreApiKeys> GetClientByIdAsync(string clientId);

    /// <summary>
    /// Validation client credentials
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <returns></returns>
    Task<CoreApiKeys> ValidateClientAsync(string clientId, string clientSecret);

    /// <summary>
    /// Is scope granted for API key
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    Task<bool> IsScopeGrantedAsync(CoreApiKeys apiKey, StaticTokenScope scope);

    /// <summary>
    /// Generate and save a static token for the given client ID and secret with specified scopes.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="scopes"></param>
    /// <param name="expiredOnUtc"></param>
    /// <returns></returns>
    Task<CoreApiTokenResponse> GenerateAndSaveTokenAsync(
        CoreApiKeys apiKey,
        List<StaticTokenScope> scopes,
        DateTime? expiredOnUtc = null
    );

    /// <summary>
    /// Get valid static token by client ID and token string.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    Task<CoreApiToken> GetValidRefreshTokenAsync(string clientId, string refreshToken);

    /// <summary>
    /// Revoke a static token, marking it as revoked and preventing further use.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task RevokeTokenAsync(CoreApiToken token);

    /// <summary>
    /// Get By Token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<CoreApiToken> GetByToken(string token);

    /// <summary>
    /// Simple search for API tokens based on the provided model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<CoreApiKeys>> SimpleSearch(SimpleSearchModel model);

    /// <summary>
    /// Update an existing API key or create a new one if it doesn't exist.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<OpenAPIQueueResponseModel> UpdateAsync(OpenAPIQueueRequestModel model);

    /// <summary>
    /// Rotate the secret for an existing API key, generating a new secret and updating the key's record.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<string> RotateSecretAsync(RotateSecretRequestModel model);

    /// <summary>
    /// Create a new API key with the provided details and return the created key information.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<CreateOpenAPIResponseModel> CreateAsync(CreateOpenAPIRequestModel model);

    /// <summary>
    /// Get by client id and biccd
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="biccd"></param>
    /// <returns></returns>
    Task<CoreApiKeys> GetByClientIdAndBICCD(string clientId, string biccd);
}
