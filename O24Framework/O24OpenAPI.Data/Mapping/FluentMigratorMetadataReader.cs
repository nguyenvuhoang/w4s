using System.Collections.Concurrent;
using System.Reflection;
using LinqToDB.Mapping;
using LinqToDB.Metadata;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Data.Mapping;

/// <summary>
/// The fluent migrator metadata reader class
/// </summary>
/// <seealso cref="IMetadataReader"/>
public class FluentMigratorMetadataReader(IMappingEntityAccessor mappingEntityAccessor)
    : IMetadataReader
{
    /// <summary>
    /// The mapping entity accessor
    /// </summary>
    private readonly IMappingEntityAccessor _mappingEntityAccessor = mappingEntityAccessor;

    /// <summary>
    /// Gets the value of the types
    /// </summary>
    private static ConcurrentDictionary<(Type, MemberInfo), Attribute> Types { get; } =
        new ConcurrentDictionary<(Type, MemberInfo), Attribute>();

    /// <summary>
    /// Gets the attribute using the specified type
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="type">The type</param>
    /// <param name="memberInfo">The member info</param>
    /// <returns>The</returns>
    private T GetAttribute<T>(Type type, MemberInfo memberInfo)
        where T : Attribute
    {
        return (T)
            Types.GetOrAdd(
                (type, memberInfo),
                _ =>
                {
                    O24OpenAPIEntityDescriptor entityDescriptor =
                        _mappingEntityAccessor.GetEntityDescriptor(type);
                    if (typeof(T) == typeof(TableAttribute))
                    {
                        return new TableAttribute(entityDescriptor.EntityName)
                        {
                            Schema = entityDescriptor.SchemaName,
                        };
                    }

                    if (typeof(T) != typeof(ColumnAttribute))
                    {
                        return null;
                    }

                    O24OpenAPIEntityFieldDescriptor entityFieldDescriptor =
                        entityDescriptor.Fields.SingleOrDefault(cd =>
                            cd.Name.Equals(
                                NameCompatibilityManager.GetColumnName(type, memberInfo.Name),
                                StringComparison.OrdinalIgnoreCase
                            )
                        );
                    if (entityFieldDescriptor == null)
                    {
                        return null;
                    }

                    Type type1 = (memberInfo as PropertyInfo)?.PropertyType;
                    type1 ??= typeof(string);
                    Type type2 = type1;
                    MappingSchema mappingSchema = _mappingEntityAccessor.GetMappingSchema();
                    ColumnAttribute attribute = new()
                    {
                        Name = entityFieldDescriptor.Name,
                        IsPrimaryKey = entityFieldDescriptor.IsPrimaryKey,
                        IsColumn = true,
                        CanBeNull = entityFieldDescriptor.IsNullable.GetValueOrDefault(),
                    };
                    int? nullable = entityFieldDescriptor.Size;
                    attribute.Length = nullable.GetValueOrDefault();
                    nullable = entityFieldDescriptor.Precision;
                    attribute.Precision = nullable.GetValueOrDefault();
                    attribute.IsIdentity = entityFieldDescriptor.IsIdentity;
                    attribute.DataType = mappingSchema.GetDataType(type2).Type.DataType;
                    return attribute;
                }
            );
    }

    /// <summary>
    /// Gets the attributes using the specified type
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="type">The type</param>
    /// <param name="attributeType">The attribute type</param>
    /// <param name="memberInfo">The member info</param>
    /// <returns>The array</returns>
    private T[] GetAttributes<T>(Type type, Type attributeType, MemberInfo memberInfo = null)
        where T : Attribute
    {
        T attribute = null;
        int num;
        if (type.IsSubclassOf(typeof(BaseEntity)) && typeof(T) == attributeType)
        {
            attribute = GetAttribute<T>(type, memberInfo);
            num = attribute != null ? 1 : 0;
        }
        else
        {
            num = 0;
        }

        if (num == 0)
        {
            return [];
        }

        return [attribute];
    }

    /// <summary>
    /// Gets the attributes using the specified type
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="type">The type</param>
    /// <param name="inherit">The inherit</param>
    /// <returns>The array</returns>
    public virtual T[] GetAttributes<T>(Type type, bool inherit = true)
        where T : Attribute
    {
        return GetAttributes<T>(type, typeof(TableAttribute), null);
    }

    /// <summary>
    /// Gets the attributes using the specified type
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="type">The type</param>
    /// <param name="memberInfo">The member info</param>
    /// <param name="inherit">The inherit</param>
    /// <returns>The array</returns>
    public virtual T[] GetAttributes<T>(Type type, MemberInfo memberInfo, bool inherit = true)
        where T : Attribute
    {
        return GetAttributes<T>(type, typeof(ColumnAttribute), memberInfo);
    }

    /// <summary>
    /// Gets the dynamic columns using the specified type
    /// </summary>
    /// <param name="type">The type</param>
    /// <returns>The member info array</returns>
    public MemberInfo[] GetDynamicColumns(Type type) => [];
}
