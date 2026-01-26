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
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; }
    public int? Gender { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public new string Status { get; set; } = "A";

    public List<string> ChangedFields { get; set; } = [];

    public static UpdateUserRequestModel FromUpdatedEntity(
        UserAccount updated,
        UserAccount original
    )
    {
        var result = new UpdateUserRequestModel();
        var entityProps = typeof(UserAccount).GetProperties();
        var modelProps = typeof(UpdateUserRequestModel).GetProperties().ToDictionary(p => p.Name);

        foreach (var prop in entityProps)
        {
            if (!modelProps.ContainsKey(prop.Name))
            {
                continue;
            }

            var newValue = prop.GetValue(updated);
            var oldValue = prop.GetValue(original);

            if (
                (oldValue == null && newValue != null)
                || (oldValue != null && !oldValue.Equals(newValue))
            )
            {
                result.ChangedFields.Add(prop.Name);
            }

            modelProps[prop.Name].SetValue(result, newValue);
        }

        return result;
    }
}

[CqrsHandler]
public class UpdateUserHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<UpdateUserCommand, UpdateUserResponseModel>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_UPDATE_USER)]
    public async Task<UpdateUserResponseModel> HandleAsync(
        UpdateUserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var entity =
            await userAccountRepository.GetById(request.Id)
            ?? throw await O24Exception.CreateAsync(
                ResourceCode.Common.NotExists,
                request.Language
            );

        var originalEntity = entity.Clone();

        request.ToEntityNullable(entity);

        entity.UpdatedOnUtc = DateTime.UtcNow;

        await userAccountRepository.Update(entity);

        return UpdateUserResponseModel.FromUpdatedEntity(entity, originalEntity);
    }
}
