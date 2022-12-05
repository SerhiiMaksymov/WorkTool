namespace WorkTool.SourceGenerator.Models;

public readonly struct ArgumentParameters
{
    public ArgumentParameters(bool isThis, TypeParameters type, string name)
    {
        IsThis = isThis;
        Type = type;
        Name = name;
    }

    public bool IsThis { get; }
    public TypeParameters Type { get; }
    public string Name { get; }

    public override string ToString()
    {
        return $"{(IsThis ? "this " : string.Empty)}{Type.ToText()} {Name}";
    }
}
