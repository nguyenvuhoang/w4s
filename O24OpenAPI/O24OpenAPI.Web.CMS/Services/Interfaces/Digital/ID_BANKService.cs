namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface ID_BANKService
{
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_BANK> GetById(int id);

    /// <summary>
    /// Gets the by bin using the specified bin
    /// </summary>
    /// <param name="bin">The bin</param>
    /// <returns>A task containing the bank model</returns>
    Task<D_BANKModel> GetByBin(string bin);

    /// <summary>
    /// Inserts the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_BANK> Insert(D_BANK learnApi);

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of d bank</returns>
    Task<List<D_BANK>> GetAll();

    /// <summary>
    /// Updates the learn api
    /// </summary>
    /// <param name="learnApi">The learn api</param>
    /// <returns>A task containing the bank</returns>
    Task<D_BANK> Update(D_BANK learnApi);

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the bank</returns>
    Task<D_BANK> DeleteById(int id);
}
