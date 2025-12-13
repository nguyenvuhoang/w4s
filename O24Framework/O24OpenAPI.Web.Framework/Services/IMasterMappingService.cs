using O24OpenAPI.Web.Framework.Domain;

namespace O24OpenAPI.Web.Framework.Services.Interfaces;

public interface IMasterMappingService
{
    /// <summary>Gets a master mapping by master class name</summary>
    /// <param name="masterClass"></param>
    /// <returns></returns>
    Task<MasterMapping> GetByMasterClass(string masterClass);
}
