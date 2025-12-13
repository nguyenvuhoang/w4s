namespace O24OpenAPI.Sample.Services.Interfaces;

public interface ILogService<T>
    where T : class
{
    Task AddAsync(T log);
}
