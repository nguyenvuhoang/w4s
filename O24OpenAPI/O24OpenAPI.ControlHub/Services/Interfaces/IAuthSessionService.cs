using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface IAuthSessionService
{
    Task<AuthResponseModel> CreateTokenAndSessionAsync(UserAccount userAccount, LoginContextModel context);
}
