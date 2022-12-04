namespace WorkTool.Core.Modules.Ui.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class UiCommandAttribute : Attribute
{
    public Type   Target       { get; }
    public string FunctionName { get; }
    public Type   Content      { get; }

    public UiCommandAttribute(Type target, string functionName, Type content)
    {
        Target       = target.ThrowIfNull();
        FunctionName = functionName.ThrowIfNull();
        Content      = content.ThrowIfNull();
    }
}