namespace WorkTool.SourceGenerator.Core.Attributes;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class PartObjectAttribute : Attribute
{
    public Type Type { get; }
    public string Name { get; }
    public string ClassName { get; }
    public ObjectType ObjectType { get; }

    public PartObjectAttribute(Type type, string name, string className, ObjectType objectType)
    {
        Type = type;
        Name = name;
        ClassName = className;
        ObjectType = objectType;
    }
}
