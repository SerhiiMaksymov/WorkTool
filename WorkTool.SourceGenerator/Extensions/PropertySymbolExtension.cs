namespace WorkTool.SourceGenerator.Extensions;

public static class PropertySymbolExtension
{
    public static PropertyParameters ToPropertyParameters(this IPropertySymbol propertySymbol)
    {
        return new PropertyParameters(
            propertySymbol.Name,
            propertySymbol.Type.ToTypeParameters(),
            propertySymbol.DeclaredAccessibility.ToAccessModifier(),
            propertySymbol.GetPropertyGetterOptions(),
            propertySymbol.GetPropertySetterOptions(),
            null);
    }

    public static PropertyGetterOptions? GetPropertyGetterOptions(this IPropertySymbol propertySymbol)
    {
        if (propertySymbol.GetMethod is null)
        {
            return null;
        }

        return new PropertyGetterOptions(propertySymbol.GetMethod.DeclaredAccessibility.ToAccessModifier(), null);
    }

    public static PropertySetterOptions? GetPropertySetterOptions(this IPropertySymbol propertySymbol)
    {
        if (propertySymbol.SetMethod is null)
        {
            return null;
        }

        return new PropertySetterOptions(propertySymbol.SetMethod.DeclaredAccessibility.ToAccessModifier(), null);
    }
}