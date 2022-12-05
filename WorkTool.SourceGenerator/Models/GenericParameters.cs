namespace WorkTool.SourceGenerator.Models;

public readonly struct GenericParameters
{
    private readonly List<TypeParameters> types;

    public GenericParameters(
        string alias,
        bool isNew,
        GenericOptionsType type,
        IEnumerable<TypeParameters> types
    )
    {
        Alias = alias;
        IsNew = isNew;
        Type = type;
        this.types = new List<TypeParameters>(types);
    }

    public string Alias { get; }
    public bool IsNew { get; }
    public GenericOptionsType Type { get; }
    public IEnumerable<TypeParameters> Types => types;

    public override string ToString()
    {
        if (!IsNew && Type == GenericOptionsType.None && !Types.Any())
        {
            return string.Empty;
        }

        var where = new List<string>(Types.Select(x => x.ToText()));

        if (IsNew)
        {
            where.Add("new()");
        }

        switch (Type)
        {
            case GenericOptionsType.Class:
                where.Add("class");

                break;
            case GenericOptionsType.Struct:
                where.Add("struct");

                break;
        }

        return $"{Alias} : {string.Join(", ", where)}";
    }
}
