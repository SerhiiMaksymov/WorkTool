namespace WorkTool.SourceGenerator.Models;

public class OptionsObjectParameters
{
    public TypeSyntax Type    { get; }
    public string?    Postfix { get; }

    public OptionsObjectParameters(TypeSyntax type, string? postfix)
    {
        Type = type;
        Postfix = postfix;
    }
}
