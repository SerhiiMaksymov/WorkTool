namespace WorkTool.SourceGenerator.Extensions;

public static class FluentObjectOptionsExtension
{
    public static string GetExtensionName(this FluentObjectParameters parameters, ITypeSymbol type)
    {
        if (parameters.ExtensionName == "null")
        {
            return $"{type.Name}Extension";
        }

        if (string.IsNullOrWhiteSpace(parameters.ExtensionName))
        {
            return $"{type.Name}Extension";
        }

        return parameters.ExtensionName;
    }
}