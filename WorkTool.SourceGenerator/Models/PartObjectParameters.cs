namespace WorkTool.SourceGenerator.Models;

public class PartObjectParameters
{
    public TypeSyntax Type       { get; }
    public string     Name       { get; }
    public string     ClassName  { get; }
    public ObjectType ObjectType { get; }

    public PartObjectParameters(TypeSyntax type, string name, string className, ObjectType objectType)
    {
        Type       = type;
        Name       = name;
        ClassName  = className;
        ObjectType = objectType;
    }
}