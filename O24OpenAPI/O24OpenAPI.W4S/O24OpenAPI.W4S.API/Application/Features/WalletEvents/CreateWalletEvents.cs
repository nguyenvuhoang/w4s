using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletEvents;

public class CreateWalletEventsCommand
    : BaseTransactionModel,
        ICommand<CreateWalletEventsResponseModel>
{
    public string EventName { get; set; } = string.Empty;
    public string EventIcon { get; set; } = string.Empty;
    public string EventColor { get; set; } = string.Empty;
    public List<int> WalletId { get; set; } = [];
    public DateTime EndDate { get; set; }
}


[CqrsHandler]
public class CreateWalletEventsHandler(
    IWalletEventRepository walletEventRepository
) : ICommandHandler<CreateWalletEventsCommand, CreateWalletEventsResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_CREATE_WALLET_EVENT)]
    public async Task<CreateWalletEventsResponseModel> HandleAsync(
        CreateWalletEventsCommand request,
        CancellationToken ct = default
    )
    {
        if (request.WalletId == null || request.WalletId.Count == 0)
            throw new ArgumentException("WalletId is required", nameof(request.WalletId));

        if (string.IsNullOrWhiteSpace(request.EventName))
            throw new ArgumentException("EventName is required", nameof(request.EventName));

        var nowUtc = DateTime.UtcNow;

        foreach (var walletId in request.WalletId.Distinct())
        {
            if (walletId <= 0)
                continue;

            var evt = WalletEvent.Create(
                walletId: walletId,
                title: request.EventName,
                startOnUtc: nowUtc,
                endOnUtc: request.EndDate == default
                    ? null
                    : EnsureUtc(request.EndDate),
                icon: string.IsNullOrWhiteSpace(request.EventIcon)
                    ? null
                    : request.EventIcon,
                color: string.IsNullOrWhiteSpace(request.EventColor)
                    ? null
                    : request.EventColor,
                reminderMinutes: 1440,
                isRecurring: true,
                recurrenceRule: "FREQ=MONTHLY;INTERVAL=1"
            );

            await walletEventRepository.InsertAsync(evt);
        }

        return new CreateWalletEventsResponseModel();
    }

    private static DateTime EnsureUtc(DateTime dt)
    {
        if (dt.Kind == DateTimeKind.Utc)
            return dt;

        if (dt.Kind == DateTimeKind.Local)
            return dt.ToUniversalTime();

        return DateTime.SpecifyKind(dt, DateTimeKind.Local).ToUniversalTime();
    }
}
