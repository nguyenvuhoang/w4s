using LinqToDB;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class BranchService : IBranchService
{
    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_BRANCH> _branchRepository;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="branchRepository"></param>
    public BranchService(
        ILocalizationService localizationService,
        IRepository<D_BRANCH> branchRepository
    )
    {
        _localizationService = localizationService;
        _branchRepository = branchRepository;
    }

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <param name="BranchID">The id</param>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_BRANCH> GetByBranchCode(string BranchID)
    {
        return await _branchRepository
            .Table.Where(s => s.BranchID == BranchID)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_BRANCH>> GetAll()
    {
        return await _branchRepository.Table.Select(s => s).ToListAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_BRANCH>> Search(SearchBranchModel model)
    {
        var listBranch = await (
            from d in _branchRepository.Table
            where
                (
                    (!string.IsNullOrEmpty(model.BranchID) && model.BranchID == d.BranchID)
                    || string.IsNullOrEmpty(model.BranchID)
                )
                && (
                    (!string.IsNullOrEmpty(model.BranchName) && model.BranchName == d.BranchName)
                    || string.IsNullOrEmpty(model.BranchName)
                )
                && (
                    (!string.IsNullOrEmpty(model.Address) && model.Address == d.Address)
                    || string.IsNullOrEmpty(model.Address)
                )
                && (
                    (!string.IsNullOrEmpty(model.Phone) && model.Phone == d.Phone)
                    || string.IsNullOrEmpty(model.Phone)
                )
            select d
        ).ToListAsync();

        return listBranch;
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_BRANCH> Insert(D_BRANCH branch)
    {
        var findBranch = await _branchRepository
            .Table.Where(s => s.BranchID.Equals(branch.BranchID))
            .FirstOrDefaultAsync();
        if (findBranch == null)
        {
            await _branchRepository.Insert(branch);
        }
        else
        {
            throw new O24OpenAPIException(
                "InvalidBranch",
                "The branch code already existing in system"
            );
        }

        return branch;
    }

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_BRANCH> Update(D_BRANCH branch)
    {
        await _branchRepository.Update(branch);
        return branch;
    }

    /// <summary>
    ///Delete.
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_BRANCH> DeleteById(string BranchID)
    {
        var branch = await _branchRepository
            .Table.Where(s => s.BranchID.Equals(BranchID))
            .FirstOrDefaultAsync();

        if (branch == null)
        {
            throw new O24OpenAPIException(
                "InvalidBranch",
                "The branch code does not exist in system"
            );
        }
        await _branchRepository.Delete(branch);
        return branch;
    }

    public async Task DeleteByListID(DeleteBranchByBranchCodeModel model)
    {
        model.ListBranchID.Add(model.BranchID);
        foreach (string brID in model.ListBranchID)
        {
            var branch = await _branchRepository
                .Table.Where(s => s.BranchID.Equals(brID))
                .FirstOrDefaultAsync();

            if (branch != null)
            {
                await _branchRepository.Delete(branch);
            }
        }
    }
}
