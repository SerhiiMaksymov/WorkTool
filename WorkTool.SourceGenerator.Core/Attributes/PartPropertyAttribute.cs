namespace WorkTool.SourceGenerator.Core.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class PartPropertyAttribute : Attribute
{
    public string Name { get; }

    public PartPropertyAttribute(string name)
    {
        Name = name;
    }
}
