namespace WorkTool.SourceGenerator.Models;

public class ModelObjectParameters
{
    public TypeSyntax Type { get; }

    public ModelObjectParameters(TypeSyntax type)
    {
        Type = type;
    }
}
