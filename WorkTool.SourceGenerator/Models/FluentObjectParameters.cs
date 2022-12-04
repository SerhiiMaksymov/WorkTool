namespace WorkTool.SourceGenerator.Models;

public class FluentObjectParameters
{
    public TypeSyntax Type          { get; }
    public string     ExtensionName { get; }

    public FluentObjectParameters(TypeSyntax type, string extensionName)
    {
        Type          = type;
        ExtensionName = extensionName;
    }
}