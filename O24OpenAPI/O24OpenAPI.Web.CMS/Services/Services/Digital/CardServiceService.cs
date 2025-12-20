using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class CardServiceService : ICardServiceService
{
    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_CARD_SERVICE> _cardServiceRepository;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="cardServiceRepository"></param>
    public CardServiceService(
        ILocalizationService localizationService,
        IRepository<D_CARD_SERVICE> cardServiceRepository
    )
    {
        _localizationService = localizationService;
        _cardServiceRepository = cardServiceRepository;
    }

    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<D_CARD_SERVICE> GetById(int id)
    {
        return await _cardServiceRepository.Table.Where(s => s.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get By ProductCode
    /// </summary>
    /// <param name="productCode"></param>
    /// <returns></returns>
    public virtual async Task<CardServiceViewResponseModel> GetByCardServiceCode(string code)
    {
        var entity = await _cardServiceRepository
            .Table.Where(s => s.CardServiceCode == code)
            .FirstOrDefaultAsync();
        if (entity == null)
        {
            throw new O24OpenAPIException(
                "InvalidCardServiceCode",
                "Card service code does not exist"
            );
        }

        var response = new CardServiceViewResponseModel()
        {
            Id = entity.Id,
            CardServiceCode = entity.CardServiceCode,
            CardServiceName = entity.CardServiceName,
            Status = entity.Status,
        };
        return response;
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<CardServiceViewResponseModel>> GetAll()
    {
        var query = await (
            from c in _cardServiceRepository.Table.DefaultIfEmpty()
            select new CardServiceViewResponseModel()
            {
                Id = c.Id,
                CardServiceCode = c.CardServiceCode,
                CardServiceName = c.CardServiceName,
                Status = c.Status,
            }
        ).ToListAsync();
        return query;
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<CardServiceViewResponseModel> ViewById(int id)
    {
        var entity = await _cardServiceRepository.GetById(id);
        if (entity == null)
        {
            throw new O24OpenAPIException(
                "InvalidCardServiceCode",
                "Card service code does not exist"
            );
        }

        var response = new CardServiceViewResponseModel()
        {
            Id = entity.Id,
            CardServiceCode = entity.CardServiceCode,
            CardServiceName = entity.CardServiceName,
            Status = entity.Status,
        };
        return response;
    }

    /// <summary>
    /// Insert
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public virtual async Task<D_CARD_SERVICE> Insert(D_CARD_SERVICE card)
    {
        await _cardServiceRepository.Insert(card);
        return card;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public virtual async Task<D_CARD_SERVICE> Update(D_CARD_SERVICE card)
    {
        await _cardServiceRepository.Update(card);
        return card;
    }

    /// <summary>
    /// Delete By Id
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public virtual async Task<D_CARD_SERVICE> DeleteById(int cardId)
    {
        var entity = await _cardServiceRepository.GetById(cardId);
        if (entity == null)
        {
            throw new O24OpenAPIException(
                "InvalidCardServiceCode",
                "Card service code does not exist"
            );
        }

        await _cardServiceRepository.Delete(entity);
        return entity;
    }

    /// <summary>
    /// Simple search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual async Task<IPagedList<CardServiceSearchResponseModel>> SimpleSearch(
        SimpleSearchModel model
    )
    {
        model.SearchText = !string.IsNullOrEmpty(model.SearchText)
            ? model.SearchText
            : string.Empty;
        var query = await (
            from p in _cardServiceRepository.Table.DefaultIfEmpty()
            where
                p.CardServiceCode.Contains(model.SearchText)
                || p.CardServiceName.Contains(model.SearchText)
                || p.Status.Contains(model.SearchText)
            select new CardServiceSearchResponseModel()
            {
                Id = p.Id,
                CardServiceCode = p.CardServiceCode,
                CardServiceName = p.CardServiceName,
                Status = p.Status,
            }
        ).OrderBy(c => c.CardServiceCode).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<CardServiceSearchResponseModel>> AdvancedSearch(
        CardServiceAdvancedSearchRequestModel model
    )
    {
        var query = await (
            from c in _cardServiceRepository.Table.DefaultIfEmpty()
            where
                (
                    !string.IsNullOrEmpty(c.CardServiceCode)
                    && !string.IsNullOrEmpty(model.CardServiceCode)
                        ? c.CardServiceCode.Contains(model.CardServiceCode)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.CardServiceName)
                    && !string.IsNullOrEmpty(model.CardServiceName)
                        ? c.CardServiceName.Contains(model.CardServiceName)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.Status) && !string.IsNullOrEmpty(model.Status)
                        ? c.Status.Contains(model.Status)
                        : true
                )

            select new CardServiceSearchResponseModel()
            {
                Id = c.Id,
                CardServiceCode = c.CardServiceCode,
                CardServiceName = c.CardServiceName,
                Status = c.Status,
            }
        ).OrderBy(c => c.CardServiceCode).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }
}
