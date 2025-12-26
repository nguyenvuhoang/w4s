using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class UpdateUserCommand : BaseTransactionModel, ICommand<UpdateUserResponseModel>
{
    public UpdateUserRequestModel Model { get; set; } = default!;
}

[CqrsHandler]
public class UpdateUserHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<UpdateUserCommand, UpdateUserResponseModel>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_UPDATE_USER)]
    public async Task<UpdateUserResponseModel> HandleAsync(
        UpdateUserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        return await UpdateUserAsync(request.Model);
    }

    public async Task<UpdateUserResponseModel> UpdateUserAsync(UpdateUserRequestModel model)
    {
        var entity =
            await userAccountRepository.GetById(model.Id)
            ?? throw await O24Exception.CreateAsync(
                ResourceCode.Common.NotExists,
                model.Language
            );

        var originalEntity = entity.Clone();

        model.ToEntityNullable(entity);

        entity.UpdatedOnUtc = DateTime.UtcNow;

        await userAccountRepository.Update(entity);

        return UpdateUserResponseModel.FromUpdatedEntity(entity, originalEntity);
    }
}
