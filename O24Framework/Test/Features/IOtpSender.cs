namespace Test.Features.Otp;

public interface IOtpSender
{
    Task SendAsync(string phoneE164NoPlus, string content, CancellationToken ct = default);
}
