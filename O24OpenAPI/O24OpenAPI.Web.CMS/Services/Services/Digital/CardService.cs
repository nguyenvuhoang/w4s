using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Localization;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
/// Ctor
/// </summary>
/// <param name="localizationService"></param>
/// <param name="cardRepository"></param>
/// <param name="cardUserRepository"></param>
public class CardService(
    ILocalizationService localizationService,
    IRepository<D_CARD> cardRepository,
    IRepository<D_CARD_USER> cardUserRepository
) : ICardService
{
    private readonly ILocalizationService _localizationService = localizationService;

    private readonly IRepository<D_CARD> _cardRepository = cardRepository;

    private readonly IRepository<D_CARD_USER> _cardUserRepository = cardUserRepository;

    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<List<GetCardInformationModel>> GetCardInformation()
    {
        var query = await (
            from c in _cardRepository.Table
            join u in _cardUserRepository.Table on c.CardCode equals u.CardCode
            select new GetCardInformationModel()
            {
                Id = u.Id,
                CardCode = c.CardCode,
                CardNumber = u.CardNumber,
                CardType = c.CardType,
                CardLogo = c.CardLogo,
                CardName = c.CardName,
                CardExpiryDate = u.CardExpiryDate,
            }
        ).ToListAsync();

        return query;
    }

    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<D_CARD> GetById(int id)
    {
        var entity = await _cardRepository.GetById(id);
        if (entity == null)
        {
            throw new O24OpenAPIException("InvalidCardCode", "Card code does not exist");
        }

        return entity;
    }

    /// <summary>
    /// Get By ProductCode
    /// </summary>
    /// <param name="productCode"></param>
    /// <returns></returns>
    public virtual async Task<CardViewResponseModel> GetByCardCode(string code)
    {
        var entity = await _cardRepository
            .Table.Where(s => s.CardCode == code)
            .FirstOrDefaultAsync();
        if (entity == null)
        {
            throw new O24OpenAPIException("InvalidCardCode", "Card code does not exist");
        }

        var response = new CardViewResponseModel()
        {
            Id = entity.Id,
            CardCode = entity.CardCode,
            CardLogo = entity.CardLogo,
            CardName = entity.CardName,
            CardType = entity.CardType,
            CardLimit = entity.CardLimit,
            CardServiceCode = entity.CardServiceCode,
        };
        return response;
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<CardViewResponseModel>> GetAll()
    {
        var query = await (
            from c in _cardRepository.Table.DefaultIfEmpty()
            select new CardViewResponseModel()
            {
                Id = c.Id,
                CardCode = c.CardCode,
                CardLogo = c.CardLogo,
                CardName = c.CardName,
                CardType = c.CardType,
                CardLimit = c.CardLimit,
                CardServiceCode = c.CardServiceCode,
            }
        ).ToListAsync();
        return query;
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<CardViewResponseModel> ViewById(int id)
    {
        var entity = await _cardRepository.GetById(id);
        if (entity == null)
        {
            throw new O24OpenAPIException("InvalidCardCode", "Card code does not exist");
        }

        var response = new CardViewResponseModel()
        {
            Id = entity.Id,
            CardCode = entity.CardCode,
            CardType = entity.CardType,
            CardName = entity.CardName,
            CardLogo = entity.CardLogo,
            CardLimit = entity.CardLimit,
            CardServiceCode = entity.CardServiceCode,
        };
        return response;
    }

    /// <summary>
    /// Insert
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public virtual async Task<D_CARD> Insert(D_CARD card)
    {
        await _cardRepository.Insert(card);
        return card;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public virtual async Task<D_CARD> Update(D_CARD card)
    {
        await _cardRepository.Update(card);
        return card;
    }

    /// <summary>
    /// Delete By Id
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public virtual async Task<D_CARD> DeleteById(int cardId)
    {
        var entity = await _cardRepository.GetById(cardId);
        if (entity == null)
        {
            throw new O24OpenAPIException("InvalidCardCode", "Card code does not exist");
        }

        await _cardRepository.Delete(entity);
        return entity;
    }

    /// <summary>
    /// Simple search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual async Task<IPagedList<CardSearchResponseModel>> SimpleSearch(
        SimpleSearchModel model
    )
    {
        model.SearchText = !string.IsNullOrEmpty(model.SearchText)
            ? model.SearchText
            : string.Empty;
        var query = await (
            from p in _cardRepository.Table.DefaultIfEmpty()
            where
                p.CardCode.Contains(model.SearchText)
                || p.CardName.Contains(model.SearchText)
                || p.CardType.Contains(model.SearchText)
                || p.CardServiceCode.ToString().Contains(model.SearchText)
            select new CardSearchResponseModel()
            {
                Id = p.Id,
                CardCode = p.CardCode,
                CardLogo = p.CardLogo,
                CardName = p.CardName,
                CardType = p.CardType,
                CardLimit = p.CardLimit,
                CardServiceCode = p.CardServiceCode,
            }
        )
            .OrderBy(c => c.CardCode)
            .AsQueryable()
            .ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<CardSearchResponseModel>> AdvancedSearch(
        CardAdvancedSearchRequestModel model
    )
    {
        var query = await (
            from c in _cardRepository.Table.DefaultIfEmpty()
            where
                (
                    !string.IsNullOrEmpty(c.CardCode) && !string.IsNullOrEmpty(model.CardCode)
                        ? c.CardCode.Contains(model.CardCode)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.CardName) && !string.IsNullOrEmpty(model.CardName)
                        ? c.CardName.Contains(model.CardName)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.CardType) && !string.IsNullOrEmpty(model.CardType)
                        ? c.CardType.Contains(model.CardType)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.CardServiceCode)
                    && !string.IsNullOrEmpty(model.CardServiceCode)
                        ? c.CardName.Contains(model.CardServiceCode)
                        : true
                )
            select new CardSearchResponseModel()
            {
                Id = c.Id,
                CardCode = c.CardCode,
                CardLogo = c.CardLogo,
                CardName = c.CardName,
                CardType = c.CardType,
                CardLimit = c.CardLimit,
                CardServiceCode = c.CardServiceCode,
            }
        )
            .OrderBy(c => c.CardCode)
            .AsQueryable()
            .ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }
}
