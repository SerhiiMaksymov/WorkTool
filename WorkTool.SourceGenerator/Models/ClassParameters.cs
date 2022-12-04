namespace WorkTool.SourceGenerator.Models;

public readonly struct ClassParameters
{
    private readonly List<GenericParameters>     generics;
    private readonly List<PropertyParameters>    properties;
    private readonly List<FieldParameters>       fields;
    private readonly List<ConstructorParameters> constructors;
    private readonly List<MethodParameters>      methods;
    private readonly List<TypeParameters>        parents;

    public ClassParameters(
    AccessModifier                     accessModifier,
    bool                               isStatic,
    bool                               isPartial,
    TypeParameters                     type,
    bool                               isAbstract,
    IEnumerable<GenericParameters>     generics,
    IEnumerable<FieldParameters>       fields,
    IEnumerable<PropertyParameters>    properties,
    IEnumerable<ConstructorParameters> constructors,
    IEnumerable<MethodParameters>      methods,
    IEnumerable<TypeParameters>        parents)
    {
        AccessModifier    = accessModifier;
        IsStatic          = isStatic;
        IsPartial         = isPartial;
        IsAbstract        = isAbstract;
        this.constructors = new List<ConstructorParameters>(constructors);
        this.methods      = new List<MethodParameters>(methods);
        this.fields       = new List<FieldParameters>(fields);
        this.generics     = new List<GenericParameters>(generics);
        this.properties   = new List<PropertyParameters>(properties);
        Type              = type;
        this.parents      = new List<TypeParameters>(parents);
    }

    public AccessModifier                     AccessModifier { get; }
    public bool                               IsStatic       { get; }
    public bool                               IsPartial      { get; }
    public TypeParameters                     Type           { get; }
    public bool                               IsAbstract     { get; }
    public IEnumerable<GenericParameters>     Generics       => generics;
    public IEnumerable<FieldParameters>       Fields         => fields;
    public IEnumerable<PropertyParameters>    Properties     => properties;
    public IEnumerable<ConstructorParameters> Constructors   => constructors;
    public IEnumerable<MethodParameters>      Methods        => methods;
    public IEnumerable<TypeParameters>        Parents        => parents;

    private string GetTypeName()
    {
        if (!Generics.Any())
        {
            return Type.Name;
        }

        return $"{Type.Name}<{Generics.Select(x => x.Alias).JoinString(", ")}>";
    }

    private string GetHeader()
    {
        var items = new List<string>
        {
            AccessModifier.AsString()
        };

        if (IsAbstract)
        {
            items.Add("abstract");
        }

        if (IsStatic)
        {
            items.Add("static");
        }

        if (IsPartial)
        {
            items.Add("partial");
        }

        items.Add("class");
        items.Add(GetTypeName());

        if (Parents.Any())
        {
            items.Add(":");
            items.Add(Parents.Select(x => x.ToText()).JoinString(", "));
        }

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
        var lines = new List<string>
        {
            $"namespace {Type.Namespace}",
            "{",
            $"  {GetHeader()}",
            "   {"
        };

        if (Fields.Any())
        {
            lines.AddRange(Fields.Select(x => $"        {x};"));
        }

        if (Constructors.Any())
        {
            lines.AddRange(Constructors.Select(x => $"      {x}"));
        }

        if (Properties.Any())
        {
            lines.AddRange(Properties.Select(x => $"     {x}"));
        }

        if (Methods.Any())
        {
            lines.AddRange(Methods.Select(x => $"     {x}"));
        }

        lines.Add("   }");
        lines.Add("}");

        return lines.JoinString(Environment.NewLine);
    }
}