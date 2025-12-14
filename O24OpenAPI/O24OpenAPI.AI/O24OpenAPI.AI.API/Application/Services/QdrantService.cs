using LinKit.Core.Abstractions;
using O24OpenAPI.AI.API.Application.Abstractions;

namespace O24OpenAPI.AI.API.Application.Services
{
    [RegisterService(Lifetime.Scoped)]
    public class QdrantService : IQdrantService
    {
    }
}
