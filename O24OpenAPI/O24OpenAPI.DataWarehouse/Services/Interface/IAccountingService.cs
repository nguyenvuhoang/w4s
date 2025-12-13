namespace O24OpenAPI.DataWarehouse.Services.Interfaces;

public interface IAccountingService
{
    Task SyncGLEntries();
}
