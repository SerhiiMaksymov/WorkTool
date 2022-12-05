namespace WorkTool.SourceGenerator.Models;

public readonly struct FieldParameters
{
    public FieldParameters(
        AccessModifier accessModifier,
        bool isReadonly,
        TypeParameters type,
        string name
    )
    {
        Name = name;
        Type = type;
        AccessModifier = accessModifier;
        IsReadonly = isReadonly;
    }

    public AccessModifier AccessModifier { get; }
    public bool IsReadonly { get; }
    public TypeParameters Type { get; }
    public string Name { get; }

    public override string ToString()
    {
        return $"{AccessModifier.AsString()} {(IsReadonly ? "readonly " : string.Empty)}{Type.ToText()} {Name}";
    }
}
