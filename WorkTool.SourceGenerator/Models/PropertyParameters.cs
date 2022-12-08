namespace WorkTool.SourceGenerator.Models;

public readonly struct PropertyParameters
{
    public PropertyParameters(
        string name,
        TypeParameters type,
        AccessModifier accessModifier,
        PropertyGetterOptions? getter,
        PropertySetterOptions? setter,
        string? next
    )
    {
        Name = name;
        Type = type;
        AccessModifier = accessModifier;
        Getter = getter;
        Setter = setter;
        Next = next;
    }

    public string Name { get; }
    public TypeParameters Type { get; }
    public AccessModifier AccessModifier { get; }
    public PropertyGetterOptions? Getter { get; }
    public PropertySetterOptions? Setter { get; }
    public string? Next { get; }

    public override string ToString()
    {
        var items = new List<string>
        {
            AccessModifier.AsString(),
            Type.ToText(),
            Name,
            "{",
            Getter.ToString() ?? string.Empty,
            Setter.ToString() ?? string.Empty,
            "}"
        };

        if (!string.IsNullOrWhiteSpace(Next))
        {
            items.Add("=");
            items.Add(Next);
        }

        return items.JoinString(" ");
    }
}
