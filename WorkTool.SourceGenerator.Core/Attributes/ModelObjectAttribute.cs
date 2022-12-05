namespace WorkTool.SourceGenerator.Core.Attributes;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class ModelObjectAttribute : Attribute
{
    public Type Type { get; }

    public ModelObjectAttribute(Type type)
    {
        Type = type;
    }
}
