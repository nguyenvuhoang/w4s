using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Models.Request.Mail;

namespace O24OpenAPI.O24NCH.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(SendMailRequestModel model);
    Task<bool> TestEmailAsync(TestMailRequestModel model);
}
