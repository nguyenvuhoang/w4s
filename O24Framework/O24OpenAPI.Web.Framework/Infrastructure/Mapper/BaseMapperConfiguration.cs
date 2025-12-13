using AutoMapper;
using O24OpenAPI.Core.Infrastructure.Mapper;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.Web.Framework.Infrastructure.Mapper;

/// <summary>
/// The base mapper configuration class
/// </summary>
/// <seealso cref="Profile"/>
/// <seealso cref="IOrderedMapperProfile"/>
public abstract class BaseMapperConfiguration : Profile, IOrderedMapperProfile
{
    /// <summary>
    ///
    /// </summary>
    /// /// <param name="ignoreRule"></param>
    /// <typeparam name="EntityType"></typeparam>
    /// <typeparam name="ModelType"></typeparam>
    /// <returns></returns>
    protected virtual void CreateModelMap<EntityType, ModelType>(
        Action<IMappingExpression<ModelType, EntityType>> ignoreRule = null
    )
        where ModelType : BaseO24OpenAPIModel
    {
        CreateMap<EntityType, ModelType>();
        IMappingExpression<ModelType, EntityType> map = CreateMap<ModelType, EntityType>();
        if (ignoreRule == null)
        {
            return;
        }

        ignoreRule(map);
    }

    /// <summary>
    ///
    /// </summary>
    public int Order => 0;
}
