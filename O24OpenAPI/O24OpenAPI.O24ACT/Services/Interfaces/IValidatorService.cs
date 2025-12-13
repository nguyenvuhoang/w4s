using O24OpenAPI.O24ACT.Models;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public interface IValidatorService
{
    Task FundTransferValidator(FundTransferModel model);
}
