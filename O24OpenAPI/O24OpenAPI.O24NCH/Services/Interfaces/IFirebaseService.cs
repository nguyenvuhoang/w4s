using O24OpenAPI.O24NCH.Models.Request;

namespace O24OpenAPI.O24NCH.Services.Interfaces;

public interface IFirebaseService
{
    Task<bool> GenereateFirebaseNotificationAsync(FirebaseNotificationRequestModel model);
}
