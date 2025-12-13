using Jits.Neptune.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Models.Portal;
using O24OpenAPI.Web.CMS.Models.Request;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper;

namespace O24OpenAPI.Web.CMS.Infrastructure.Mapper;

/// <summary>

/// The cms mapper configuration class

/// </summary>

/// <seealso cref="BaseMapperConfiguration"/>

public partial class CMSMapperConfiguration : BaseMapperConfiguration
{
    /// <summary>
    /// Framework mapper configuration constructor
    /// </summary>
    public CMSMapperConfiguration()
    {
        CreateMap<Bo, BoModel>()
            .ForMember(des => des.Input, src => src.Ignore())
            .ForMember(des => des.Actions, src => src.Ignore())
            .ForMember(des => des.Response, src => src.Ignore());
        CreateMap<Fo, FoModel>()
            .ForMember(des => des.Input, src => src.Ignore())
            .ForMember(des => des.Actions, src => src.Ignore())
            .ForMember(des => des.Request, src => src.Ignore());
        CreateMap<Fo, FoCreateModel>()
            .ForMember(des => des.Input, src => src.Ignore())
            .ForMember(des => des.Actions, src => src.Ignore())
            .ForMember(des => des.Request, src => src.Ignore());

        CreateMap<App, AppModel>().ForMember(des => des.ConfigTemplate, src => src.Ignore());

        CreateMap<LearnApi, LearnApiModel>();
        CreateMap<WorkflowRequestModel, BaseTransactionModel>();
        CreateMap<LearnApiRequestModel, BaseTransactionModel>();
        CreateModelMap<LogService, LogServiceModel>();
        CreateMap<D_CURRENCY, CurrencyModel>();
        CreateModelMap<D_COUNTRY, CountryInsertModel>();
        CreateModelMap<D_COUNTRY, CountryUpdateModel>();
        CreateModelMap<D_COUNTRY, CountrySearchAdvanceModel>();
        CreateModelMap<D_COUNTRY, CountryViewModel>();
        CreateModelMap<D_CURRENCY, CurrencyModel>();
        CreateModelMap<D_BRANCH, BranchModel>();
        CreateModelMap<D_PRODUCT, ProductModel>();
        CreateModelMap<D_BRANCH, BranchUpdateModel>();
        CreateModelMap<D_SAVING_PRODUCT, SavingProductViewResponseModel>();
        CreateModelMap<D_SAVING_PRODUCT, SavingProductAdvancedSearchRequestModel>();
        CreateModelMap<D_SAVING_PRODUCT, SavingProductInsertModel>();
        CreateModelMap<D_SAVING_PRODUCT, SavingProductUpdateModel>();
        CreateModelMap<D_CITY, GetCityByCityCodeModel>();
        CreateModelMap<D_CITY, GetCityByCityNameModel>();
        CreateModelMap<D_CITY, CityModel>();
        CreateModelMap<D_CITY, CityUpdateModel>();
        CreateModelMap<D_CITY, SearchCityModel>();

        CreateModelMap<D_CARD, CardAdvancedSearchRequestModel>();
        CreateModelMap<D_CARD, CardInsertModel>();
        CreateModelMap<D_CARD, CardUpdateModel>();

        CreateModelMap<D_CARD_USER, CardUserAdvancedSearchRequestModel>();
        CreateModelMap<D_CARD_USER, CardUserInsertModel>();
        CreateModelMap<D_CARD_USER, CardUserUpdateModel>();

        CreateModelMap<D_CARD_SERVICE, CardServiceAdvancedSearchRequestModel>();
        CreateModelMap<D_CARD_SERVICE, CardServiceInsertModel>();
        CreateModelMap<D_CARD_SERVICE, CardServiceUpdateModel>();
        CreateModelMap<D_FEE_TYPE, FeeTypeInsertModel>();
        CreateModelMap<D_FEE_TYPE, FeeTypeUpdateModel>();
        CreateModelMap<D_FEE_TYPE, FeeTypeSearchAdvanceModel>();
        CreateModelMap<D_FEE_TYPE, FeeTypeViewModel>();
        CreateModelMap<D_FEE, FeeInsertModel>();
        CreateModelMap<D_FEE, FeeUpdateModel>();
        CreateModelMap<D_FEE, FeeSearchAdvanceModel>();
        CreateModelMap<D_FEE, FeeViewModel>();
        CreateModelMap<C_REASONS_DEFINITION, ReasonsDefinitionInsertModel>();
        CreateModelMap<C_REASONS_DEFINITION, ReasonsDefinitionUpdateModel>();
        CreateModelMap<C_REASONS_DEFINITION, ReasonsDefinitionSearchAdvanceModel>();
        CreateModelMap<C_REASONS_DEFINITION, ReasonsDefinitionViewModel>();
        CreateModelMap<D_SERVICE, ServiceInsertModel>();
        CreateModelMap<D_SERVICE, ServiceUpdateModel>();
        CreateModelMap<D_SERVICE, ServiceSearchAdvanceModel>();
        CreateModelMap<D_SERVICE, ServiceViewModel>();
        CreateModelMap<D_REMITTANCE_PURPOSES, RemittancePurposesInsertModel>();
        CreateModelMap<D_REMITTANCE_PURPOSES, RemittancePurposesUpdateModel>();
        CreateModelMap<D_REMITTANCE_PURPOSES, RemittancePurposesSearchAdvanceModel>();
        CreateModelMap<D_REMITTANCE_PURPOSES, RemittancePurposesViewModel>();
        CreateModelMap<D_SECURITY_QUESTION, SecurityQuestionInsertModel>();
        CreateModelMap<D_SECURITY_QUESTION, SecurityQuestionUpdateModel>();
        CreateModelMap<D_SECURITY_QUESTION, SecurityQuestionSearchAdvanceModel>();
        CreateModelMap<D_SECURITY_QUESTION, SecurityQuestionViewModel>();

        CreateModelMap<C_CODELIST, CodeListAdvancedSearchRequestModel>();
        CreateModelMap<C_CODELIST, CodeListInsertModel>();
        CreateModelMap<C_CODELIST, CodeListUpdateModel>();

        CreateModelMap<D_REWARDS, RewardModel>();
        CreateModelMap<D_REWARDS, GetRewardByIDModel>();
        CreateModelMap<D_REWARDS, SearchRewardModel>();
        CreateModelMap<D_REQUESTREWARD, GetRequestRewardByIDModel>();
        CreateModelMap<D_REQUESTREWARD, RequestRewardModel>();
        CreateModelMap<D_USER_REWARD, SearchUserRewardModel>();
        CreateModelMap<D_USER_REWARD, GetUserRewardByIDModel>();
        CreateModelMap<D_USER_REWARD, UserRewardModel>();
        CreateModelMap<D_USER_REWARD, SearchUserRewardModel>();
        CreateModelMap<WorkflowDefinition, WorkflowDefinitionSearchResponseModel>();
        CreateModelMap<UserRight, UserRightModel>();
        CreateModelMap<CoreApiKeys, CoreAPIKeyModel>();
    }
}
