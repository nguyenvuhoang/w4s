using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Models;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public interface IBranchService
{
    Task<Branch> GetBranchByBranchCode(string branchCode);
    Task<ListBranchModel> GetBranchByBranchName(string branchName);
}
