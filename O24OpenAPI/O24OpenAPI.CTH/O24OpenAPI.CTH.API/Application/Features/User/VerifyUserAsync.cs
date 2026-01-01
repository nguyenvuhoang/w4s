using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class VerifyUserAsyncCommand : BaseTransactionModel, ICommand<VerifyUserResponseModel>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}

[CqrsHandler]
public class VerifyUserAsyncHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<VerifyUserAsyncCommand, VerifyUserResponseModel>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_VERIFY_USER)]
    public async Task<VerifyUserResponseModel> HandleAsync(
        VerifyUserAsyncCommand request,
        CancellationToken cancellationToken = default
    )
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Username))
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UserNameAndEmailIsRequired,
                request.Language
            );
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            request.Email = "";
        }

        try
        {
            var userAccount = await userAccountRepository.GetByLoginNameAndEmailAsync(
                request.Username,
                request.Email,
                request.PhoneNumber
            );
            return userAccount == null
                ? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UsernameIsNotExist,
                    request.Language
                )
                : new VerifyUserResponseModel
                {
                    IsVerified = true,
                    ContractNumber = userAccount.ContractNumber,
                    UserCode = userAccount.UserCode,
                };
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            Console.WriteLine($"VerifyUser=Exception={ex.Message}\nStackTrace={ex.StackTrace}");
            throw;
        }
    }
}
