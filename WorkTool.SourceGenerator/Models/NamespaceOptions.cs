namespace WorkTool.SourceGenerator.Models;

public readonly struct NamespaceOptions : IEquatable<NamespaceOptions>
{
    private readonly List<NamespaceSegment> segments;

    public NamespaceOptions(IEnumerable<NamespaceSegment> segments)
    {
        this.segments = new List<NamespaceSegment>();
        this.segments.AddRange(segments);
    }

    public IEnumerable<NamespaceSegment> Segments => segments;

    public override int GetHashCode()
    {
        return segments.GetHashCode();
    }

    public override string ToString()
    {
        return string.Join(".", Segments);
    }

    public override bool Equals(object? obj)
    {
        return obj is NamespaceOptions other && Equals(other);
    }

    public bool Equals(NamespaceOptions other)
    {
        if (segments.Count != other.segments.Count)
        {
            return false;
        }

        for (var index = 0; index < segments.Count; index++)
        {
            if (!segments[index].Equals(other.segments[index]))
            {
                return false;
            }
        }

        return true;
    }
}
