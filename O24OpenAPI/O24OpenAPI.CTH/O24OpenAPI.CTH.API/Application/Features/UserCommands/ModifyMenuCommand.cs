using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using System.Text.Json;

namespace O24OpenAPI.CTH.API.Application.Features.UserCommands;

public class ModifyMenuCommand
    : BaseTransactionModel,
        ICommand<UserCommandResponseModel>
{
    public string ApplicationCode { get; set; } = string.Empty;
    public string CommandId { get; set; } = string.Empty;
    public string ParentId { get; set; } = string.Empty;
    public string CommandName { get; set; } = string.Empty;
    public string CommandNameLanguage { get; set; } = string.Empty;
    public string CommandType { get; set; } = string.Empty;
    public string CommandURI { get; set; } = string.Empty;
    public bool Enabled { get; set; } = false;
    public bool IsVisible { get; set; } = false;
    public int DisplayOrder { get; set; } = 0;
    public string GroupMenuIcon { get; set; } = string.Empty;
    public string GroupMenuVisible { get; set; } = "1";
    public string GroupMenuListAuthorizeForm { get; set; } = string.Empty;
}

[CqrsHandler]
public class ModifyMenuCommandHandler(IUserCommandRepository userCommandRepository)
    : ICommandHandler<ModifyMenuCommand, UserCommandResponseModel>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_MODIFY_MENU)]
    public async Task<UserCommandResponseModel> HandleAsync(
        ModifyMenuCommand request,
        CancellationToken cancellationToken = default
    )
    {

        var userCommandExists = await userCommandRepository.GetByCommandIdAsync(request.CommandId, request.ApplicationCode) ??
            throw new O24OpenAPIException($"CommandId {request.CommandId} is does not exists.");

        userCommandExists.ParentId = request.ParentId;
        userCommandExists.CommandName = request.CommandName;
        userCommandExists.CommandNameLanguage = JsonSerializer.Serialize(request.CommandNameLanguage);
        userCommandExists.CommandType = request.CommandType;
        userCommandExists.Enabled = request.Enabled;
        userCommandExists.IsVisible = request.IsVisible;
        userCommandExists.CommandURI = request.CommandURI;
        userCommandExists.DisplayOrder = request.DisplayOrder;
        userCommandExists.GroupMenuIcon = request.GroupMenuIcon;
        userCommandExists.GroupMenuVisible = request.GroupMenuVisible;
        userCommandExists.GroupMenuListAuthorizeForm = request.GroupMenuListAuthorizeForm;

        var userCommand = await userCommandRepository.ModifyAsync(userCommandExists);
        return userCommand.ToUserCommandResponseModel();
    }
}
