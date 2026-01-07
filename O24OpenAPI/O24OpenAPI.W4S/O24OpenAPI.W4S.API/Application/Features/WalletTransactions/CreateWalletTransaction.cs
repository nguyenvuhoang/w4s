using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using System.Security.Cryptography;

namespace O24OpenAPI.W4S.API.Application.Features.WalletTransactions;

public class CreateWalletTransactionCommand
    : BaseTransactionModel,
        ICommand<CreateWalletTransactionResponse>
{
    public int WalletId { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "VND";
    public string AccountNumber { get; set; } = default!;
    public int CategoryId { get; set; } = default!;
    public string TransactionDescription { get; set; } = string.Empty;
    public List<string> WithUsers { get; set; } = [];
    public string Location { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public DateTime? ReminderAt { get; set; }
    public List<string> Images { get; set; } = [];
    public DateTime RecordedAt { get; set; }
    public bool IsCalculateReport { get; set; } = false;
    public bool IsLoanForFund { get; set; } = false;
    public bool IsFunding { get; set; } = false;
    public decimal Fee { get; set; }
}

[CqrsHandler]
public class CreateWalletTransactionCommandHandler(
    IWalletTransactionRepository walletTransactionRepository
) : ICommandHandler<CreateWalletTransactionCommand, CreateWalletTransactionResponse>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_CREATE_WALLET_TRANSACTION)]
    public async Task<CreateWalletTransactionResponse> HandleAsync(
        CreateWalletTransactionCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ValidateRequest(request);

        string transactionId = GenerateTransactionId();

        WalletTransaction entity = new()
        {
            TRANSACTIONID = transactionId,
            TRANSACTIONCODE = request.Type,
            TRANSACTIONDATE = request.TransactionDate,
            TRANSACTIONWORKDATE = request.RecordedAt,
            CCYID = request.Currency,
            SOURCEID = request.AccountNumber,
            SOURCETRANREF = request.WalletId.ToString(),
            USERID = request.CurrentUserCode,
            TRANDESC = request.TransactionDescription ?? string.Empty,

            STATUS = Code.Status.NORMAL,
            APPRSTS = 0,
            OFFLSTS = Code.Status.NORMAL,
            DELETED = false,
            ONLINE = true,
            DESTID = "WALLET",

            CHAR01 = request.WalletId.ToString(),
            CHAR02 = request.CategoryId.ToString(),
            CHAR03 = request.Location,
            CHAR04 = request.EventId,
            CHAR05 = request.Images.Count > 0 ? string.Join(",", request.Images) : null,
            CHAR06 = request.IsCalculateReport.ToString(),
            CHAR07 = request.IsLoanForFund.ToString(),
            CHAR08 = request.IsFunding.ToString(),

            LISTUSERAPP = request.WithUsers.Count > 0 ? string.Join(",", request.WithUsers) : null,

            NUM01 = request.Amount,
            NUM02 = request.Fee,

            CreatedOnUtc = DateTime.UtcNow,
        };

        await walletTransactionRepository.InsertAsync(entity);

        return new CreateWalletTransactionResponse { TransactionId = entity.TRANSACTIONID };
    }

    /// <summary>
    /// Generate Transaction Id
    /// </summary>
    /// <returns></returns>
    private static string GenerateTransactionId()
    {
        string ts = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int rnd = RandomNumberGenerator.GetInt32(100, 999);
        return ts + rnd;
    }

    /// <summary>
    /// Validate Request
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="O24OpenAPIException"></exception>
    private static void ValidateRequest(CreateWalletTransactionCommand request)
    {
        if (request is null)
            throw new O24OpenAPIException("Request is null.");

        if (request.WalletId == 0)
            throw new O24OpenAPIException("WalletId is required.");

        if (string.IsNullOrWhiteSpace(request.Type))
            throw new O24OpenAPIException("Transaction Type is required.");

        if (request.Amount <= 0)
            throw new O24OpenAPIException("Amount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(request.AccountNumber))
            throw new O24OpenAPIException("AccountNumber is required.");

        if (request.CategoryId == 0)
            throw new O24OpenAPIException("CategoryId is required.");

        if (request.RecordedAt == default)
            throw new O24OpenAPIException("RecordedAt is required.");

        if (request.Fee < 0)
            throw new O24OpenAPIException("Fee must be greater than or equal to zero.");
    }
}
