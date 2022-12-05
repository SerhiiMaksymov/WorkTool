namespace WorkTool.SourceGenerator.Models;

public readonly struct ConstructorParameters
{
    private readonly List<ArgumentParameters> arguments;

    public ConstructorParameters(
        AccessModifier accessModifier,
        TypeParameters type,
        IEnumerable<ArgumentParameters> arguments,
        string? body,
        string? @base
    )
    {
        this.arguments = new List<ArgumentParameters>(arguments);
        Type = type;
        AccessModifier = accessModifier;
        Body = body;
        Base = @base;
    }

    public ConstructorParameters(
        AccessModifier accessModifier,
        TypeParameters type,
        ArgumentParameters argument,
        string? body,
        string? @base
    )
    {
        arguments = new List<ArgumentParameters> { argument };

        Type = type;
        AccessModifier = accessModifier;
        Body = body;
        Base = @base;
    }

    public AccessModifier AccessModifier { get; }
    public TypeParameters Type { get; }
    public IEnumerable<ArgumentParameters> Arguments => arguments;
    public string? Body { get; }
    public string? Base { get; }

    public override string ToString()
    {
        return $@"{
            AccessModifier.AsString()
        } {
            Type.Name
        }({
            string.Join(", ", Arguments.Select(x => x.ToString()))
        }){
            (string.IsNullOrWhiteSpace(Base) ? string.Empty : $" : base({Base})")
        }
{{
    {
        Body
    }
}}";
    }
}
