namespace WorkTool.SourceGenerator.Core.Attributes;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class OptionsObjectAttribute : Attribute
{
    public Type    Type    { get; }
    public string? Postfix { get; }

    public OptionsObjectAttribute(Type type, string? postfix)
    {
        Type    = type;
        Postfix = postfix;
    }
}