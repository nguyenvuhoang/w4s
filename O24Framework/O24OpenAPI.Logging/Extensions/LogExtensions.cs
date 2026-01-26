using O24OpenAPI.Logging.Helpers;

namespace O24OpenAPI.Logging.Extensions;

public static class LogExtensions
{
    public static void WriteError(this Exception ex)
    {
        BusinessLogHelper.Error(ex, ex.Message);
    }
}
