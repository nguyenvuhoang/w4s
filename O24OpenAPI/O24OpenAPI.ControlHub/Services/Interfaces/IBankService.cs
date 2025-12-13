namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface IBankService
{
    DateTime GetWorkingDate(bool inBatch = false);
    Task UpdateWorkingDateTripletAsync(DateTime newWorkingDate);
}
