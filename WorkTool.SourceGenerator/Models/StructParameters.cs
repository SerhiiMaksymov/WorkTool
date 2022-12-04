namespace WorkTool.SourceGenerator.Models;

public readonly struct StructParameters
{
    private readonly List<GenericParameters>     generics;
    private readonly List<PropertyParameters>    properties;
    private readonly List<FieldParameters>       fields;
    private readonly List<ConstructorParameters> constructors;
    private readonly List<MethodParameters>      methods;
    private readonly List<OperatorParameters>    operators;

    public StructParameters(
    AccessModifier                     accessModifier,
    StructIdentifier                   identifier,
    TypeParameters                     type,
    IEnumerable<GenericParameters>     generics,
    IEnumerable<FieldParameters>       fields,
    IEnumerable<ConstructorParameters> constructors,
    IEnumerable<PropertyParameters>    properties,
    IEnumerable<MethodParameters>      methods,
    IEnumerable<OperatorParameters>    operators)
    {
        this.constructors = new List<ConstructorParameters>(constructors);
        this.fields       = new List<FieldParameters>(fields);
        this.properties   = new List<PropertyParameters>(properties);
        AccessModifier    = accessModifier;
        Identifier        = identifier;
        Type              = type;
        this.methods      = new List<MethodParameters>(methods);
        this.generics     = new List<GenericParameters>(generics);
        this.operators    = new List<OperatorParameters>(operators);
    }

    public AccessModifier                     AccessModifier { get; }
    public StructIdentifier                   Identifier     { get; }
    public TypeParameters                     Type           { get; }
    public IEnumerable<GenericParameters>     Generics       => generics;
    public IEnumerable<FieldParameters>       Fields         => fields;
    public IEnumerable<PropertyParameters>    Properties     => properties;
    public IEnumerable<ConstructorParameters> Constructors   => constructors;
    public IEnumerable<MethodParameters>      Methods        => methods;
    public IEnumerable<OperatorParameters>    Operators      => operators;

    private string GatTypeName()
    {
        if (!Generics.Any())
        {
            return Type.Name;
        }

        return $"{Type.Name}<{Generics.Select(x => x.Alias).JoinString(", ")}>";
    }

    private string GetHeader()
    {
        var items = new List<string>();
        items.Add(AccessModifier.AsString());

        switch (Identifier)
        {
            case StructIdentifier.None:
                break;
            case StructIdentifier.Readonly:
                items.Add(Identifier.AsString());

                break;
            case StructIdentifier.Partial:
                items.Add(Identifier.AsString());

                break;
            default: throw new ArgumentOutOfRangeException();
        }

        items.Add("struct");
        items.Add(GatTypeName());

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
            $"    {GetHeader()}",
            "     {"
        };

        if (Fields.Any())
        {
            lines.AddRange(Fields.Select(x => $"        {x};"));
        }

        if (Constructors.Any())
        {
            lines.AddRange(Constructors.Select(x => $"        {x}"));
        }

        if (Properties.Any())
        {
            lines.AddRange(Properties.Select(x => $"        {x}"));
        }

        if (Methods.Any())
        {
            lines.AddRange(Methods.Select(x => $"        {x}"));
        }

        if (Operators.Any())
        {
            lines.AddRange(Operators.Select(x => $"        {x}"));
        }

        lines.Add("    }");
        lines.Add("}");

        return lines.JoinString(Environment.NewLine);
    }
}