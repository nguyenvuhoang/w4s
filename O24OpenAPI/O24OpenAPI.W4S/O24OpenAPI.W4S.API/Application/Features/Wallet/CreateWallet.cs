using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.API.Application.Helpers;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;
using O24OpenAPI.W4S.Infrastructure.Configurations;

namespace O24OpenAPI.W4S.API.Application.Features.Wallet;

public class CreateWalletCommand : BaseTransactionModel, ICommand<CreateWalletResponseModel>
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string LoginName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public int Gender { get; set; }
    public string Birthday { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string UserCreated { get; set; }
    public string UserLevel { get; set; }
    public int PolicyId { get; set; }
    public List<string> UserGroup { get; set; }
    public string ContractNumber { get; set; }
    public string UserChannelId { get; set; }
    public string RoleChannel { get; set; }
    public string NotificationType { get; set; } = "MAIL";
    public string ContractType { get; set; }
    public string UserType { get; set; } = "0502";
}

[CqrsHandler]
public class CreateWalletHandle(IWalletProfileRepository walletProfileRepository, IWalletContractRepository walletContractRepository, W4SSetting w4SSetting)
    : ICommandHandler<CreateWalletCommand, CreateWalletResponseModel>
{
    [WorkflowStep(WorkflowStep.W4S.WF_STEP_W4S_CREATE_WALLET)]
    public async Task<CreateWalletResponseModel> HandleAsync(
        CreateWalletCommand request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await ValidateRequest(request, request.Language);
            var contractNumber = await ResolveContractNumberAsync(request, cancellationToken);
            var contract = WalletContract.Create(
                contractNumber: contractNumber,
                contractType: WalletContractTypeHelper.Parse(request.ContractType),
                walletTier: WalletTier.Basic,
                userType: WalletUserTypeHelper.Parse(request.UserType),
                userLevel: WalletUserLevelHelper.Parse(request.UserLevel),
                policyCode: $"POL{request.PolicyId}",
                customerCode: $"CUST{request.LoginName}",
                fullName: $"{request.FirstName} {request.LastName}".Trim(),
                phone: request.Phone,
                email: request.Email,
                channel: WalletChannel.MB
            );
            try
            {
                await walletContractRepository.InsertAsync(contract);
            }
            catch (Exception ex) when (DbExceptionHelper.IsUniqueConstraintViolation(ex))
            {
                contract.ContractNumber = await ResolveContractNumberAsync(request, cancellationToken);
                await walletContractRepository.InsertAsync(contract);
            }

            var walletid = Guid.NewGuid();
            var profile = WalletProfile.Create(
                walletId: walletid,
                contractNumber: contract.ContractNumber,
                userCode: request.LoginName,
                walletName: $"{request.FirstName} ${request.MiddleName} {request.LastName}".Trim(),
                walletType: Code.WalletType.TWDR,
                defaultCurrency: w4SSetting.BaseCurrency
            );

            await walletProfileRepository.InsertAsync(profile);

            return new CreateWalletResponseModel
            {
                WalletId = profile.WalletId,
                ContractNumber = contract.ContractNumber,
                UserCode = profile.UserCode
            };
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw await O24Exception.CreateAsync(
                ResourceCode.Common.SystemError,
                request.Language,
                [ex.Message]
            );
        }
    }



    /// <summary>
    /// Validate request
    /// </summary>
    /// <param name="r"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    private async static Task ValidateRequest(CreateWalletCommand r, string language)
    {
        if (string.IsNullOrWhiteSpace(r.LoginName))
            throw await O24Exception.CreateAsync(
                ResourceCode.Validation.RequiredField,
                language,
                "LoginName"
            );

        if (string.IsNullOrWhiteSpace(r.Phone))
            throw await O24Exception.CreateAsync(
                ResourceCode.Validation.RequiredField,
                language,
                "Phone"
            );

        if (string.IsNullOrWhiteSpace(r.FirstName) && string.IsNullOrWhiteSpace(r.LastName))
            throw await O24Exception.CreateAsync(
                ResourceCode.Validation.RequiredField,
                language,
                "LastName"
            );
    }

    /// <summary>
    /// Resolve contract number
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>

    private async Task<string> ResolveContractNumberAsync(CreateWalletCommand request, CancellationToken ct)
    {
        if (!string.IsNullOrWhiteSpace(request.ContractNumber))
        {
            var cn = request.ContractNumber.Trim();
            var exists = await walletContractRepository.ExistsByContractNumberAsync(cn);
            if (exists)
                return cn;
            return cn;
        }

        var contractNumber = WalletContractNumberGenerator.Generate(WalletContractKind.Personal, nodeId: 1);
        return contractNumber;
    }
}
