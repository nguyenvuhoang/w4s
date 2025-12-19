namespace O24OpenAPI.DataWarehouse.Services.Interface;

public interface IAccountingService
{
    Task SyncGLEntries();
}
