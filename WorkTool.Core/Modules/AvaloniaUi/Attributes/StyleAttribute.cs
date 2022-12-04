namespace WorkTool.Core.Modules.AvaloniaUi.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class StyleAttribute : Attribute
{
    public string? Name { get; }

    public StyleAttribute()
    {
    }

    public StyleAttribute(string name)
    {
        Name = name;
    }
}