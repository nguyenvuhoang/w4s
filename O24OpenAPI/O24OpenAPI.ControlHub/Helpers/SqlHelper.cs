using LinqToDB;

namespace O24OpenAPI.ControlHub.Helpers;

public static class SqlHelper
{
    [Sql.Function("COALESCE", ServerSideOnly = true)]
    public static T Coalesce<T>(T expr1, T expr2) => throw new NotImplementedException();
}
