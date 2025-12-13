namespace O24OpenAPI.O24NCH.Services.Interfaces;

public interface ISMSLoanAlertService
{
    public Task SubmitSMSLoanAlert(CancellationToken ct = default);
}
