using LinqToDB;
using O24OpenAPI.Data;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Services;

public class BranchService(IRepository<Branch> branchRepository) : IBranchService
{
    private readonly IRepository<Branch> _branchRepository = branchRepository;
    private readonly StringComparison ICIC = StringComparison.InvariantCultureIgnoreCase;

    public async Task<Branch> GetBranchByBranchCode(string branchCode)
    {
        var branch = await _branchRepository.Table.Where(c => c.BranchCode.Equals(branchCode)).FirstOrDefaultAsync();
        return branch;
    }

    public async Task<ListBranchModel> GetBranchByBranchName(string branchName)
    {
        var branches = await _branchRepository.Table
            .Where(b => b.BranchName.Contains(branchName))
            .ToListAsync();

        var branchList = new ListBranchModel
        {
            Branchs = branches.Select(b => new BranchModel
            {
                BranchCode = b.BranchCode,
                BranchName = b.BranchName,
                Id = b.Id,
            }).ToList()
        };

        return branchList;
    }

}
