using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Attributes;

namespace O24OpenAPI.CMS.API.Application.Features.Hello
{
    public class HelloCommand : BaseTransactionModel, ICommand<HelloResponse>
    {
        public string? Name { get; set; }
    }

    public class HelloResponse
    {
        public string Message { get; set; } = string.Empty;
    }

    [CqrsHandler]
    public class HelloHandler : ICommandHandler<HelloCommand, HelloResponse>
    {
        [WorkflowStep("CMS_HELLO")]
        public Task<HelloResponse> HandleAsync(HelloCommand request, CancellationToken cancellationToken = default)
        {
            throw new Exception("linh test exception");
            return Task.FromResult(new HelloResponse() { Message = $"Hello {request.Name}" });
        }
    }
}
