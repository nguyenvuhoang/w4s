using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using Test.Features;

namespace Test.Controllers;

[ApiController]
[Route("api/zalo-otp")]
public sealed class ZaloOtpController : ControllerBase
{
    private readonly IMemoryCache _cache;
    private readonly ZaloZnsClient _zns;

    public ZaloOtpController(IMemoryCache cache, ZaloZnsClient zns)
    {
        _cache = cache;
        _zns = zns;
    }

    [HttpPost("request")]
    public async Task<IActionResult> RequestOtp([FromBody] RequestOtpDto dto, CancellationToken ct)
    {
        var phone = NormalizeVnPhoneToE164NoPlus(dto.Phone);
        var otp = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");

        // cache 2 phút
        _cache.Set($"otp:{phone}", otp, TimeSpan.FromMinutes(2));

        var zaloRes = await _zns.SendOtpAsync(phone, otp, ct);
        return Ok(new { phone, ttlSeconds = 120, zalo = zaloRes });
    }

    [HttpPost("verify")]
    public IActionResult VerifyOtp([FromBody] VerifyOtpDto dto)
    {
        var phone = NormalizeVnPhoneToE164NoPlus(dto.Phone);

        if (!_cache.TryGetValue($"otp:{phone}", out string? expected) || string.IsNullOrWhiteSpace(expected))
            return Unauthorized(new { ok = false, reason = "OTP expired/not found" });

        if (!string.Equals(expected, dto.Otp, StringComparison.Ordinal))
            return Unauthorized(new { ok = false, reason = "OTP invalid" });

        _cache.Remove($"otp:{phone}");
        return Ok(new { ok = true });
    }

    private static string NormalizeVnPhoneToE164NoPlus(string phone)
    {
        var p = (phone ?? "").Trim().Replace(" ", "").Replace("-", "");
        if (p.StartsWith("+")) p = p[1..];

        // VN: 0xxxxxxxxx => 84xxxxxxxxx
        if (p.StartsWith("0")) p = "84" + p[1..];

        // đơn giản: chỉ giữ số
        p = new string(p.Where(char.IsDigit).ToArray());

        return p;
    }

    [ApiController]
    [Route("api/zalo-debug")]
    public class ZaloDebugController : ControllerBase
    {
        private readonly ZaloTokenProvider _tp;
        public ZaloDebugController(ZaloTokenProvider tp) => _tp = tp;

        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken(CancellationToken ct)
        {
            var at = await _tp.GetAccessTokenAsync(ct);
            return Ok(new { access_token = at });
        }
    }

    public sealed record RequestOtpDto(string Phone);
    public sealed record VerifyOtpDto(string Phone, string Otp);
}
