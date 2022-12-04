namespace WorkTool.SourceGenerator.Core.Attributes;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class FluentObjectAttribute : Attribute
{
    public Type    Type          { get; }
    public string? ExtensionName { get; }

    public FluentObjectAttribute(Type type, string? extensionName)
    {
        Type          = type;
        ExtensionName = extensionName;
    }
}