namespace O24OpenAPI.AI.API.Application.Abstractions
{
    public interface ILlmChatProvider
    {
        Task<string> AskAsync(string system, string user, CancellationToken ct = default);
    }
}
