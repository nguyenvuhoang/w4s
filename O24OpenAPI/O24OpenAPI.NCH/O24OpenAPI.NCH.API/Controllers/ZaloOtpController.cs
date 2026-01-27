using Microsoft.AspNetCore.Mvc;

namespace O24OpenAPI.NCH.API.Controllers;


public class ZaloOtpController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("NCH Zalo OTP Test OK");
    }

    // GET https://localhost:5090/api/zalo-otp-test/hash-otp?otp=123456
    // chỉ để test hash, không dùng framework cũ
    [HttpGet("hash-otp")]
    public IActionResult HashOtp([FromQuery] string otp)
    {
        if (string.IsNullOrWhiteSpace(otp))
            return BadRequest("otp is required");

        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(otp);
        var hashBytes = sha.ComputeHash(bytes);
        var hash = Convert.ToHexString(hashBytes);

        return Ok(hash);
    }
}
