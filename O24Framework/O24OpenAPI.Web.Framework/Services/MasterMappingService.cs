using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.Framework.Domain;
using O24OpenAPI.Web.Framework.Services.Interfaces;

namespace O24OpenAPI.Web.Framework.Services;

public class MasterMappingService(IRepository<MasterMapping> mappingRepository) : IMasterMappingService
{
    private readonly IRepository<MasterMapping> _mappingRepository = mappingRepository;
    public async Task<MasterMapping> GetByMasterClass(string masterClass)
    {
        if (string.IsNullOrWhiteSpace(masterClass))
        {
            throw new ArgumentException("masterClass is required.", nameof(masterClass));
        }

        var normalized = masterClass.Trim().ToLower();

        return await _mappingRepository.Table
            .FirstOrDefaultAsync(a => a.MasterClass.ToLower().Trim() == normalized);
    }
}
