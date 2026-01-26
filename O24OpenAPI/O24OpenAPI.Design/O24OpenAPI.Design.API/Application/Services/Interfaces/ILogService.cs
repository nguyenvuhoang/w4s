namespace O24OpenAPI.Design.API.Application.Services.Interfaces;

public interface ILogService<T>
    where T : class
{
    Task AddAsync(T log);
}
