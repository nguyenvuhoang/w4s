using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace O24OpenAPI.CTH.API.Application.Features.UserCommands;

public class CreateMenuCommand
    : BaseTransactionModel,
        ICommand<UserCommandResponseModel>
{
    public string ApplicationCode { get; set; }
    public string CommandId { get; set; }
    public string ParentId { get; set; }
    public string CommandName { get; set; }
    public string CommandNameLanguage { get; set; }
    public string CommandType { get; set; }
    public string CommandURI { get; set; }
    public bool Enabled { get; set; }
    public bool IsVisible { get; set; }
    public int DisplayOrder { get; set; }
    public string GroupMenuIcon { get; set; }
    public string GroupMenuVisible { get; set; } = "1";
    public string GroupMenuListAuthorizeForm { get; set; } = string.Empty;
}

[CqrsHandler]
public class CreateMenuCommandHandler(IUserCommandRepository userCommandRepository)
    : ICommandHandler<CreateMenuCommand, UserCommandResponseModel>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_CREATE_MENU)]
    public async Task<UserCommandResponseModel> HandleAsync(
        CreateMenuCommand request,
        CancellationToken cancellationToken = default
    )
    {

        var userCommandExists = await userCommandRepository.GetByCommandIdAsync(request.CommandId, request.ApplicationCode);
        if (userCommandExists != null)
        {
            throw new O24OpenAPIException($"CommandId {request.CommandId} already exists.");
        }

        Dictionary<string, string> langMap;
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        try
        {

            langMap = JsonSerializer.Deserialize<Dictionary<string, string>>(
                request.CommandNameLanguage,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? [];
        }
        catch (JsonException ex)
        {
            throw new ArgumentException("command_name_language must be a JSON object like {\"en\":\"...\",\"vi\":\"...\"}", ex);
        }

        var command = new UserCommand
        {
            CommandId = request.CommandId,
            ApplicationCode = request.ApplicationCode,
            ParentId = request.ParentId,
            CommandName = request.CommandName,
            CommandNameLanguage = JsonSerializer.Serialize(langMap, options),
            CommandType = request.CommandType,
            CommandURI = request.CommandURI,
            Enabled = request.Enabled,
            IsVisible = request.IsVisible,
            DisplayOrder = request.DisplayOrder,
            GroupMenuIcon = request.GroupMenuIcon,
            GroupMenuVisible = request.GroupMenuVisible,
            GroupMenuListAuthorizeForm = request.GroupMenuListAuthorizeForm
        };
        var userCommand = await userCommandRepository.AddAsync(command);
        return userCommand.ToUserCommandResponseModel();
    }
}
