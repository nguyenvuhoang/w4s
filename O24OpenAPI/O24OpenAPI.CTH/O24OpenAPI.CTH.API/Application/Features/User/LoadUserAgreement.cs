using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class LoadUserAgreementCommand : BaseTransactionModel, ICommand<UserAgreement>
{
    public new string TransactionCode { get; set; }
}

[CqrsHandler]
public class LoadUserAgreementHandle(IUserAgreementRepository userAgreementRepository)
    : ICommandHandler<LoadUserAgreementCommand, UserAgreement>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_LOAD_USERAGREEMENT)]
    public async Task<UserAgreement> HandleAsync(
        LoadUserAgreementCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var model = request.ToModel<LoadUserAgreementRequestModel>();
        return await LoadUserAgreementAsync(model);
    }

    public async Task<UserAgreement> LoadUserAgreementAsync(LoadUserAgreementRequestModel model)
    {
        return await userAgreementRepository
            .Table.Where(s => s.IsActive && s.TransactionCode == model.TransactionCode)
            .FirstOrDefaultAsync();
    }
}
