namespace WorkTool.SourceGenerator.Models;

public readonly struct MethodParameters
{
    private readonly List<GenericParameters>  generics;
    private readonly List<ArgumentParameters> arguments;

    public MethodParameters(
    AccessModifier                  accessModifier,
    bool                            isStatic,
    TypeParameters                  outType,
    string                          name,
    IEnumerable<GenericParameters>  generics,
    IEnumerable<ArgumentParameters> arguments,
    string                          body)
    {
        IsStatic       = isStatic;
        AccessModifier = accessModifier;
        OutType        = outType;
        Name           = name;
        this.arguments = new List<ArgumentParameters>(arguments);
        this.generics  = new List<GenericParameters>(generics);
        Body           = body;
    }

    public MethodParameters(
    AccessModifier                  accessModifier,
    bool                            isStatic,
    TypeParameters                  outType,
    string                          name,
    GenericParameters               generic,
    IEnumerable<ArgumentParameters> arguments,
    string                          body)
    {
        IsStatic       = isStatic;
        AccessModifier = accessModifier;
        OutType        = outType;
        Name           = name;
        this.arguments = new List<ArgumentParameters>(arguments);

        generics = new List<GenericParameters>
        {
            generic
        };

        Body = body;
    }

    public AccessModifier                  AccessModifier { get; }
    public bool                            IsStatic       { get; }
    public TypeParameters                  OutType        { get; }
    public string                          Name           { get; }
    public IEnumerable<GenericParameters>  Generics       => generics;
    public IEnumerable<ArgumentParameters> Arguments      => arguments;
    public string                          Body           { get; }

    private string GetTextName()
    {
        if (!Generics.Any())
        {
            return Name;
        }

        return $"{Name}<{Generics.Select(x => x.Alias).JoinString(", ")}>";
    }

    private string GetHeader()
    {
        var items = new List<string>();
        items.Add(AccessModifier.AsString());

        if (IsStatic)
        {
            items.Add("static");
        }

        items.Add(OutType.ToText());
        items.Add(GetTextName());
        items.Add($"({Arguments.Select(x => x.ToString()).JoinString(", ")})");

        if (Generics.Any())
        {
            items.Add(
                Generics.Select(x => x.ToString())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => $"where {x}")
                    .JoinString(" "));
        }

        return items.JoinString(" ");
    }

    public override string ToString()
    {
        var lines = new List<string>();
        lines.Add($"        {GetHeader()}");
        lines.Add("        {");
        lines.Add($"            {Body}");
        lines.Add("        }");

        return lines.JoinString(Environment.NewLine);
    }
}