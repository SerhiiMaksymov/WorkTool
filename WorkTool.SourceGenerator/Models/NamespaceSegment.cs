namespace WorkTool.SourceGenerator.Models;

public readonly struct NamespaceSegment : IEquatable<NamespaceSegment>
{
    public NamespaceSegment(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public bool Equals(NamespaceSegment other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is NamespaceSegment other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }
}
