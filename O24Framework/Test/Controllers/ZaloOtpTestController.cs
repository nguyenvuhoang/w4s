using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Test.Features.Otp;

namespace Test.Controllers;

[ApiController]
[Route("api/otp")]
public sealed class OtpController : ControllerBase
{
    private readonly OtpService _otp;
    private readonly IOptionsMonitor<OtpOptions> _opt;

    public OtpController(OtpService otp, IOptionsMonitor<OtpOptions> opt)
    {
        _otp = otp;
        _opt = opt;
    }

    [HttpPost("request")]
    public async Task<IActionResult> Request([FromBody] RequestDto dto, CancellationToken ct)
    {
        try
        {
            var r = await _otp.RequestAsync(dto.Phone, dto.Purpose, ct);

            if (_opt.CurrentValue.ReturnOtpInResponse)
            {
                return Ok(new { ok = true, r.phone, r.purpose, r.ttlSeconds, otp = r.otp }); // DEV only
            }

            // “giống thật”: không trả OTP
            return Ok(new { ok = true, r.phone, r.purpose, r.ttlSeconds });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(429, new { ok = false, reason = ex.Message });
        }
    }

    [HttpPost("verify")]
    public IActionResult Verify([FromBody] VerifyDto dto)
    {
        var ok = _otp.Verify(dto.Phone, dto.Purpose, dto.Otp);
        return ok ? Ok(new { ok = true }) : Unauthorized(new { ok = false });
    }

    public sealed record RequestDto(string Phone, string? Purpose);
    public sealed record VerifyDto(string Phone, string? Purpose, string Otp);
}
