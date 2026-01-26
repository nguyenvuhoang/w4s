using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class UpdateUserCurrencyCommnad : BaseTransactionModel, ICommand<bool>
{
    public string UserCode { get; set; }
    public string CurrencyCode { get; set; }
}

[CqrsHandler]
public class UpdateUserCurrencyHandler(IUserAccountRepository userAccountRepository)
    : ICommandHandler<UpdateUserCurrencyCommnad, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_UPDATE_CURRENCY_CODE)]
    public async Task<bool> HandleAsync(
        UpdateUserCurrencyCommnad request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            UserAccount entity = await userAccountRepository.GetByUserCodeAsync(request.UserCode);

            if (entity != null)
            {
                entity.CurrencyCode = request.CurrencyCode;
                await userAccountRepository.Update(entity);
            }

            return true;
        }
        catch (O24Exception)
        {
            throw;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync(
                $"[UpdateUserCurrencyAsync] Error for {request.UserCode}: {ex.Message}"
            );
            throw await O24Exception.CreateAsync(
                ResourceCode.Common.SystemError,
                request.Language,
                ex
            );
        }
    }
}
