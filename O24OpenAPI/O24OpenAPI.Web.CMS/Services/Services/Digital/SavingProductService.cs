using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
/// Ctor
/// </summary>
/// <param name="loanProductRepository"></param>
public class SavingProductService(IRepository<D_SAVING_PRODUCT> loanProductRepository)
    : ISavingProductService
{
    private readonly IRepository<D_SAVING_PRODUCT> _savingProductRepository =
        loanProductRepository;

    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<D_SAVING_PRODUCT> GetById(int id)
    {
        return await _savingProductRepository
            .Table.Where(s => s.Id == id)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get By ProductCode
    /// </summary>
    /// <param name="productCode"></param>
    /// <returns></returns>
    public virtual async Task<D_SAVING_PRODUCT> GetByProductCode(string productCode)
    {
        return await _savingProductRepository
            .Table.Where(s => s.ProductCode == productCode)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_SAVING_PRODUCT>> GetAll()
    {
        return await _savingProductRepository.Table.Select(s => s).ToListAsync();
    }

    public virtual async Task<SavingProductViewResponseModel> ViewById(int id, string lang)
    {
        try
        {
            var entity = await _savingProductRepository.GetById(id);
            if (entity == null)
            {
                throw new O24OpenAPIException("InvalidProduct", "Product code does not exist");
            }

            var result = new SavingProductViewResponseModel();
            result.ProductCode = entity.ProductCode;
            result.ProductName = entity.ProductName.GetLangValue(lang);
            result.ProductImage = entity.ProductImage;
            result.Description = entity.Description.GetLangValue(lang);
            result.UserCode = entity.UserCode;
            result.IsAllowRegister = entity.IsAllowRegister;
            result.ProductUrl = entity.ProductUrl;
            return result;
        }
        catch (Exception ex)
        {
            throw new O24OpenAPIException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Insert
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public virtual async Task<D_SAVING_PRODUCT> Insert(D_SAVING_PRODUCT product)
    {
        await _savingProductRepository.Insert(product);
        return product;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public virtual async Task<D_SAVING_PRODUCT> Update(D_SAVING_PRODUCT product)
    {
        await _savingProductRepository.Update(product);
        return product;
    }

    /// <summary>
    /// Delete By Id
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public virtual async Task<D_SAVING_PRODUCT> DeleteById(int productId)
    {
        var data = await _savingProductRepository.GetById(productId);
        await _savingProductRepository.Delete(data);
        return data;
    }

    /// <summary>
    /// Simple search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual async Task<IPagedList<SavingProductSimpleSearchResponseModel>> SimpleSearch(
        SimpleSearchModel model
    )
    {
        model.SearchText = !string.IsNullOrEmpty(model.SearchText)
            ? model.SearchText
            : string.Empty;
        var query = await (
            from p in _savingProductRepository.Table.DefaultIfEmpty()
            where
                p.ProductCode.Contains(model.SearchText)
                || p.ProductName.Contains(model.SearchText)
                || p.Description.Contains(model.SearchText)
                || p.UserCode.Contains(model.SearchText)
                || p.IsAllowRegister.ToString().Contains(model.SearchText)
            select new SavingProductSimpleSearchResponseModel()
            {
                Id = p.Id,
                ProductCode = p.ProductCode,
                ProductName = p.ProductName,
                Description = p.Description,
                UserCode = p.UserCode,
                IsAllowRegister = p.IsAllowRegister,
                ProductUrl = p.ProductUrl,
            }
        ).OrderBy(c => c.ProductCode).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<
        IPagedList<SavingProductAdvancedSearchResponseModel>
    > AdvancedSearch(SavingProductAdvancedSearchRequestModel model)
    {
        var query = await (
            from c in _savingProductRepository.Table.DefaultIfEmpty()
            where
                (
                    !string.IsNullOrEmpty(c.ProductCode)
                    && !string.IsNullOrEmpty(model.ProductCode)
                        ? c.ProductCode.Contains(model.ProductCode)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.ProductName)
                    && !string.IsNullOrEmpty(model.ProductName)
                        ? c.ProductName.Contains(model.ProductName)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.Description)
                    && !string.IsNullOrEmpty(model.Description)
                        ? c.Description.Contains(model.Description)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.UserCode) && !string.IsNullOrEmpty(model.UserCode)
                        ? c.UserCode.Contains(model.UserCode)
                        : true
                )
            select new SavingProductAdvancedSearchResponseModel()
            {
                Id = c.Id,
                ProductCode = c.ProductCode,
                ProductName = c.ProductName,
                Description = c.Description,
                UserCode = c.UserCode,
                IsAllowRegister = c.IsAllowRegister,
                ProductUrl = c.ProductUrl,
            }
        ).OrderBy(c => c.ProductCode).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }
}
