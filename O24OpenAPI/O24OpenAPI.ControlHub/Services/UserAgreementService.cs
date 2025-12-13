using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;

namespace O24OpenAPI.ControlHub.Services;

public class UserAgreementService(IRepository<UserAgreement> userAgreementRepository) : IUserAgreementService
{
    private readonly IRepository<UserAgreement> _userAgreementRepository = userAgreementRepository;
    public async Task<UserAgreement> LoadUserAgreementAsync(LoadUserAgreementRequestModel model)
    {
        return await _userAgreementRepository.Table
            .Where(s => s.IsActive && s.TransactionCode == model.TransactionCode)
            .FirstOrDefaultAsync();
    }
}
