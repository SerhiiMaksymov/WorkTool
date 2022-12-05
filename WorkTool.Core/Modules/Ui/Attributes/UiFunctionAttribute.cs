namespace WorkTool.Core.Modules.Ui.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class UiFunctionAttribute : Attribute
{
    public string Name { get; }

    public UiFunctionAttribute(string name)
    {
        Name = name;
    }
}
