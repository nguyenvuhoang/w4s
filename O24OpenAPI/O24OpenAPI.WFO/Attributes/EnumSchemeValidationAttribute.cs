namespace O24OpenAPI.WFO.Attributes;

[AttributeUsage(AttributeTargets.Property)]
internal class EnumSchemeValidationAttribute : Attribute
{
    public Type EnumType;

    public EnumSchemeValidationAttribute(Type EnumType)
    {
        this.EnumType = EnumType;
    }
}
