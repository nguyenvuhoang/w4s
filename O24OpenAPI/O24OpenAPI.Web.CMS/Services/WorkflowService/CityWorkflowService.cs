using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class CityWorkflowService : BaseQueueService
{
    private readonly ICityService _cityRepository = EngineContext.Current.Resolve<ICityService>();

    public async Task<WorkflowScheme> GetAll(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var list = await _cityRepository.GetAll();
                return list;
            }
        );
    }

    public async Task<WorkflowScheme> GetCityByCityNameModel(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetCityByCityNameModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var city = await _cityRepository.GetByCityName(model.CityName);
                return city;
            }
        );
    }

    public async Task<WorkflowScheme> Insert(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<CityModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var newCity = new D_CITY();
                newCity = model.ToEntity(newCity);
                var city = await _cityRepository.Insert(newCity);
                return city;
            }
        );
    }

    public async Task<WorkflowScheme> Update(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<CityUpdateModel>();

        if (string.IsNullOrEmpty(model.CityName))
        {
            throw new O24OpenAPIException("InvalidCityName", "The City Name is required");
        }

        if (string.IsNullOrEmpty(model.CityCode))
        {
            throw new O24OpenAPIException("InvalidCityCode", "The City Code is required");
        }

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var city = await _cityRepository.GetByCityID(model.CityID);

                if (city == null)
                {
                    throw new O24OpenAPIException(
                        "InvalidCity",
                        "The city does not exist in system"
                    );
                }

                var updateCity = model.ToEntity(city);
                await _cityRepository.Update(updateCity);
                return updateCity;
            }
        );
    }

    public async Task<WorkflowScheme> DeleteById(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetCityByCityCodeModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var city = await _cityRepository.DeleteById(model.CityID);
                return city;
            }
        );
    }

    public async Task<WorkflowScheme> Search(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<SearchCityModel>();

        return await Invoke<SearchCityModel>(
            workflow,
            async () =>
            {
                var list = await _cityRepository.Search(model);
                return list;
            }
        );
    }
}
