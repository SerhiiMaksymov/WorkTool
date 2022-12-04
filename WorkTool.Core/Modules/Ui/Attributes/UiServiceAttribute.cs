namespace WorkTool.Core.Modules.Ui.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class UiServiceAttribute : Attribute
{
    public Type Type { get; }

    public UiServiceAttribute(Type type)
    {
        Type = type.ThrowIfNull();
    }
}