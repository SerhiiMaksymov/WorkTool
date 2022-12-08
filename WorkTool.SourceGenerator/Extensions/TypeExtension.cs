namespace WorkTool.SourceGenerator.Extensions;

public static class TypeExtension
{
    public static readonly Regex TypeName =
        new("^(?<name>[a-zA-Z0-9_]+)(`\\d){0,1}$", RegexOptions.Compiled);

    public static TypeParameters ToTypeOptions(this Type type)
    {
        return type.ToTypeOptions(
            Array.Empty<TypeParameters>(),
            Enumerable.Empty<TypeParameters>(),
            new Ref<byte>(0)
        );
    }

    public static TypeParameters ToTypeOptions(
        this Type type,
        TypeParameters[] generics,
        IEnumerable<TypeParameters> ignores
    )
    {
        return type.ToTypeOptions(generics, ignores, new Ref<byte>(0));
    }

    private static TypeParameters ToTypeOptions(
        this Type type,
        TypeParameters[] generics,
        IEnumerable<TypeParameters> ignores,
        Ref<byte> index
    )
    {
        var nameMatch  = TypeName.Match(type.Name);
        var @namespace = type.Namespace.ThrowIfNull();

        var result = new TypeParameters(
            @namespace.ToNamespace(),
            nameMatch.Groups["name"].Value,
            type.GenericTypeArguments.Select(x => x.ToTypeOptions(generics, ignores, index))
        );

        if (ignores.Contains(result))
        {
            return generics[index.Value++];
        }

        return result;
    }
}
