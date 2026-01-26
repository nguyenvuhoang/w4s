using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.API.Application.Helpers;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;
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
    public List<WalletCounterpartyRequestModel> WithUsers { get; set; } = [];
    public string Location { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public DateTime? ReminderAt { get; set; }
    public List<string> Images { get; set; } = [];
    public DateTime RecordedAt { get; set; }
    public bool IsCalculateReport { get; set; } = false;
    public bool IsLoanForFund { get; set; } = false;
    public bool IsFunding { get; set; } = false;
    public decimal Fee { get; set; }
    public string UserCode { get; set; }
}

[CqrsHandler]
public class CreateWalletTransactionCommandHandler(
    IWalletTransactionRepository walletTransactionRepository,
    IWalletStatementRepository walletStatementRepository,
    IWalletCounterpartyRepository counterpartyRepository,
    IWalletBalanceRepository walletBalanceRepository
) : ICommandHandler<CreateWalletTransactionCommand, CreateWalletTransactionResponse>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_CREATE_WALLET_TRANSACTION)]
    public async Task<CreateWalletTransactionResponse> HandleAsync(
        CreateWalletTransactionCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ValidateRequest(request);

        //Declare
        var userCode = request.UserCode;
        var currencycode = request.Currency;
        var amount = request.Amount;
        var fee = request.Fee;
        var walletId = request.WalletId.ToString();
        var walletIdInt = request.WalletId;
        var accountNumber = request.AccountNumber;
        var transactiontype = request.Type;
        var transactiondate = request.TransactionDate;
        var recordedAt = request.RecordedAt;
        var currentUserCode = request.CurrentUserCode;
        var transactionDescription = request.TransactionDescription ?? string.Empty;
        var categoryId = request.CategoryId.ToString();
        var location = request.Location ?? string.Empty;
        var eventid = request.EventId ?? string.Empty;
        var image = request.Images.Count > 0 ? string.Join(",", request.Images) : null;
        var isCalculateReport = request.IsCalculateReport.ToString();
        var isLoanForFund = request.IsLoanForFund.ToString();
        var isFunding = request.IsFunding.ToString();
        var refid = string.IsNullOrWhiteSpace(request.RefId) ? Guid.NewGuid().ToString("N") : request.RefId;

        var now = DateTime.UtcNow;

        string transactionId = GenerateTransactionId();
        List<int> counterpartyIds = [];
        foreach (var input in request.WithUsers)
        {
            WalletCounterparty cp;

            if (input.Id.HasValue)
            {
                cp = await counterpartyRepository.GetById(input.Id.Value);
                if (cp == null || cp.UserCode != request.UserCode)
                    throw new O24OpenAPIException("Invalid counterparty");
            }
            else
            {
                cp = await counterpartyRepository.FindByPhoneOrEmailAsync(
                    request.UserCode,
                    input.Phone,
                    input.Email
                );

                if (cp == null)
                {
                    cp = WalletCounterparty.Create(
                        request.UserCode,
                        input.DisplayName,
                        input.Phone,
                        input.Email,
                        (WalletCounterpartyType)(input.CounterpartyType ?? 1),
                        input.AvatarUrl
                    );

                    await counterpartyRepository.InsertAsync(cp);
                }
            }

            cp.Touch();
            counterpartyIds.Add(cp.Id);
        }
        WalletTransaction entity = new()
        {
            TRANSACTIONID = transactionId,
            TRANSACTIONCODE = transactiontype,
            TRANSACTIONDATE = transactiondate,
            TRANSACTIONWORKDATE = recordedAt,
            CCYID = currencycode,
            SOURCEID = walletId,
            SOURCETRANREF = accountNumber,
            USERID = currentUserCode,
            TRANDESC = transactionDescription,

            STATUS = Code.Status.NORMAL,
            APPRSTS = 0,
            OFFLSTS = Code.Status.NORMAL,
            DELETED = false,
            ONLINE = true,
            DESTID = "WALLET",

            CHAR01 = walletId,
            CHAR02 = categoryId,
            CHAR03 = location,
            CHAR04 = eventid,
            CHAR05 = image,
            CHAR06 = isCalculateReport,
            CHAR07 = isLoanForFund,
            CHAR08 = isFunding,

            LISTUSERAPP = counterpartyIds.Count > 0 ? string.Join(",", counterpartyIds) : null,

            NUM01 = request.Amount,
            NUM02 = request.Fee,

            CreatedOnUtc = now,
            TRANSACTIONNAME = WalletAccountTypeHelper.GetDisplayName(transactiontype)
        };

        var direction = entity.TRANSACTIONCODE switch
        {
            "INCOME" => DrCr.D,
            "EXPENSE" => DrCr.C,
            "LOAN" => DrCr.D,      // nếu tracker có loan
            _ => DrCr.D
        };

        var lastClosing = await walletStatementRepository.GetLastClosingBalanceAsync(
            walletId: walletIdInt,
            accountNumber: accountNumber,
            currencyCode: currencycode,
            beforeUtc: now,
            cancellationToken
        );

        var opening = lastClosing ?? 0m;

        var statements = new List<WalletStatement>
        {
            WalletStatement.Create(
                walletId: walletIdInt,
                accountNumber: accountNumber,
                statementOnUtc: now,
                drCr: direction,
                amount: amount,
                currencyCode: currencycode,
                openingBalance: opening,
                description: transactionDescription ?? $"{entity.TRANSACTIONCODE} wallet {walletId}",
                referenceType: "WalletTransaction",
                referenceId: transactionId,
                externalRef: refid,
                source: walletId    ,
                transactionOnUtc: now
            )
        };

        if (direction == DrCr.D)
        {
            // EXPENSE / LOAN
            await walletBalanceRepository.DebitBalanceAsync(accountNumber, amount, currencycode);

            if (fee > 0)
                await walletBalanceRepository.DebitBalanceAsync(accountNumber, fee, currencycode);
        }
        else if (direction == DrCr.C)
        {
            var totalDebit = amount + fee;
            await walletBalanceRepository.CreditBalanceAsync(accountNumber, totalDebit, currencycode);
        }


        await walletStatementRepository.BulkInsert(statements);
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
