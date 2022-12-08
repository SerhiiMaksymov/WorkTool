namespace WorkTool.SourceGenerator.Extensions;

public static class TypeInfoExtension
{
    public static TypeParameters ToTypeOptions(this CodeAnalysisTypeInfo type)
    {
        return type.Type.ThrowIfNull().ToTypeParameters();
    }

    public static bool EqualsType(this CodeAnalysisTypeInfo typeInfo, ITypeSymbol symbol)
    {
        var type = typeInfo.Type.ThrowIfNull();
        
        if (type.Name != symbol.Name)
        {
            return false;
        }

        if (type.ContainingNamespace.ToString() != symbol.ContainingNamespace.ToString())
        {
            return false;
        }

        return true;
    }
}
