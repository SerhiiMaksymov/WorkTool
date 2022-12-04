namespace WorkTool.SourceGenerator.Extensions;

public static class TypeInfoExtension
{
    public static TypeParameters ToTypeOptions(this CodeAnalysisTypeInfo type)
    {
        return type.Type.ToTypeParameters();
    }

    public static bool EqualsType(this CodeAnalysisTypeInfo type, ITypeSymbol symbol)
    {
        if (type.Type.Name != symbol.Name)
        {
            return false;
        }

        if (type.Type.ContainingNamespace.ToString() != symbol.ContainingNamespace.ToString())
        {
            return false;
        }

        return true;
    }
}