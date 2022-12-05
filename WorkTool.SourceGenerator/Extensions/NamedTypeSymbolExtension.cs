namespace WorkTool.SourceGenerator.Extensions;

public static class NamedTypeSymbolExtension
{
    public static IEnumerable<IPropertySymbol> GetProperties<TNamedTypeSymbol>(
        this TNamedTypeSymbol symbol
    ) where TNamedTypeSymbol : INamedTypeSymbol
    {
        return symbol.GetMembers().OfType<IPropertySymbol>();
    }
}
