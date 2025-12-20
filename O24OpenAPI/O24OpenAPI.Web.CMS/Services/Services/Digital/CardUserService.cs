using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class CardUserService : ICardUserService
{
    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_CARD_USER> _cardUserRepository;
    private readonly IRepository<D_CARD> _cardRepository;
    private readonly ICodeListService _codeListService;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="cardUserRepository"></param>
    /// <param name="cardRepository"></param>
    /// <param name="codeListService"></param>
    public CardUserService(
        ILocalizationService localizationService,
        IRepository<D_CARD_USER> cardUserRepository,
        IRepository<D_CARD> cardRepository,
        ICodeListService codeListService
    )
    {
        _localizationService = localizationService;
        _cardUserRepository = cardUserRepository;
        _cardRepository = cardRepository;
        _codeListService = codeListService;
    }

    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<D_CARD_USER> GetById(int id)
    {
        return await _cardUserRepository.Table.Where(s => s.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<bool> IsCheckUserCode(string code)
    {
        var query = await (
            from u in _cardUserRepository.Table
            where u.UserCode == code
            select u
        ).FirstOrDefaultAsync();
        return query != null;
    }

    /// <summary>
    /// Get By ProductCode
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public virtual async Task<List<CardUserViewResponseModel>> GetListCardByUserCode(
        string code,
        string lang = "en"
    )
    {
        if (await IsCheckUserCode(code))
        {
            var query = await (
                from u in _cardUserRepository.Table
                join c in _cardRepository.Table on u.CardCode equals c.CardCode
                where u.UserCode == code
                select new CardUserViewResponseModel()
                {
                    Id = u.Id,
                    UserCode = u.UserCode,
                    CardCode = u.CardCode,
                    CardNumber = u.CardNumber,
                    CardHolderName = u.CardHolderName,
                    CardLogo = c.CardLogo,
                    CardName = c.CardName,
                    CardType = c.CardType,
                    CardLimit = u.CardLimit,
                    AvailableLimit = u.AvailableLimit,
                    Balance = u.Balance,
                    Status = u.Status.GetCaption("CARDSTS", "CARD"),
                    IsPrimary = u.IsPrimary,
                    CardExpiryDate = u.CardExpiryDate,
                    LinkagedAccount = u.LinkagedAccount,
                }
            ).ToListAsync();
            return query;
        }
        return new List<CardUserViewResponseModel>();
    }

    /// <summary>
    /// Get By ProductCode
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public virtual async Task<CardUserViewResponseModel> GetCardByUserCode(string code)
    {
        if (await IsCheckUserCode(code))
        {
            var query = await (
                from u in _cardUserRepository.Table
                join c in _cardRepository.Table on u.CardCode equals c.CardCode
                where u.UserCode == code
                select new CardUserViewResponseModel()
                {
                    Id = u.Id,
                    UserCode = u.UserCode,
                    CardCode = u.CardCode,
                    CardNumber = u.CardNumber,
                    CardHolderName = u.CardHolderName,
                    CardLogo = c.CardLogo,
                    CardName = c.CardName,
                    CardType = c.CardType,
                    CardLimit = u.CardLimit,
                    AvailableLimit = u.AvailableLimit,
                    Balance = u.Balance,
                    Status = u.Status,
                    IsPrimary = u.IsPrimary,
                    CardExpiryDate = u.CardExpiryDate,
                    LinkagedAccount = u.LinkagedAccount,
                }
            ).FirstOrDefaultAsync();
            return query;
        }
        return new CardUserViewResponseModel();
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<CardUserViewResponseModel>> GetAll(string lang = "en")
    {
        var query = await (
            from u in _cardUserRepository.Table.DefaultIfEmpty()
            join c in _cardRepository.Table on u.CardCode equals c.CardCode
            select new CardUserViewResponseModel()
            {
                Id = u.Id,
                UserCode = u.UserCode,
                CardCode = u.CardCode,
                CardLogo = c.CardLogo,
                CardName = c.CardName,
                CardType = c.CardType,
                CardNumber = u.CardNumber,
                CardHolderName = u.CardHolderName,
                CardLimit = u.CardLimit,
                AvailableLimit = u.AvailableLimit,
                Balance = u.Balance,
                Status = u.Status.GetCaption("CARDSTS", "CARD"),
                IsPrimary = u.IsPrimary,
                CardExpiryDate = u.CardExpiryDate,
                LinkagedAccount = u.LinkagedAccount,
            }
        ).ToListAsync();
        return query;
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<CardUserViewResponseModel> ViewById(int id, string lang = "en")
    {
        var entity = await _cardUserRepository.GetById(id);
        if (entity == null)
        {
            throw new O24OpenAPIException("InvalidUserCode", "User code does not exist");
        }

        var card = await _cardRepository
            .Table.Where(s => s.CardCode.Equals(entity.CardCode))
            .FirstOrDefaultAsync();
        if (card == null)
        {
            throw new O24OpenAPIException("InvalidCardCode", "Card code does not exist");
        }

        var response = new CardUserViewResponseModel()
        {
            Id = entity.Id,
            UserCode = entity.UserCode,
            CardCode = entity.CardCode,
            CardNumber = entity.CardNumber,
            CardLogo = card.CardLogo,
            CardName = card.CardName,
            CardHolderName = entity.CardHolderName,
            CardLimit = entity.CardLimit,
            AvailableLimit = entity.AvailableLimit,
            Balance = entity.Balance,
            CardType = card.CardType,
            Status = entity.Status.GetCaption("CARDSTS", "CARD"),
            IsPrimary = entity.IsPrimary,
            CardExpiryDate = entity.CardExpiryDate,
            LinkagedAccount = entity.LinkagedAccount,
        };
        return response;
    }

    /// <summary>
    /// Insert
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public virtual async Task<D_CARD_USER> Insert(D_CARD_USER cardUser)
    {
        await _cardUserRepository.Insert(cardUser);
        return cardUser;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public virtual async Task<D_CARD_USER> Update(D_CARD_USER cardUser)
    {
        await _cardUserRepository.Update(cardUser);
        return cardUser;
    }

    /// <summary>
    /// Delete By Id
    /// </summary>
    /// <param name="cardId"></param>
    /// <returns></returns>
    public virtual async Task<D_CARD_USER> DeleteById(int cardId)
    {
        var entity = await _cardUserRepository.GetById(cardId);
        if (entity == null)
        {
            throw new O24OpenAPIException("InvalidUserCode", "User code does not exist");
        }

        await _cardUserRepository.Delete(entity);
        return entity;
    }

    /// <summary>
    /// Simple search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual async Task<IPagedList<CardUserSearchResponseModel>> SimpleSearch(
        SimpleSearchModel model
    )
    {
        model.SearchText = !string.IsNullOrEmpty(model.SearchText)
            ? model.SearchText
            : string.Empty;
        var query = await (
            from p in _cardUserRepository.Table.DefaultIfEmpty()
            where
                p.UserCode.Contains(model.SearchText)
                || p.CardCode.Contains(model.SearchText)
                || p.CardNumber.Contains(model.SearchText)
                || p.CardHolderName.Contains(model.SearchText)
                || p.Status.ToString().Contains(model.SearchText)
                || p.CardExpiryDate.ToString().Contains(model.SearchText)
                || p.LinkagedAccount.Contains(model.SearchText)
            select new CardUserSearchResponseModel()
            {
                Id = p.Id,
                UserCode = p.UserCode,
                CardCode = p.CardCode,
                CardNumber = p.CardNumber,
                CardHolderName = p.CardHolderName,
                CardLimit = p.CardLimit,
                AvailableLimit = p.AvailableLimit,
                Balance = p.Balance,
                Status = p.Status,
                IsPrimary = p.IsPrimary,
                CardExpiryDate = p.CardExpiryDate,
                LinkagedAccount = p.LinkagedAccount,
            }
        ).OrderBy(c => c.UserCode).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<CardUserSearchResponseModel>> AdvancedSearch(
        CardUserAdvancedSearchRequestModel model
    )
    {
        var query = await (
            from c in _cardUserRepository.Table.DefaultIfEmpty()
            where
                (
                    !string.IsNullOrEmpty(c.UserCode) && !string.IsNullOrEmpty(model.UserCode)
                        ? c.UserCode.Contains(model.UserCode)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.CardCode) && !string.IsNullOrEmpty(model.CardCode)
                        ? c.CardCode.Contains(model.CardCode)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.CardNumber) && !string.IsNullOrEmpty(model.CardNumber)
                        ? c.CardNumber.Contains(model.CardNumber)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.CardHolderName)
                    && !string.IsNullOrEmpty(model.CardHolderName)
                        ? c.CardHolderName.Contains(model.CardHolderName)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.Status) && !string.IsNullOrEmpty(model.Status)
                        ? c.Status.Contains(model.Status)
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.CardExpiryDate.ToString())
                    && !string.IsNullOrEmpty(model.CardExpiryDate)
                        ? c.CardExpiryDate.Date >= DateTime.Parse(model.CardExpiryDate).Date
                        : true
                )
                && (
                    !string.IsNullOrEmpty(c.LinkagedAccount)
                    && !string.IsNullOrEmpty(model.LinkagedAccount)
                        ? c.LinkagedAccount.Contains(model.LinkagedAccount)
                        : true
                )

            select new CardUserSearchResponseModel()
            {
                Id = c.Id,
                UserCode = c.UserCode,
                CardCode = c.CardCode,
                CardNumber = c.CardNumber,
                CardHolderName = c.CardHolderName,
                CardLimit = c.CardLimit,
                AvailableLimit = c.AvailableLimit,
                Balance = c.Balance,
                Status = c.Status,
                IsPrimary = c.IsPrimary,
                CardExpiryDate = c.CardExpiryDate,
                LinkagedAccount = c.LinkagedAccount,
            }
        ).OrderBy(c => c.UserCode).AsQueryable().ToPagedList(model.PageIndex, model.PageSize);

        return query;
    }
}
