using System.Security.Cryptography;
using O24OpenAPI.CMS.API.Application.Models.OpenAPI;
using O24OpenAPI.CMS.API.Application.Models.Request;
using O24OpenAPI.CMS.API.Application.Models.Response;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.CMS.API.Application.Utils;
using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

public partial class CoreAPIService(
    WebApiSettings webApiSettings,
    IRepository<CoreApiKeys> coreAPIKey,
    IRepository<CoreApiToken> coreApiToken
) : ICoreAPIService
{
    private readonly WebApiSettings _webApiSettings = webApiSettings;
    private readonly IRepository<CoreApiKeys> _coreAPIKey = coreAPIKey;
    private readonly IRepository<CoreApiToken> _coreApiToken = coreApiToken;
    private static readonly string[] valuesArray = ["ClientId", "BICCode"];

    /// <summary>
    /// Get all API keys with pagination support.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<CoreApiKeys> GetById(int id)
    {
        return await _coreAPIKey.GetById(id, cache => default);
    }

    /// <summary>
    /// Get client by client ID
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    public async Task<CoreApiKeys> GetClientByIdAsync(string clientId)
    {
        return await _coreAPIKey
            .Table.Where(x =>
                x.ClientId == clientId
                && !x.IsRevoked
                && x.IsActive
                && x.ExpiredOnUtc > DateTime.UtcNow
            )
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Validate client credentials and check if the API key is valid.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <returns></returns>
    public async Task<CoreApiKeys> ValidateClientAsync(string clientId, string clientSecret)
    {
        return await _coreAPIKey
            .Table.Where(x =>
                x.ClientId == clientId
                && x.ClientSecret == clientSecret
                && !x.IsRevoked
                && x.IsActive
                && x.ExpiredOnUtc > DateTime.UtcNow
            )
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Is the given scope granted for the API key?
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    public Task<bool> IsScopeGrantedAsync(CoreApiKeys apiKey, StaticTokenScope scope)
    {
        if (string.IsNullOrWhiteSpace(apiKey.Scopes))
        {
            return Task.FromResult(false);
        }

        var grantedScopes = apiKey
            .Scopes.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim().ToUpper())
            .ToHashSet();

        return Task.FromResult(grantedScopes.Contains(scope.ToString().ToUpper()));
    }

    /// <summary>
    /// Generate and save a static token for the given client ID and secret with specified scopes.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="scopes"></param>
    /// <param name="expiredOnUtc"></param>
    /// <returns></returns>
    public async Task<CoreApiTokenResponse> GenerateAndSaveTokenAsync(
        CoreApiKeys apiKey,
        List<StaticTokenScope> scopes,
        DateTime? expiredOnUtc = null
    )
    {
        var expiredAt =
            expiredOnUtc ?? DateTime.UtcNow.AddDays(_webApiSettings.StaticTokenLifetimeDays);
        var refreshToken = Guid.NewGuid().ToString();
        var refreshExpiredAt = DateTime.UtcNow.AddDays(_webApiSettings.StaticTokenLifetimeDays * 2);

        const int maxAttempts = 5;
        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            var token = JWT.GenerateStaticTokenJwt(
                apiKey.ClientId,
                apiKey.ClientSecret,
                scopes,
                expiredOnUtc
            );
            var hashedToken = token.Hash();
            var coreToken = new CoreApiToken
            {
                ClientId = apiKey.ClientId,
                Token = hashedToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiredAt = refreshExpiredAt,
                Scopes = string.Join(",", scopes.Select(s => s.ToString())),
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = expiredAt,
                BICCD = apiKey.BICCode,
            };
            try
            {
                await _coreApiToken.InsertAsync(coreToken);

                return new CoreApiTokenResponse
                {
                    AccessToken = token,
                    RefreshToken = refreshToken,
                    ExpiredAt = expiredAt,
                    ExpiredDuration = (long)(expiredAt - DateTime.UtcNow).TotalSeconds,
                };
            }
            catch (Exception ex) when (IsUniqueViolation(ex))
            {
                if (attempt == maxAttempts)
                {
                    throw;
                }

                await Task.Delay(Random.Shared.Next(15, 75));
            }
        }
        throw new InvalidOperationException("Unexpected flow in GenerateAndSaveTokenAsync.");
    }

    /// <summary>
    /// Is the exception caused by a unique constraint violation?
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    static bool IsUniqueViolation(Exception ex)
    {
        var baseEx = ex.GetBaseException();
        var type = baseEx.GetType();
        var typeName = type.FullName ?? type.Name;

        if (
            typeName
            is "System.Data.SqlClient.SqlException"
                or "Microsoft.Data.SqlClient.SqlException"
        )
        {
            var number = (int?)type.GetProperty("Number")?.GetValue(baseEx);
            if (number is 2627 or 2601)
            {
                return true;
            }
        }

        if (typeName == "Npgsql.PostgresException")
        {
            var sqlState = type.GetProperty("SqlState")?.GetValue(baseEx)?.ToString();
            if (sqlState == "23505")
            {
                return true;
            }
        }

        if (
            typeName
            is "Oracle.ManagedDataAccess.Client.OracleException"
                or "Oracle.DataAccess.Client.OracleException"
        )
        {
            var number = (int?)type.GetProperty("Number")?.GetValue(baseEx);
            if (number == 1)
            {
                return true;
            }
        }

        if (typeName is "MySql.Data.MySqlClient.MySqlException" or "MySqlConnector.MySqlException")
        {
            var number = (int?)type.GetProperty("Number")?.GetValue(baseEx);
            if (number == 1062)
            {
                return true;
            }
        }

        if (typeName == "Microsoft.Data.Sqlite.SqliteException")
        {
            var code = (int?)type.GetProperty("SqliteErrorCode")?.GetValue(baseEx);
            if (code == 19)
            {
                return true;
            }
        }

        var msg = baseEx.Message?.ToLowerInvariant() ?? "";
        if (
            msg.Contains("unique constraint")
            || msg.Contains("duplicate key")
            || msg.Contains("unique violation")
        )
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Get valid static token by client ID and refresh token.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public async Task<CoreApiToken> GetValidRefreshTokenAsync(string clientId, string refreshToken)
    {
        return await _coreApiToken
            .Table.Where(t =>
                t.ClientId == clientId
                && t.RefreshToken == refreshToken
                && !t.IsRevoked
                && t.RefreshTokenExpiredAt > DateTime.UtcNow
            )
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Revoke a static token, marking it as revoked and preventing further use.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task RevokeTokenAsync(CoreApiToken token)
    {
        token.IsRevoked = true;
        await _coreApiToken.Update(token);
    }

    public async Task<CoreApiToken> GetByToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        try
        {
            var tokenHash = token.Hash();
            return await _coreApiToken
                .Table.Where(s => s.Token == tokenHash && !s.IsRevoked)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            return null;
        }
    }

    public async Task<IPagedList<CoreApiKeys>> SimpleSearch(SimpleSearchModel model)
    {
        var query =
            from d in _coreAPIKey.Table
            where
                (
                    !string.IsNullOrEmpty(model.SearchText)
                    && d.ClientId.Contains(model.SearchText)
                    && d.ClientSecret.Contains(model.SearchText)
                ) || true
            select d;
        return await query.ToPagedList(model.PageIndex, model.PageSize);
    }

    /// <summary>
    /// Create a new API key.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public async Task<CreateOpenAPIResponseModel> CreateAsync(CreateOpenAPIRequestModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        try
        {
            var clientId = model.ClientId?.Trim();
            var bicCode = model.BICCode?.Trim();

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw await O24Exception.CreateAsync(
                    ResourceCode.Common.BadRequest,
                    model.Language,
                    [clientId]
                );
            }

            var isClientExists = await LinqToDB.AsyncExtensions.AnyAsync(
                _coreAPIKey.Table,
                x => x.ClientId == clientId && x.BICCode == bicCode && !x.IsRevoked
            );

            if (isClientExists)
            {
                throw await O24Exception.CreateAsync(
                    ResourceCode.Common.DataExists,
                    model.Language,
                    [clientId, bicCode]
                );
            }

            var clientSecret = GenerateSecret();

            string normalizedScopes = null;
            if (!string.IsNullOrWhiteSpace(model.Scopes))
            {
                var scopeSet = model
                    .Scopes.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.ToUpperInvariant())
                    .Distinct();

                normalizedScopes = string.Join(",", scopeSet);
            }

            var entity = new CoreApiKeys
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                DisplayName = model.DisplayName,
                Environment = model.Environment,
                Scopes = normalizedScopes,
                BICCode = bicCode,

                IsActive = model.IsActive,
                IsRevoked = false,

                ExpiredOnUtc =
                    model.ExpiredOnUtc == default
                        ? DateTime.UtcNow.AddDays(_webApiSettings.StaticTokenLifetimeDays)
                        : model.ExpiredOnUtc,

                AccessTokenTtlSeconds = model.AccessTokenTtlSeconds,
                AccessTokenMaxTtlSeconds = model.AccessTokenMaxTtlSeconds,
                AccessTokenMaxUses = model.AccessTokenMaxUses,
                AccessTokenTrustedIPs = model.AccessTokenTrustedIPs,

                ClientSecretTrustedIPs = model.ClientSecretTrustedIPs,
                ClientSecretDescription = model.ClientSecretDescription,
                ClientSecretExpiresOnUtc =
                    model.ClientSecretExpiresOnUtc == default
                        ? (DateTime?)null
                        : model.ClientSecretExpiresOnUtc,

                CreatedOnUtc = DateTime.UtcNow,
                LastUsedOnUtc = null,
            };

            await _coreAPIKey.InsertAsync(entity);

            return new CreateOpenAPIResponseModel
            {
                ClientId = entity.ClientId,
                DisplayName = entity.DisplayName,
                Environment = entity.Environment,
                Scopes = entity.Scopes,
                BICCode = entity.BICCode,
                ClientSecret = clientSecret,
            };
        }
        catch (O24OpenAPIException)
        {
            throw;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw await O24Exception.CreateAsync(
                ResourceCode.Common.ServerError,
                model.Language,
                [ex.Message]
            );
        }
    }

    /// <summary>
    /// Update an existing API key.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<OpenAPIQueueResponseModel> UpdateAsync(OpenAPIQueueRequestModel model)
    {
        var entity =
            await GetById(model.Id)
            ?? throw await O24Exception.CreateAsync(ResourceCode.Common.NotExists, model.Language);

        var originalEntity = entity.Clone();

        model.ToEntityNullable(entity);

        entity.LastUsedOnUtc = DateTime.UtcNow;

        await _coreAPIKey.Update(entity);

        return OpenAPIQueueResponseModel.FromUpdatedEntity(entity, originalEntity);
    }

    /// <summary>
    /// Rotate client secret for the given ClientId.
    /// - Throws if Client not found or is revoked.
    /// - Optionally updates ClientSecretDescription / ClientSecretExpiresOnUtc if provided in the request.
    /// Returns the new plain secret (hãy hiển thị 1 lần rồi ẩn).
    /// </summary>
    public async Task<string> RotateSecretAsync(RotateSecretRequestModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.ClientId))
        {
            throw new ArgumentException("ClientId is required.", nameof(model));
        }

        var entity =
            await _coreAPIKey.Table.FirstOrDefaultAsync(x => x.ClientId == model.ClientId)
            ?? throw new InvalidOperationException($"Client '{model.ClientId}' not found.");
        if (entity.IsRevoked)
        {
            throw await O24Exception.CreateAsync(ResourceCode.Common.NotExists, model.Language);
        }

        var newSecret = GenerateSecret();

        entity.ClientSecret = newSecret;

        if (!string.IsNullOrWhiteSpace(model.ClientSecretDescription))
        {
            entity.ClientSecretDescription = model.ClientSecretDescription;
        }

        if (model.ClientSecretExpiresOnUtc.HasValue)
        {
            entity.ClientSecretExpiresOnUtc = model.ClientSecretExpiresOnUtc;
        }

        await _coreAPIKey.Update(entity);
        return newSecret;
    }

    /// <summary>
    /// Generate a cryptographically-strong Base64 secret.
    /// 32 random bytes -> 44 chars Base64 (giống mẫu của bạn).
    /// </summary>
    private static string GenerateSecret(int byteLength = 32)
    {
        var bytes = new byte[byteLength];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }

    public async Task<CoreApiKeys> GetByClientIdAndBICCD(string clientId, string biccd)
    {
        return await _coreAPIKey
            .Table.Where(x =>
                x.ClientId == clientId
                && x.BICCode == biccd
                && !x.IsRevoked
                && x.IsActive
                && x.ExpiredOnUtc > DateTime.UtcNow
            )
            .FirstOrDefaultAsync();
    }
}
