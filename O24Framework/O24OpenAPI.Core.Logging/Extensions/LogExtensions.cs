using O24OpenAPI.Core.Logging.Helpers;

namespace O24OpenAPI.Core.Logging.Extensions;

public static class LogExtensions
{
    public static void WriteError(this Exception ex)
    {
        BusinessLogHelper.Error(ex, ex.Message);
    }
}
