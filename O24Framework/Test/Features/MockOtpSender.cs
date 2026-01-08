using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Test.Features.Otp;

public sealed class MockOtpSender : IOtpSender
{
    private readonly ILogger<MockOtpSender> _logger;
    private readonly IOptionsMonitor<OtpOptions> _opt;

    public MockOtpSender(ILogger<MockOtpSender> logger, IOptionsMonitor<OtpOptions> opt)
    {
        _logger = logger;
        _opt = opt;
    }

    public Task SendAsync(string phoneE164NoPlus, string content, CancellationToken ct = default)
    {
        if (_opt.CurrentValue.LogOtp)
            _logger.LogWarning("[MOCK_OTP] To={Phone} | {Content}", phoneE164NoPlus, content);

        return Task.CompletedTask;
    }
}
