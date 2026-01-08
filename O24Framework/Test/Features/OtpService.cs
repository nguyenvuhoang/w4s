using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;

namespace Test.Features.Otp;

public sealed class OtpService
{
    private readonly IMemoryCache _cache;
    private readonly IOtpSender _sender;

    public OtpService(IMemoryCache cache, IOtpSender sender)
    {
        _cache = cache;
        _sender = sender;
    }

    public async Task<(string phone, string purpose, int ttlSeconds, string otp)> RequestAsync(
        string phoneInput, string? purpose, CancellationToken ct)
    {
        var phone = NormalizeVnPhoneToE164NoPlus(phoneInput);
        var p = string.IsNullOrWhiteSpace(purpose) ? "register" : purpose.Trim().ToLowerInvariant();
        var ttlSeconds = 120;

        // rate-limit đơn giản
        var rlKey = $"otp:rl:{phone}:{p}";
        var count = _cache.TryGetValue(rlKey, out int c) ? c : 0;
        if (count >= 5) throw new InvalidOperationException("Too many requests, try later.");
        _cache.Set(rlKey, count + 1, TimeSpan.FromMinutes(10));

        var otp = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
        var hash = Sha256Hex(otp);

        _cache.Set($"otp:{phone}:{p}", hash, TimeSpan.FromSeconds(ttlSeconds));
        _cache.Set($"otp:attempts:{phone}:{p}", 0, TimeSpan.FromMinutes(10));

        await _sender.SendAsync(phone, $"OTP={otp} (ttl={ttlSeconds}s, purpose={p})", ct);

        return (phone, p, ttlSeconds, otp); // otp trả về chỉ để demo/mock
    }

    public bool Verify(string phoneInput, string? purpose, string otp)
    {
        var phone = NormalizeVnPhoneToE164NoPlus(phoneInput);
        var p = string.IsNullOrWhiteSpace(purpose) ? "register" : purpose.Trim().ToLowerInvariant();

        var key = $"otp:{phone}:{p}";
        if (!_cache.TryGetValue(key, out string? expectedHash) || string.IsNullOrWhiteSpace(expectedHash))
            return false;

        var attemptsKey = $"otp:attempts:{phone}:{p}";
        var attempts = _cache.TryGetValue(attemptsKey, out int a) ? a : 0;
        if (attempts >= 5) return false;

        var ok = string.Equals(expectedHash, Sha256Hex(otp), StringComparison.OrdinalIgnoreCase);
        if (!ok)
        {
            _cache.Set(attemptsKey, attempts + 1, TimeSpan.FromMinutes(10));
            return false;
        }

        // one-time use
        _cache.Remove(key);
        _cache.Remove(attemptsKey);
        return true;
    }

    private static string Sha256Hex(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }

    private static string NormalizeVnPhoneToE164NoPlus(string phone)
    {
        var p = (phone ?? "").Trim().Replace(" ", "").Replace("-", "");
        if (p.StartsWith("+")) p = p[1..];
        p = new string(p.Where(char.IsDigit).ToArray());
        if (p.StartsWith("0")) p = "84" + p[1..];
        return p;
    }
}
