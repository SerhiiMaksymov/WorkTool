namespace WorkTool.SourceGenerator.Models;

public readonly struct TypeParameters : IEquatable<TypeParameters>, IEquatable<INamedTypeSymbol>
{
    public static readonly TypeParameters Void =
        new(null, "void", Enumerable.Empty<TypeParameters>());

    public static readonly IEnumerable<string> PrimitiveTypeNames = new[]
    {
        "bool",
        "byte",
        "sbyte",
        "char",
        "decimal",
        "double",
        "float",
        "int",
        "uint",
        "nint",
        "nuint",
        "long",
        "ulong",
        "short",
        "ushort",
        "object",
        "string",
        "dynamic"
    };

    private readonly List<TypeParameters> generics;

    public TypeParameters(
        NamespaceOptions? @namespace,
        string name,
        IEnumerable<TypeParameters> generics
    )
    {
        Namespace = @namespace;
        Name = name;
        this.generics = new List<TypeParameters>(generics);
    }

    public NamespaceOptions? Namespace { get; }
    public string Name { get; }
    public IEnumerable<TypeParameters> Generics => generics;

    public bool IsPrimitiveType()
    {
        return PrimitiveTypeNames.Contains(Name);
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            TypeParameters typeOptions => Equals(typeOptions),
            INamedTypeSymbol namedTypeSymbol => Equals(namedTypeSymbol),
            _ => false
        };
    }

    public bool Equals(INamedTypeSymbol? other)
    {
        if (other is null)
        {
            return false;
        }

        var typeOptions = other.ToTypeParameters();

        return Equals(typeOptions);
    }

    public bool Equals(TypeParameters other)
    {
        if (Name != other.Name)
        {
            return false;
        }

        if (!Nullable.Equals(Namespace, other.Namespace))
        {
            return false;
        }

        if (generics.Count != other.generics.Count)
        {
            return false;
        }

        for (var index = 0; index < generics.Count; index++)
        {
            if (!generics[index].Equals(other.generics[index]))
            {
                return false;
            }
        }

        return true;
    }

    public override string ToString()
    {
        if (IsPrimitiveType())
        {
            return Name;
        }

        if (Namespace.HasValue)
        {
            return $"{Namespace}.{Name}{(Generics.Any() ? $"<{string.Join(", ", Generics.Select(x => x.ToText()))}>" : string.Empty)}";
        }

        return Name;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = generics.GetHashCode();
            hashCode = (hashCode * 397) ^ (Namespace?.GetHashCode()).GetValueOrDefault();
            hashCode = (hashCode * 397) ^ (Name?.GetHashCode()).GetValueOrDefault();

            return hashCode;
        }
    }
}
