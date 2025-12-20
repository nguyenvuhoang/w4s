using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Web.CMS.Constant;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Models.Request;
using O24OpenAPI.Web.CMS.Models.Response;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Controllers;

public partial class CoreAPIController(
    IWebChannelService webChannelService,
    ICoreAPIService coreAPIService,
    IWebHostEnvironment env
) : BaseController
{
    readonly IWebChannelService _webChannelService = webChannelService;
    readonly ICoreAPIService _coreAPIService = coreAPIService;
    readonly IWebHostEnvironment _env = env;

    [HttpPost("/coreapi/auth/get-static-token")]
    public async Task<IActionResult> GetStaticToken([FromBody] GetStaticTokenModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await _coreAPIService.ValidateClientAsync(
                model.ClientId,
                model.ClientSecret
            );
            if (client == null)
            {
                return UnauthorizedProblem("Invalid client credentials");
            }

            foreach (var scope in model.Scopes ?? [])
            {
                if (!await _coreAPIService.IsScopeGrantedAsync(client, scope))
                {
                    return StatusCode(
                        403,
                        new ProblemDetails
                        {
                            Status = 403,
                            Title = "Forbidden",
                            Detail = $"Client is not granted scope: {scope}",
                            Type = "https://httpstatuses.com/403",
                        }
                    );
                }
            }

            var (remoteIp, ipSource) = GetRemoteIpWithSource(HttpContext);

            var accessList = SplitIps(client.AccessTokenTrustedIPs);
            if (accessList.Length > 0 && !accessList.Contains(remoteIp))
            {
                return ForbiddenIp(
                    policyName: "AccessTokenTrustedIPs",
                    remoteIp: remoteIp,
                    source: ipSource,
                    trustedIps: accessList,
                    includeListInDetail: _env.IsDevelopment()
                );
            }

            // Check ClientSecretTrustedIPs
            var secretList = SplitIps(client.ClientSecretTrustedIPs);
            if (secretList.Length > 0 && !secretList.Contains(remoteIp))
            {
                return ForbiddenIp(
                    policyName: "ClientSecretTrustedIPs",
                    remoteIp: remoteIp,
                    source: ipSource,
                    trustedIps: secretList,
                    includeListInDetail: _env.IsDevelopment()
                );
            }

            var tokenEntry = await _coreAPIService.GenerateAndSaveTokenAsync(
                client,
                scopes: model.Scopes,
                expiredOnUtc: model.ExpiredOnUtc
            );

            return Ok(tokenEntry);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("/coreapi/auth/refresh-static-token")]
    public async Task<IActionResult> RefreshStaticToken([FromBody] RefreshStaticTokenModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var tokenEntry = await _coreAPIService.GetValidRefreshTokenAsync(
            model.ClientId,
            model.RefreshToken
        );
        if (tokenEntry == null)
        {
            return Unauthorized("Invalid or expired refresh token.");
        }

        tokenEntry.IsRevoked = true;

        var client = await _coreAPIService.GetClientByIdAsync(model.ClientId);

        var newTokenEntry = await _coreAPIService.GenerateAndSaveTokenAsync(
            client,
            scopes: tokenEntry
                .Scopes.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => Enum.Parse<StaticTokenScope>(s))
                .ToList()
        );

        await _coreAPIService.RevokeTokenAsync(tokenEntry);

        return Ok(newTokenEntry);
    }

    /// <summary>
    /// Execute
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("/coreapi/execute")]
    public virtual async Task<IActionResult> Execute([FromBody] CoreAPIRequestModel model)
    {
        try
        {
            // 1) Lấy bearer token
            var authHeader = HttpContext.Request.Headers.Authorization.FirstOrDefault();
            if (
                string.IsNullOrEmpty(authHeader)
                || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            )
            {
                var error = Utils.Utils.AddActionError(
                    ErrorType.errorSystem,
                    ErrorMainForm.danger,
                    "Missing or invalid Bearer Token",
                    "AUTH_ERROR",
                    "#AUTH: Missing or invalid Bearer Token"
                );

                return Unauthorized(
                    new CoreAPIResponseModel
                    {
                        WorkflowId = model?.WorkflowId,
                        Data = null,
                        Error = error,
                    }
                );
            }

            var token = authHeader["Bearer ".Length..].Trim();
            if (string.IsNullOrWhiteSpace(token))
            {
                var error = Utils.Utils.AddActionError(
                    ErrorType.errorSystem,
                    ErrorMainForm.danger,
                    "Bearer Token is empty",
                    "AUTH_ERROR",
                    "#AUTH: Bearer Token is empty"
                );

                return Unauthorized(
                    new CoreAPIResponseModel
                    {
                        WorkflowId = model?.WorkflowId,
                        Data = null,
                        Error = error,
                    }
                );
            }

            // 2) Tra token trong DB
            var coreApiToken = await _coreAPIService.GetByToken(token);
            if (coreApiToken == null)
            {
                var error = Utils.Utils.AddActionError(
                    ErrorType.errorSystem,
                    ErrorMainForm.danger,
                    "Invalid or expired Bearer Token",
                    "AUTH_ERROR",
                    "#AUTH: Invalid or expired Bearer Token"
                );
                return Unauthorized(
                    new CoreAPIResponseModel
                    {
                        WorkflowId = model?.WorkflowId,
                        Data = null,
                        Error = error,
                    }
                );
            }

            var biccd = coreApiToken.BICCD;
            var clientId = coreApiToken.ClientId;

            // 3) Lấy remote IP + nguồn (X-Forwarded-For hay RemoteIpAddress)
            var (remoteIp, ipSource) = GetRemoteIpWithSource(HttpContext);

            // 4) Lấy client để đọc whitelist IP
            var client = await _coreAPIService.GetByClientIdAndBICCD(clientId, biccd);
            if (client == null)
            {
                var error = Utils.Utils.AddActionError(
                    ErrorType.errorSystem,
                    ErrorMainForm.danger,
                    "Invalid client token credentials",
                    "AUTH_ERROR",
                    "#AUTH: Invalid client for token"
                );

                return Unauthorized(
                    new CoreAPIResponseModel
                    {
                        WorkflowId = model?.WorkflowId,
                        Data = null,
                        Error = error,
                    }
                );
            }

            // 5) Check whitelist IP (ưu tiên tách 2 policy để báo lỗi rõ ràng)
            var accessList = SplitIps(client.AccessTokenTrustedIPs);
            if (accessList.Length > 0 && !accessList.Contains(remoteIp))
            {
                return ForbiddenIp(
                    policyName: "AccessTokenTrustedIPs",
                    remoteIp: remoteIp,
                    source: ipSource,
                    trustedIps: accessList,
                    includeListInDetail: _env.IsDevelopment()
                );
            }

            var secretList = SplitIps(client.ClientSecretTrustedIPs);
            if (secretList.Length > 0 && !secretList.Contains(remoteIp))
            {
                return ForbiddenIp(
                    policyName: "ClientSecretTrustedIPs",
                    remoteIp: remoteIp,
                    source: ipSource,
                    trustedIps: secretList,
                    includeListInDetail: _env.IsDevelopment()
                );
            }

            // 6) Validate model
            if (!ModelState.IsValid)
            {
                var errorList = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .Select(x =>
                        Utils.Utils.AddActionError(
                            ErrorType.errorSystem,
                            ErrorMainForm.danger,
                            x.Value!.Errors.First().ErrorMessage,
                            "VALIDATION_ERROR",
                            "#VALIDATION: " + x.Value!.Errors.First().ErrorMessage
                        )
                    )
                    .ToList();

                return BadRequest(
                    new CoreAPIResponseModel
                    {
                        WorkflowId = model?.WorkflowId,
                        Data = null,
                        Error = errorList.FirstOrDefault(),
                    }
                );
            }

            // 7) Bổ sung BICCD vào payload
            if (!string.IsNullOrWhiteSpace(coreApiToken.BICCD))
            {
                model.Data ??= [];
                model.Data["biccd"] = coreApiToken.BICCD;
            }

            // 8) Build BO request
            var bo = new BoRequestModel
            {
                Bo =
                [
                    new BoRequest
                    {
                        UseMicroservice = true,
                        Input = new Dictionary<string, object>
                        {
                            { "workflowid", model.WorkflowId },
                            { "learn_api", "cbs_workflow_execute" },
                            { "fields", model.Data },
                        },
                    },
                ],
            };

            // 9) Đảm bảo header app
            var app = HttpContext.Request.Headers["app"].FirstOrDefault();
            if (string.IsNullOrEmpty(app))
            {
                app = AppCode.CoreAPI;
                HttpContext.Request.Headers["app"] = app;
            }

            // 10) Gọi kênh xử lý
            ActionsResponseModel<object> response = await _webChannelService.StartRequest(
                bo,
                HttpContext
            );

            // 11) Trả kết quả
            return Ok(BuildCoreAPIResponse(model.WorkflowId, response));
        }
        catch (Exception ex)
        {
            var error = Utils.Utils.AddActionError(
                ErrorType.errorSystem,
                ErrorMainForm.danger,
                ex.Message,
                "ERROR",
                "#ERROR_SYSTEM: "
            );

            return StatusCode(
                500,
                new
                {
                    Status = 500,
                    Message = "Internal Server Error",
                    Response = new CoreAPIResponseModel
                    {
                        WorkflowId = model?.WorkflowId,
                        Data = null,
                        Error = error,
                    },
                }
            );
        }
    }

    private static CoreAPIResponseModel BuildCoreAPIResponse(
        string workflowId,
        ActionsResponseModel<object> response
    )
    {
        var error = response.error?.FirstOrDefault();
        return new CoreAPIResponseModel
        {
            WorkflowId = workflowId,
            Data = error != null ? null : response.fo?.FirstOrDefault()?.input,
            Error = error,
        };
    }

    private static (string ip, string source) GetRemoteIpWithSource(HttpContext ctx)
    {
        if (ctx.Request.Headers.TryGetValue("X-Forwarded-For", out var fwd))
        {
            var first = fwd.ToString().Split(',')[0].Trim();
            if (!string.IsNullOrWhiteSpace(first))
            {
                return (first, "X-Forwarded-For");
            }
        }
        return (ctx.Connection.RemoteIpAddress?.ToString() ?? string.Empty, "RemoteIpAddress");
    }

    private static string[] SplitIps(string? trustedIpsString) =>
        string.IsNullOrWhiteSpace(trustedIpsString)
            ? Array.Empty<string>()
            : trustedIpsString.Split(
                ';',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            );

    private IActionResult ForbiddenIp(
        string policyName,
        string remoteIp,
        string source,
        IEnumerable<string> trustedIps,
        bool includeListInDetail
    )
    {
        var detail = includeListInDetail
            ? $"Blocked by {policyName}. Remote IP {remoteIp} (source: {source}) is not in: {string.Join(", ", trustedIps)}"
            : $"Blocked by {policyName}. Remote IP {remoteIp} (source: {source}) is not allowed.";

        Response.Headers["X-Block-Reason"] = detail;
        Response.Headers["X-Client-IP"] = remoteIp;
        Response.Headers["X-IP-Source"] = source;

        return StatusCode(
            403,
            new ProblemDetails
            {
                Status = 403,
                Title = "Forbidden",
                Type = "https://httpstatuses.com/403",
                Detail = detail,
            }
        );
    }

    private IActionResult UnauthorizedProblem(string detail, string? instance = null)
    {
        var pd = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Detail = detail,
            Type = "https://httpstatuses.com/401",
            Instance = instance ?? HttpContext?.Request?.Path.Value,
        };

        // Thêm metadata tuỳ ý
        pd.Extensions["traceId"] = HttpContext?.TraceIdentifier;
        pd.Extensions["timestamp"] = DateTimeOffset.UtcNow;

        return StatusCode(StatusCodes.Status401Unauthorized, pd);
    }
}
