namespace O24OpenAPI.O24Design.Services.Interfaces;

public interface ILogService<T>
    where T : class
{
    Task AddAsync(T log);
}
