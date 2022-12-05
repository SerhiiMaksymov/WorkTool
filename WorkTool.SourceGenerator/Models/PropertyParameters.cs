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
        var items = new List<string>();
        items.Add(AccessModifier.AsString());
        items.Add(Type.ToText());
        items.Add(Name);
        items.Add("{");
        items.Add(Getter.ToString());
        items.Add(Setter.ToString());
        items.Add("}");

        if (!string.IsNullOrWhiteSpace(Next))
        {
            items.Add("=");
            items.Add(Next);
        }

        return items.JoinString(" ");
    }
}
